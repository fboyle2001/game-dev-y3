/**
* Represents an action on a game object that the player can interact with
* and then trigger some event as a result
**/
public interface IInteractable {

    public void OnInteractPossible();

    public void OnInteractImpossible();
    
}
