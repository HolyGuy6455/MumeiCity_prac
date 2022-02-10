using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hittable : MonoBehaviour
{
    public Animator animator;
    public int HP = 10;
    public Collider hitBoxCollider;
    public delegate void HittableEvent(Hittable hittable);
    public event HittableEvent DeadEventHandler;
    public event HittableEvent HitEventHandler;
    [SerializeField] Dictionary<Tool.ToolType, int> effectiveToolDictionary;
    [SerializeField] int effectiveToolDictionaryLength;

    public void SetEffectiveTool(List<EffectiveTool> list){
        effectiveToolDictionary = new Dictionary<Tool.ToolType, int>();
        foreach (EffectiveTool effectiveTool in list){
            effectiveToolDictionary[effectiveTool.tool] = effectiveTool.damage;
        }
        effectiveToolDictionaryLength = effectiveToolDictionary.Count;
    }

    public void Hit(Tool.ToolType tool){
        int damage = 0;
        if(effectiveToolDictionary != null && effectiveToolDictionary.ContainsKey(tool)){
            damage = effectiveToolDictionary[tool];
        }
        if(damage == 0){
            return;
        }
        HP -= damage;

        if(HP<=0){
            animator.SetBool("isDead",true);
            if(!(DeadEventHandler is null))
                DeadEventHandler(this);
            return;
        }
        if(!(HitEventHandler is null)){
            HitEventHandler(this);
        }

        if(damage != 0){
            animator.SetTrigger("Hit");
        }
    } 

    public void Dead(){
        Destroy(this.gameObject);
    }

    void EnableHit(){
        this.hitBoxCollider.enabled = true;
    }
    void DisableHit(){
        this.hitBoxCollider.enabled = false;
    }
    
}
