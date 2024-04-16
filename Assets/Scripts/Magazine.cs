using UnityEngine;

public class Magazine : MonoBehaviour
{
    private int _ammo = 12;

    [SerializeField] private GameObject _bullets;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Shoot()
    {
        _ammo--;
        if(_ammo == 0)
        {
            _bullets.SetActive(false);
        }
    }

    public bool IsLoaded()
    {
        if(_ammo > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
