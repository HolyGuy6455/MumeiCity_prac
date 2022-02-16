using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public Animator animator;
    public int HP = 10;
    public int HPMax = 10;
    public Collider hitBoxCollider;
    public UnityEvent DeadEventHandler;
    public UnityEvent HitEventHandler;
    [SerializeField] Dictionary<Tool.ToolType, EffectiveTool> effectiveToolDictionary;
    [SerializeField] int effectiveToolDictionaryLength;

    public void SetEffectiveTool(List<EffectiveTool> list){
        effectiveToolDictionary = new Dictionary<Tool.ToolType, EffectiveTool>();
        foreach (EffectiveTool effectiveTool in list){
            effectiveToolDictionary[effectiveTool.tool] = effectiveTool;
        }
        effectiveToolDictionaryLength = effectiveToolDictionary.Count;
    }

    public void Hit(Tool.ToolType tool){
        int damage = 0;
        if(effectiveToolDictionary != null && effectiveToolDictionary.ContainsKey(tool)){
            if(effectiveToolDictionary[tool].minHP < HP){
                damage = effectiveToolDictionary[tool].damage;
            }
        }
        if(damage == 0){
            return;
        }
        HP -= damage;

        if(HP<=0){
            animator.SetBool("isDead",true);
            animator.SetInteger("hp",HP);
            return;
        }
        if(!(HitEventHandler is null)){
            HitEventHandler.Invoke();
        }

        if(damage != 0){
            animator.SetTrigger("Hit");
        }
        animator.SetInteger("hp",HP);
    } 

    public void Dead(){
        if(!(DeadEventHandler is null))
            DeadEventHandler.Invoke();
        Destroy(this.gameObject);
    }

    void EnableHit(){
        if(hitBoxCollider != null)
            this.hitBoxCollider.enabled = true;
    }
    void DisableHit(){
        if(hitBoxCollider != null)
            this.hitBoxCollider.enabled = false;
    }
    
}
