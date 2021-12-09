using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PersonCommonAI : MonoBehaviour
{
    [SerializeField] AIDestinationSetter aIDestination; 
    [SerializeField] Animator animator;
    [SerializeField] GameObject target;
    public PersonData personData = new PersonData();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.target == null){
            List<GameObject> treesForLogging= GameManager.Instance.buildingManager.wholeBuildingSet().FindAll(
                delegate(GameObject value){
                    BuildingObject buildingObject = value.GetComponent<BuildingObject>();
                    if(buildingObject == null){
                        return false;
                    }
                    BuildingPreset preset = buildingObject.buildingData.buildingPreset;
                    return preset.hasAttribute("Log");
                }
            );
            Debug.Log("We Found trees For Logging "+treesForLogging.Count);

            float minDistance = float.MaxValue;
            foreach (GameObject tree in treesForLogging){
                if(this.target == null){
                    this.target = tree;
                    minDistance = Vector3.Distance(this.transform.position,tree.transform.position);
                    continue;
                }
                float distance = Vector3.Distance(this.transform.position,tree.transform.position);
                if(distance < minDistance){
                    this.target = tree;
                    minDistance = distance;
                }
            }
            if(this.target != null){
                aIDestination.target = this.target.transform;
                this.target.GetComponent<Hittable>().EntityDestroyEventHandler += LoseMyTarget;
                animator.SetBool("HasAGoal",true);
            }
        }else{
            float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
            animator.SetFloat("DistanceToTheGoal",distance);
            float movementX = this.target.transform.position.x - this.transform.position.x;
            if(movementX <= -0.01f){
                transform.localScale = new Vector3(-1f,1f,1f);
            }else if(movementX >= 0.01f){
                transform.localScale = new Vector3(1f,1f,1f);
            }
        }
        
    }

    void LoseMyTarget() {
        target = null;
        aIDestination.target = null;
        animator.SetBool("HasAGoal",false);
    }
}
