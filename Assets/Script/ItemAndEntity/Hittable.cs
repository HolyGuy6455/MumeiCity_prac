using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class Hittable : MonoBehaviour
{
    public Animator animator;
    public int HP = 10;
    public int HPMax = 10;
    public Collider hitBoxCollider;
    public UnityEvent DeadEventHandler;
    public UnityEvent HitEventHandler;
    [SerializeField] Dictionary<ToolType, EffectiveTool> effectiveToolDictionary;
    [SerializeField] int effectiveToolDictionaryLength;
    [SerializeField] StudioEventEmitter hitSound;
    [SerializeField] StudioEventEmitter deadSound;

    public void SetEffectiveTool(List<EffectiveTool> list){
        effectiveToolDictionary = new Dictionary<ToolType, EffectiveTool>();
        foreach (EffectiveTool effectiveTool in list){
            effectiveToolDictionary[effectiveTool.tool] = effectiveTool;
        }
        effectiveToolDictionaryLength = effectiveToolDictionary.Count;
    }

    public void Hit(ToolType tool){
        Debug.Log("hit - " + this);
        int damage = 0;
        if(effectiveToolDictionary != null && effectiveToolDictionary.ContainsKey(tool)){
            if(effectiveToolDictionary[tool].minHP < HP){
                damage = effectiveToolDictionary[tool].damage;
            }
        }
        float distance = Vector3.Distance(GameManager.Instance.PlayerTransform.position,this.transform.position);
        hitSound.SetParameter("Distance",distance/20.0f);
        deadSound.SetParameter("Distance",distance/20.0f);
        
        Debug.Log("Distance!! " + (distance/20.0f));
        Debug.Log("damage - " + damage);
        if(damage == 0){
            return;
        }
        HP -= damage;
        hitSound.EventInstance.start();

        if(HP<=0){
            animator.SetBool("isDead",true);
            animator.SetFloat("hp",(float)HP/(float)HPMax);
            deadSound.EventInstance.start();
            return;
        }
        if(!(HitEventHandler is null)){
            HitEventHandler.Invoke();
        }

        if(damage != 0){
            animator.SetTrigger("Hit");
        }
        animator.SetFloat("hp",(float)HP/(float)HPMax);
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

    public void Restore(int value){
        if(HP < HPMax){
            HP += value;
        }
        animator.SetFloat("hp",(float)HP/(float)HPMax);
    }
    
}
