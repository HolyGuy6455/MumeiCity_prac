using UnityEngine;
using UnityEngine.Events;

public class SimpleInteract : MonoBehaviour{
    [SerializeField] UnityEvent interactEventHandler;

    public void Interact(){
        interactEventHandler.Invoke();
    }

}
