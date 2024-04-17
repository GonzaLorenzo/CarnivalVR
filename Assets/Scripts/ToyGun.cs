using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ToyGun : XRGrabInteractable
{
    [Header("Input Actions")]
    public InputActionReference leftPrimaryButton;
    public InputActionReference rightPrimaryButton;

    private Animator _gunAnimator;
    private AudioSource _audioSource;
    private bool hasMagazine = false;
    [SerializeField] private GunSocket _gunSocket;
    private Magazine _magazine;

    [Header("Prefabs")]
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject oldSphere;

    [Header("Settings")]
    [SerializeField] private float _casingEjectPower = 150f;
    [SerializeField] private float _muzzleFlashTimer = 2f;
    
    [Header("Location Refrences")]
    [SerializeField] private Transform _shootPos;
    [SerializeField] private Transform _casingExitLocation;

    [Header("AudioClips")]
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _emptySound;
    
    private XRBaseController m_Controller;

    void Start()
    {
        _gunAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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
        if(hasMagazine && _magazine.IsLoaded())
        {
            _gunAnimator.Play("Fire");
            _magazine.Shoot();

            RaycastHit hit;

            if (Physics.Raycast(_shootPos.position, _shootPos.forward, out hit, 1000))
            {
                Debug.DrawRay(_shootPos.position, _shootPos.forward * hit.distance, Color.red, 0.5f);

                Debug.DrawRay(hit.point, Vector3.up * 0.1f, Color.green, 0.5f);

                Instantiate(oldSphere, hit.point, Quaternion.identity);
            }
        }
        else
        {
            _audioSource.PlayOneShot(_emptySound);
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

    public void ReleaseMagazine(InputAction.CallbackContext context)
    {
        if(hasMagazine)
        {
            Debug.Log("Intentamos sacar el cargador");
            _gunSocket.ReleaseMagazine();
            hasMagazine = false;
        }
    }

    public void SetMagazine(Magazine magazine)
    {
        _magazine = magazine;
        hasMagazine = true;
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
