using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class Hittable : MonoBehaviour
{
    public Animator animator;
    public int HP = 10;
    public int HPMax = 10;
    public List<EffectiveTool> removalTool;
    public Collider hitBoxCollider;
    public UnityEvent DeadEventHandler;
    public UnityEvent HitEventHandler;
    [SerializeField] Dictionary<ToolType, EffectiveTool> effectiveToolDictionary;
    [SerializeField] int effectiveToolDictionaryLength;
    [SerializeField] StudioEventEmitter studioEventEmitter;
    [SerializeField] HitSound dead;
    enum HitSound{
        NONE    = 0,
        WHIP    = 1,
        ENTITY  = 2,
        STONE   = 3,
        CROP    = 4,
        WOOD    = 5,
        COLLAPSE= 6,
        CHEST   = 7,
        WHINE   = 8,
        PLAYER  = 9,
        BANG    = 10
    }

    private void Awake() {
        animator.SetFloat("hp",(float)HP/(float)HPMax);
    }

    public void SetEffectiveTool(List<EffectiveTool> list){
        effectiveToolDictionary = new Dictionary<ToolType, EffectiveTool>();
        foreach (EffectiveTool effectiveTool in list){
            effectiveToolDictionary[effectiveTool.tool] = effectiveTool;
        }
        effectiveToolDictionaryLength = effectiveToolDictionary.Count;
    }

    public void Hit(ToolType tool){
        int damage = 0;
        if(effectiveToolDictionary != null && effectiveToolDictionary.ContainsKey(tool)){
            if(effectiveToolDictionary[tool].minHP < HP){
                damage = effectiveToolDictionary[tool].damage;
            }
        }
        float distance = Vector3.Distance(GameManager.Instance.PlayerTransform.position,this.transform.position);
        if(studioEventEmitter != null)
            studioEventEmitter.SetParameter("Distance",distance/20.0f);
        
        if(damage == 0){
            return;
        }
        HP -= damage;
        if(studioEventEmitter != null)
            studioEventEmitter.EventInstance.start();

        if(HP<=0){
            animator.SetBool("isDead",true);
            animator.SetFloat("hp",(float)HP/(float)HPMax);
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

    public void DeadSound(){
        float distance = Vector3.Distance(GameManager.Instance.PlayerTransform.position,this.transform.position);
        studioEventEmitter.SetParameter("Distance",distance/20.0f);
        studioEventEmitter.SetParameter("HitTarget",(int)dead);
    }

    public void Dead(){
        GameManager.Instance.buildingManager.astarPath.Scan();
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
