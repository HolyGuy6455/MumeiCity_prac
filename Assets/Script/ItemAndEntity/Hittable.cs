using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hittable : MonoBehaviour, IEntityDestroyEvent
{
    public Animator animator;
    public int HP = 5;
    public Collider hitBoxCollider;
    public event IEntityDestroyEvent.VoidEvent EntityDestroyEventHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(){
        // Debug.Log("Hit!!");
        HP--;
        if(HP<=0){
            animator.SetBool("isDead",true);
            return;
        }
        animator.SetTrigger("Hit");
    }

    public void Dead(){
        if(!(EntityDestroyEventHandler is null))
            EntityDestroyEventHandler();

        // 임시 코드
        // GameManager.Instance.buildingManager.wholeBuildingSet.Remove(this.transform.gameObject);
        Destroy(this.gameObject);
    }
}
