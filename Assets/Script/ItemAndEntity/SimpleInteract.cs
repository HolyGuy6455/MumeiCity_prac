using UnityEngine;
using UnityEngine.Events;

public class SimpleInteract : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] UnityEvent interactEventHandler;

    public void Interact(){
        if(animator != null){
            animator.SetTrigger("Interact");
        }
        interactEventHandler.Invoke();
    }

}
