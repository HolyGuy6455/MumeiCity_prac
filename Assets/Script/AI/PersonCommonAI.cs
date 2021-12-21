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
    [SerializeField] BuildingObject workplace;
    public PersonData personData = new PersonData();
    
    /*
     ThinkCode 각각 무엇을 의미하는지 적어두기
     1 : 아이템을 줍는다
     2 : 나무를 벤다
     3 : 아이템을 저장하러 직장으로 복귀

     */

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GetToolNowHold().name == "Axe"){
            LoseMyTarget();
            return;
        }
        TimeManager.TimeSlot timeSlot = GameManager.Instance.timeManager._timeSlot;
        if(timeSlot == TimeManager.TimeSlot.EVENING || timeSlot == TimeManager.TimeSlot.NIGHT){
            LoseMyTarget();
            return;
        }else if(this.target == null){
            Debug.Log("Searching Target");
            ///////////////////////////////////////////////////////////////////////////
            if(personData.items.Count >= 6){
                // 주머니가 꽉 차있다면 직장으로 돌아간다
                Debug.Log("Going Put In");
                animator.SetInteger("ThinkCode",3);
                CheckWorkplace();
                this.target = workplace.gameObject;
                aIDestination.target = workplace.gameObject.transform;
                animator.SetBool("HasAGoal",true);
                return;
            }else{
                // 주머니가 꽉 차있지 않다면 주변 아이템을 줍는다
                sence.filter = delegate(GameObject value){
                    if(value == null){
                        return false;
                    }
                    ItemPickup itemPickup = value.GetComponent<ItemPickup>();
                    if(itemPickup == null){
                        return false;
                    }
                    return true;
                };

                GameObject nearestItem = sence.FindNearest(this.transform.position);
                Debug.Log("nearestItem : " + nearestItem);

                if(nearestItem != null){
                    Debug.Log("Going Pick Up");
                    animator.SetInteger("ThinkCode",1);
                    this.target = nearestItem;
                    aIDestination.target = this.target.transform;
                    animator.SetBool("HasAGoal",true);
                    return;
                }
            }
            ///////////////////////////////////////////////////////////////////////////
            // 베어낼 나무가 있는지 확인
            sence.filter =
                delegate(GameObject value){
                    if(value == null){
                        return false;
                    }
                    BuildingObject buildingObject = value.GetComponent<BuildingObject>();
                    if(buildingObject == null){
                        return false;
                    }
                    BuildingPreset preset = buildingObject.buildingData.buildingPreset;
                    return preset.attributes.Contains("Log");
                };
            GameObject nearestTree = sence.FindNearest(this.transform.position);
            if(nearestTree != null){
                Debug.Log("Going Chop Chop");
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

    void CheckWorkplace(){
        if(workplace == null || workplace.buildingData.id != personData.workplaceID){
            workplace = GameManager.Instance.buildingManager.FindBuildingObjectWithID(this.personData.workplaceID);
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
        this.personData.items.Add(itemPickup.ProcessToItemSlotData());
        Destroy(target);
        LoseMyTarget();
        UpdateItemView();
    }

    void PutInItem(){
        this.personData.items = this.personData.items.FindAll(x => x.code != 0);

        if(this.personData.items.Count > 0){
            ItemSlotData itemSlotData = this.personData.items[0];
            workplace.buildingData.AddItem(itemSlotData);
            UpdateItemView();
        }else{
            LoseMyTarget();
        }
    }

    void LoseMyTarget() {
        target = null;
        aIDestination.target = this.transform;
        animator.SetBool("HasAGoal",false);
        animator.SetInteger("ThinkCode",0);
    }

    public void UpdateItemView(){
        this.personData.items = this.personData.items.FindAll(x => x.code != 0);

        int length = this.personData.items.Count;
        for (int i = 0; i < length; i++){
            pocketItemSlots[i].SetActive(true);
            pocketItemSlots[i].transform.localPosition = new Vector3(((float)-length+1)/4 + ((float)i)/2,0.5f,0);
            pocketItemSlots[i].GetComponent<SpriteRenderer>().sprite = this.personData.items[i].itemPreset.itemSprite;
        }
        for (int i = length; i < pocketItemSlots.Count; i++){
            pocketItemSlots[i].SetActive(false);
        }
    }
}
