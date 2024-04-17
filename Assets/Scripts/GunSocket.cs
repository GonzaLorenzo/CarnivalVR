using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GunSocket : MonoBehaviour
{
    [SerializeField] private ToyGun _gunParent;
    private XRSocketInteractor _gunSocket;

    void Start()
    {
        _gunSocket = GetComponent<XRSocketInteractor>();
    }

    public void SendMagazineReference()
    {
        IXRSelectInteractable socketMagazine = _gunSocket.GetOldestInteractableSelected();
        Magazine magazine = socketMagazine.transform.GetComponent<Magazine>();
        magazine.DisableCollider();
        _gunParent.SetMagazine(magazine);
    }

    public void ReleaseMagazine()
    {
        IXRSelectInteractable socketMagazine = _gunSocket.GetOldestInteractableSelected();
        //_gunSocket.GetComponent<SphereCollider>().enabled = false;
        _gunSocket.interactionManager.SelectExit(_gunSocket, socketMagazine);
        socketMagazine.transform.GetComponent<Magazine>().ReActivateCollider();
        //StartCoroutine("ActivateSocket");
    }

    
    IEnumerator ActivateSocket()
    {
        yield return new WaitForSeconds(5f);
        _gunSocket.GetComponent<SphereCollider>().enabled = true;
    }

}


