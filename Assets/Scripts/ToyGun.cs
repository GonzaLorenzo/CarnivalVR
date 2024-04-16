using UnityEngine;

public class ToyGun : MonoBehaviour
{
    private Animator _gunAnimator;
    private AudioSource _audioSource;

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
    
    void Start()
    {
        _gunAnimator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        //_gunAnimator.SetTrigger("Fire");
        _gunAnimator.Play("Fire");
        RaycastHit hit;

        if (Physics.Raycast(_shootPos.position, _shootPos.forward, out hit, 1000))
        {
            Debug.DrawRay(_shootPos.position, _shootPos.forward * hit.distance, Color.red, 0.5f);

            Debug.DrawRay(hit.point, Vector3.up * 0.1f, Color.green, 0.5f);

            Instantiate(oldSphere, hit.point, Quaternion.identity);
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

    void CasingRelease()
    {
        if (!_casingExitLocation || !casingPrefab)
        { return; }

        GameObject casing;
        casing = Instantiate(casingPrefab, _casingExitLocation.position, _casingExitLocation.rotation) as GameObject;

        //Add force on casing to push it out
        casing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(_casingEjectPower * 0.7f, _casingEjectPower), (_casingExitLocation.position - _casingExitLocation.right * 0.3f - _casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy(tempCasing, destroyTimer);
    }
}
