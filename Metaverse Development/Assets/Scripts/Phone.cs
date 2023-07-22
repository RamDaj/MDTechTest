using UnityEngine;

public class Phone : MonoBehaviour, IInteractable
{
    //Pick up Amy's phone
    public bool Interact (Interactor interactor)
    {
        GameManager.Instance.phoneAquired = true;
        gameObject.SetActive(false);
        return true;
    }

    //Can only be interacted with if the phone was never picked up
    public bool CanInteract()
    {
        return !GameManager.Instance.phoneAquired;
    }
}
