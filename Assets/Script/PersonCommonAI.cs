using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PersonCommonAI : MonoBehaviour
{
    [SerializeField]
    AIDestinationSetter aIDestination; 
    [SerializeField]
    Animator animator;
    [SerializeField]
    Hittable target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.target == null){
            HashSet<GameObject>.Enumerator enumerator= GameManager.Instance.buildingManager.wholeBuildingSet.GetEnumerator();
            GameObject targetObject = null;
            while (enumerator.MoveNext()){
                targetObject = enumerator.Current;

                this.target = targetObject.GetComponent<Hittable>();
                if(this.target != null){
                    aIDestination.target = targetObject.transform;
                    this.target.EntityDestroyEventHandler += LoseMyTarget;
                    animator.SetBool("HasAGoal",true);
                    break;
                }
            }
        }
        if(this.target != null){
            float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
            animator.SetFloat("DistanceToTheGoal",distance);
        }else{
            animator.SetFloat("DistanceToTheGoal",0);
        }
    }

    void LoseMyTarget() {
        target = null;
        aIDestination.target = null;
        animator.SetBool("HasAGoal",false);
    }
}
