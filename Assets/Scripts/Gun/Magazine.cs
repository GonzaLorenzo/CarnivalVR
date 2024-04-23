using System.Collections;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    private int _ammo = 12;

    [SerializeField] private GameObject _bullets;

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
        return _ammo == 12 ? true : false;
    }

    public bool IsLoaded()
    {
        return _ammo > 0 ? true : false;
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
