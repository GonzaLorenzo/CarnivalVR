using System.Collections;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    private int _ammo = 15;
    [SerializeField] private MagazineManager _manager;

    [SerializeField] private GameObject _bullets;

    void Start()
    {
        _manager = GameObject.Find("GameManager").GetComponent<MagazineManager>();
        _manager.AddMagazine(this, transform.position);
    }

    public void Shoot()
    {
        _ammo--;
        if(_ammo == 0)
        {
            _bullets.SetActive(false);
        }
    }

    public bool isFull()
    {
        return _ammo == 15 ? true : false;
    }

    public bool IsLoaded()
    {
        if(_ammo > 0)
        {
            return true;
        }
        else
        {
            _manager.MagazineEmptied(this);
            return false;     
        }
    }

    public void DisableCollider()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    public void ReActivateCollider()
    {
        StartCoroutine("ActivateCollider");
    }

    IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(.2f);
        GetComponent<BoxCollider>().enabled = true;
    }
}
