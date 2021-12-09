using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PersonCommonAI : MonoBehaviour
{
    
    [SerializeField] AIDestinationSetter aIDestination; 
    [SerializeField] Animator animator;
    [SerializeField] Transform spriteTransform;
    [SerializeField] GameObject target;
    [SerializeField] Sence sence;
    [SerializeField] List<GameObject> pocketItemSlots;
    public PersonData personData = new PersonData();

    private GameObject findNearest(List<GameObject> pool){
        if(pool.Count == 0){
            return null;
        }
        GameObject result = null;
        float minDistance = float.MaxValue;
        foreach (GameObject target in pool){
            if(result == null){
                result = target;
                minDistance = Vector3.Distance(this.transform.position,target.transform.position);
                continue;
            }
            float distance = Vector3.Distance(this.transform.position,target.transform.position);
            if(distance < minDistance){
                result = target;
                minDistance = distance;
            }
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GetToolNowHold().name == "Axe"){
            LoseMyTarget();
            return;
        }
        if(this.target == null){
            ///////////////////////////////////////////////////////////////////////////
            // 주워올 아이템이 있는지 확인
            if(personData.items.Count < 6){
                List<GameObject> itemsForPickup = sence.whatTheySee.FindAll(
                    delegate(GameObject value){
                        ItemPickup itemPickup = value.GetComponent<ItemPickup>();
                        if(itemPickup == null){
                            return false;
                        }
                        return true;
                    }
                );

                GameObject nearestItem = findNearest(itemsForPickup);

                if(nearestItem != null){
                    animator.SetInteger("ThinkCode",1);
                    this.target = nearestItem;
                    aIDestination.target = this.target.transform;
                    // this.target.GetComponent<Hittable>().EntityDestroyEventHandler += LoseMyTarget;
                    animator.SetBool("HasAGoal",true);
                    return;
                }
            }
            ///////////////////////////////////////////////////////////////////////////
            // 베어낼 나무가 있는지 확인
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
            GameObject nearestTree = findNearest(treesForLogging);
            if(nearestTree != null){
                animator.SetInteger("ThinkCode",2);
                this.target = nearestTree;
                aIDestination.target = this.target.transform;
                this.target.GetComponent<Hittable>().EntityDestroyEventHandler += LoseMyTarget;
                animator.SetBool("HasAGoal",true);
                return;
            }
            ///////////////////////////////////////////////////////////////////////////
        }else{
            float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
            animator.SetFloat("DistanceToTheGoal",distance);
            float movementX = this.target.transform.position.x - this.transform.position.x;
            if(movementX <= -0.01f){
                spriteTransform.localScale = new Vector3(-1f,1f,1f);
            }else if(movementX >= 0.01f){
                spriteTransform.localScale = new Vector3(1f,1f,1f);
            }
        }
        
    }

    void PickupItem(){
        if(target == null){
            LoseMyTarget();
            return;
        }
        ItemPickup itemPickup = target.GetComponent<ItemPickup>();
        if(itemPickup == null){
            Debug.Log("Fail to Pick up the Item");
            return;
        }
        this.personData.items.Add(itemPickup.item);
        Destroy(target);
        LoseMyTarget();
        UpdateItemView();
    }

    void LoseMyTarget() {
        target = null;
        aIDestination.target = this.transform;
        animator.SetBool("HasAGoal",false);
    }

    public void UpdateItemView(){
        int length = this.personData.items.Count;
        for (int i = 0; i < length; i++){
            pocketItemSlots[i].SetActive(true);
            pocketItemSlots[i].transform.localPosition = new Vector3(((float)-length+1)/4 + ((float)i)/2,0.5f,0);
            pocketItemSlots[i].GetComponent<SpriteRenderer>().sprite = this.personData.items[i].itemPreset.icon;
        }
        for (int i = length; i < pocketItemSlots.Count; i++){
            pocketItemSlots[i].SetActive(false);
        }
    }
}
