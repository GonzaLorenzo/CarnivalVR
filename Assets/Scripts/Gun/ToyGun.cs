using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ToyGun : XRGrabInteractable
{
    [Header("Input Actions")]
    public InputActionReference leftPrimaryButton;
    public InputActionReference rightPrimaryButton;

    private Rigidbody _rb;
    private Animator _gunAnimator;
    private AudioSource _audioSource;
    private bool _hasMagazine = false;
    private bool _isCharged = false;
    [SerializeField] private GunSocket _gunSocket;
    private Magazine _magazine;

    [Header("Prefabs")]
    [SerializeField] private LineRenderer _laserRenderer;
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public ParticleSystem muzzleFlashPrefab;
    public ParticleSystem impactParticlePrefab;
    public TrailRenderer bulletTrail;

    [Header("Settings")]
    [SerializeField] private float _recoilForce;
    [SerializeField] private float _casingEjectPower = 150f;
    [SerializeField] LayerMask _laserLayerMask;
    
    [Header("Location Refrences")]
    [SerializeField] private Transform _shootPos;
    [SerializeField] private Transform _laserSightPos;
    [SerializeField] private Transform _casingExitLocation;

    [Header("AudioClips")]
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private AudioClip _emptySound;
    
    private XRBaseController m_Controller;

    void Start()
    {
        _gunAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        RaycastHit hit;
        _laserRenderer.SetPosition(0, _laserSightPos.position);
        if (Physics.Raycast(_shootPos.position, transform.forward, out hit, 30, _laserLayerMask))
        {
            _laserRenderer.SetPosition(1, hit.point);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        SetParentToXRRig();

        var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        m_Controller = controllerInteractor.xrController;
        m_Controller.SendHapticImpulse(1, 0.5f);

        SetPrimaryButtonEvent();
        _laserRenderer.enabled = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        SetParentToWorld();

        leftPrimaryButton.action.started -= ReleaseMagazine;
        rightPrimaryButton.action.started -= ReleaseMagazine;
        
        _laserRenderer.enabled = false;
    }

    public void Shoot()
    {
        //_gunAnimator.SetTrigger("Fire");
        if(_hasMagazine && _magazine.IsLoaded() && _isCharged)
        {
            _gunAnimator.Play("Fire");
            
            _magazine.Shoot();
            _rb.AddForceAtPosition(Vector3.up * _recoilForce, _shootPos.position, ForceMode.Force);

            RaycastHit hit;

            if (Physics.Raycast(_shootPos.position, _shootPos.forward, out hit, 100))
            {
                Debug.DrawRay(_shootPos.position, _shootPos.forward * hit.distance, Color.red, 0.5f);

                Debug.DrawRay(hit.point, Vector3.up * 0.1f, Color.green, 0.5f);

                TrailRenderer trail = Instantiate(bulletTrail, _shootPos.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
                //Instantiate(oldSphere, hit.point, Quaternion.identity);
                IShootable target;
                if(hit.transform.TryGetComponent<IShootable>(out target))
                {
                    target.GetShot();
                }
            }
        }
        else
        {
            _audioSource.PlayOneShot(_emptySound);
        }

        if(!_magazine.IsLoaded())
        {
            _isCharged = false;
            _gunAnimator.SetBool("NeedsCharge", true);
        }
        
    }

    public void Fire()
    {
        _audioSource.PlayOneShot(_shootSound);
        /* if (muzzleFlashPrefab)
        {
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, _shootPos.position, _shootPos.rotation);

            Destroy(tempFlash, _muzzleFlashTimer);
        } */
        m_Controller.SendHapticImpulse(.4f, 0.12f);
        Instantiate(muzzleFlashPrefab, _shootPos.position, _shootPos.rotation);
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Instantiate(impactParticlePrefab, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }

    public void CasingRelease()
    {
        GameObject casing;
        casing = Instantiate(casingPrefab, _casingExitLocation.position, _casingExitLocation.rotation);
        
        casing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(_casingEjectPower * 0.7f, _casingEjectPower), (_casingExitLocation.position - _casingExitLocation.right * 0.3f - _casingExitLocation.up * 0.6f), 1f);
    }

    private void BulletRelease()
    {
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, _casingExitLocation.position, _casingExitLocation.rotation);

        bullet.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(_casingEjectPower * 0.7f, _casingEjectPower), (_casingExitLocation.position - _casingExitLocation.right * 0.3f - _casingExitLocation.up * 0.6f), 1f);
    }

    public void ChargeSlide()
    {
        if(_hasMagazine)
        {
            _gunAnimator.SetBool("NeedsCharge", false);
            
            if(_isCharged && _magazine.IsLoaded())
            {
                _magazine.Shoot();
                BulletRelease();
            }

            _isCharged = true;
        }

        _audioSource.PlayOneShot(_reloadSound);
    }

    public void ReleaseMagazine(InputAction.CallbackContext context)
    {
        if(_hasMagazine)
        {
            _gunSocket.ReleaseMagazine();
            _hasMagazine = false;
        }
    }

    public void SetMagazine(Magazine magazine)
    {
        _magazine = magazine;
        _hasMagazine = true;
        _audioSource.PlayOneShot(_reloadSound);
    }

    public void SetPrimaryButtonEvent()
    {
        if(m_Controller.name == "Right Controller")
        {
            leftPrimaryButton.action.started -= ReleaseMagazine;
            rightPrimaryButton.action.started += ReleaseMagazine;
        }
        else if(m_Controller.name == "Left Controller")
        {
            rightPrimaryButton.action.started -= ReleaseMagazine;
            leftPrimaryButton.action.started += ReleaseMagazine;
        }    
    }

    public void SetParentToXRRig()
    {
        transform.SetParent(selectingInteractor.transform);
    }
 
    public void SetParentToWorld()
    {
        transform.SetParent(null);
    }
}
