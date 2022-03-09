using UnityEngine;

public class SimpleInteract : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] string achievementStr;

    public void Interact(){
        animator.SetTrigger("Interact");
        GameManager.Instance.achievementManager.AddTrial(achievementStr,1);
    }

}
