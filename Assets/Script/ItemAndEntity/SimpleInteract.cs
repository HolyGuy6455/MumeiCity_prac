using UnityEngine;

public class SimpleInteract : MonoBehaviour{
    [SerializeField] Animator animator;

    public void Interact(){
        animator.SetTrigger("Interact");
    }

}
