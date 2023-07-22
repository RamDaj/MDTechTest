public interface IInteractable
{
    //Executed whenever the player clicks the interact key facing this object
    //Interactor is the player interacting.
    public bool Interact(Interactor interactor);

    //Returns true if the object can be currently interacted with
    public bool CanInteract();
}