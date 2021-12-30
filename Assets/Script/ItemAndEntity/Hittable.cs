using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hittable : MonoBehaviour
{
    public Animator animator;
    public int HP = 10;
    public Collider hitBoxCollider;
    public delegate void EntityEvent(Component component);
    public event EntityEvent EntityDestroyEventHandler;
    public event EntityEvent EntityHitEventHandler;
    public Tool.ToolType effectiveTool;

    public void Hit(Tool.ToolType tool){
        if(tool == effectiveTool){
            HP -= 3;
        }else{
            HP -= 1;
        }
        if(HP<=0){
            animator.SetBool("isDead",true);
            return;
        }
        if(!(EntityHitEventHandler is null))
            EntityHitEventHandler(this);
        animator.SetTrigger("Hit");
    }

    public void Dead(){
        if(!(EntityDestroyEventHandler is null))
            EntityDestroyEventHandler(this);

        // 임시 코드
        Destroy(this.gameObject);
    }
}
