using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditView : MonoBehaviour{
    [SerializeField] Animator animator;

    public void SetVisible(bool value){
        animator.SetBool("Open",value);
    }
}
