using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;
    //Add or Remove an InteractionEvent component to this game object
    public bool useEvents;
    // this function will be called from our player
    public void BaseInteract(){
        if(useEvents){
            GetComponent<InteractEvent>().OnInteract.Invoke();
        }
        Interact();
    }
    protected virtual void Interact(){

    }
}
