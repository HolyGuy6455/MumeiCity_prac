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
    [SerializeField] Dictionary<Tool.ToolType, int> effectiveToolDictionary;

    public void SetEffectiveTool(List<EffectiveTool> list){
        effectiveToolDictionary = new Dictionary<Tool.ToolType, int>();
        foreach (EffectiveTool effectiveTool in list){
            effectiveToolDictionary[effectiveTool.tool] = effectiveTool.damage;
        }
    }

    public void Hit(Tool.ToolType tool){
        int damage = 0;
        if(effectiveToolDictionary.ContainsKey(tool)){
            damage = effectiveToolDictionary[tool];
            Debug.Log("damage - " + effectiveToolDictionary[tool]);
        }
        HP -= damage;

        if(HP<=0){
            animator.SetBool("isDead",true);
            return;
        }
        if(!(EntityHitEventHandler is null)){
            EntityHitEventHandler(this);
        }

        if(damage != 0){
            animator.SetTrigger("Hit");
        }
    } 

    public void Dead(){
        if(!(EntityDestroyEventHandler is null))
            EntityDestroyEventHandler(this);

        // 임시 코드
        Destroy(this.gameObject);
    }

    void EnableHit(){
        this.hitBoxCollider.enabled = true;
    }
    void DisableHit(){
        this.hitBoxCollider.enabled = false;
    }
    
}
