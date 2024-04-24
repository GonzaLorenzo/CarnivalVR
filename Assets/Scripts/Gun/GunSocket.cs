using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GunSocket : XRSocketInteractor
{
    public string magazineTag;
    [SerializeField] private ToyGun _gunParent;

    public void SendMagazineReference()
    {
        IXRSelectInteractable socketMagazine = this.GetOldestInteractableSelected();
        Magazine magazine = socketMagazine.transform.GetComponent<Magazine>();
        if(magazine != null)
        {
            magazine.DisableCollider();
            _gunParent.SetMagazine(magazine);
        }
        else
        {
            IXRSelectInteractable nullMagazine = this.GetOldestInteractableSelected();
            interactionManager.SelectExit(this, nullMagazine);
        }
        
    }

    public void ReleaseMagazine()
    {
        IXRSelectInteractable socketMagazine = this.GetOldestInteractableSelected();
        //this.GetComponent<SphereCollider>().enabled = false;
        interactionManager.SelectExit(this, socketMagazine);
        socketMagazine.transform.GetComponent<Magazine>().ReActivateCollider();
        //StartCoroutine("ActivateSocket");
    }

    
    /* IEnumerator ActivateSocket()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<SphereCollider>().enabled = true;
    } */

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(magazineTag);
    }
}


