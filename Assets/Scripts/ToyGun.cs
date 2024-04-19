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
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject oldSphere;

    [Header("Settings")]
    [SerializeField] private float _recoilForce;
    [SerializeField] private float _casingEjectPower = 150f;
    [SerializeField] private float _muzzleFlashTimer = 2f;
    
    [Header("Location Refrences")]
    [SerializeField] private Transform _shootPos;
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

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        m_Controller = controllerInteractor.xrController;
        m_Controller.SendHapticImpulse(1, 0.5f);
        Debug.Log(m_Controller);

        SetPrimaryButtonEvent();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        leftPrimaryButton.action.started -= ReleaseMagazine;
        rightPrimaryButton.action.started -= ReleaseMagazine;
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

            if (Physics.Raycast(_shootPos.position, _shootPos.forward, out hit, 1000))
            {
                Debug.DrawRay(_shootPos.position, _shootPos.forward * hit.distance, Color.red, 0.5f);

                Debug.DrawRay(hit.point, Vector3.up * 0.1f, Color.green, 0.5f);

                Instantiate(oldSphere, hit.point, Quaternion.identity);
            }

            //_rb.AddForceAtPosition(Vector3.up * 2, _shootPos.position, ForceMode.Impulse);
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
        if (muzzleFlashPrefab)
        {
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, _shootPos.position, _shootPos.rotation);

            Destroy(tempFlash, _muzzleFlashTimer);
        }
    }

    public void CasingRelease()
    {
        if (!_casingExitLocation || !casingPrefab)
        { return; }

        GameObject casing;
        casing = Instantiate(casingPrefab, _casingExitLocation.position, _casingExitLocation.rotation);

        casing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(_casingEjectPower * 0.7f, _casingEjectPower), (_casingExitLocation.position - _casingExitLocation.right * 0.3f - _casingExitLocation.up * 0.6f), 1f);
        //casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy(tempCasing, destroyTimer);
    }

    public void BulletRelease()
    {
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, _casingExitLocation.position, _casingExitLocation.rotation);

        bullet.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(_casingEjectPower * 0.7f, _casingEjectPower), (_casingExitLocation.position - _casingExitLocation.right * 0.3f - _casingExitLocation.up * 0.6f), 1f);
    }

    public void ChargeSlide()
    {
        if(_hasMagazine)
        {
            _isCharged = true;
            _gunAnimator.SetBool("NeedsCharge", false);
            if(_magazine.IsLoaded())
            {
                _magazine.Shoot();
                CasingRelease();
            }
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
}
