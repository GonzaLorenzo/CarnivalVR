using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction;
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

        _gunParent.SetMagazine(magazine);
    }

    public void ReleaseMagazine()
    {
        IXRSelectInteractable socketMagazine = _gunSocket.GetOldestInteractableSelected();

        _gunSocket.interactionManager.SelectExit(_gunSocket, socketMagazine);
    }


}
