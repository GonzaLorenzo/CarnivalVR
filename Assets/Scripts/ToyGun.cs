using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyGun : MonoBehaviour
{
    [SerializeField] private Transform _shootPos;
    public GameObject oldSphere;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        Debug.Log("Hola putos");

        RaycastHit hit;

        if (Physics.Raycast(_shootPos.position, _shootPos.forward, out hit, 1000))
        {
            Debug.DrawRay(_shootPos.position, _shootPos.forward * hit.distance, Color.red, 0.5f);

            Debug.DrawRay(hit.point, Vector3.up * 0.1f, Color.green, 0.5f);

            Instantiate(oldSphere, hit.point, Quaternion.identity);
        }
    }
}
