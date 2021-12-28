using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Panda;

public class PersonBehavior : MonoBehaviour
{
    
    [SerializeField] AIDestinationSetter aIDestination; 
    [SerializeField] Animator animator;
    [SerializeField] Transform spriteTransform;
    [SerializeField] GameObject target;
    [SerializeField] Sence sence;
    [SerializeField] List<GameObject> pocketItemSlots;
    [SerializeField] BuildingObject workplace;
    [SerializeField] TimeEventQueueTicket sleepEvent;
    [SerializeField] string think;
    public PersonData personData = new PersonData();
    
    /*
     ThinkCode 각각 무엇을 의미하는지 적어두기
     1 : 아이템을 줍는다
     2 : 나무를 벤다
     3 : 아이템을 저장하러 직장으로 복귀
     4 : 퇴근

     */

    // Update is called once per frame
    void Update(){
        if(GameManager.Instance.GetToolNowHold().name == "Shovel"){
            LoseMyTarget();
            return;
        }

        // if(this.target != null){
        //     float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
        //     animator.SetFloat("DistanceToTheGoal",distance);
        //     float movementX = this.target.transform.position.x - this.transform.position.x;
        //     if(movementX <= -0.01f){
        //         spriteTransform.localScale = new Vector3(-1f,1f,1f);
        //     }else if(movementX >= 0.01f){
        //         spriteTransform.localScale = new Vector3(1f,1f,1f);
        //     }
        // }

        /*
        if(GameManager.Instance.GetToolNowHold().name == "Shovel"){
            LoseMyTarget();
            return;
        }

        this.target = GameManager.Instance.PlayerTransform.gameObject;
        animator.SetBool("HasAGoal",true);
        aIDestination.target = this.target.transform;

        float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
        animator.SetFloat("DistanceToTheGoal",distance);
        float movementX = this.target.transform.position.x - this.transform.position.x;
        if(movementX <= -0.01f){
            spriteTransform.localScale = new Vector3(-1f,1f,1f);
        }else if(movementX >= 0.01f){
            spriteTransform.localScale = new Vector3(1f,1f,1f);
        }

        

        if(this.personData.sleep){
            if(GameManager.Instance.timeManager.isDayTime()){
                this.personData.sleep = false;
                animator.SetBool("Sleep",false);
                Debug.Log("Good Morning!");
            }else{
                if(sleepEvent == null || !sleepEvent.isThisValid()){
                    string ticketName = "person"+this.personData.id+"_sleep";
                    sleepEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1,ticketName,SleepAndRecharging);
                    // Debug.Log("Waiting until "+this.personData.id+" fall asleep......");
                }
                return;
            }
        }
        TimeManager timeManager = GameManager.Instance.timeManager;
        if(this.target == null){
            Debug.Log("Searching Target");
            ///////////////////////////////////////////////////////////////////////////
            if(personData.items.Count >= 6 || (!timeManager.isDayTime() && personData.items.Count > 0)){
                // 주머니가 꽉 차있다면 직장으로 돌아간다
                // 일과시간이 끝났는데 주머니에 뭐가 있어도 직장에 아이템 넣으러 간다
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
            if(!timeManager.isDayTime() && this.personData.homeID != 0){
                // 일과시간이 끝났다
                Debug.Log("Going to Sleep");
                animator.SetInteger("ThinkCode",4);
                BuildingObject home = GameManager.Instance.buildingManager.FindBuildingObjectWithID(this.personData.homeID);
                this.target = home.gameObject;
                aIDestination.target = this.target.transform;
                animator.SetBool("HasAGoal",true);
                return;
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

        */
    }

    // 목표를 향해 이동한다
    [Task]
    void MoveToDestination(){
        float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
        animator.SetFloat("DistanceToTheGoal",distance);

        float movementX = this.target.transform.position.x - this.transform.position.x;
        if(movementX <= -0.01f){
            spriteTransform.localScale = new Vector3(-1f,1f,1f);
        }else if(movementX >= 0.01f){
            spriteTransform.localScale = new Vector3(1f,1f,1f);
        }

        if(distance < 2.0f)
            ThisTask.Succeed();
    }

    // 플레이어를 향해 간다
    [Task]
    void GoToPlayer(){
        this.target = GameManager.Instance.PlayerTransform.gameObject;
        animator.SetBool("HasAGoal",true);
        aIDestination.target = this.target.transform;
    }

    // 내 직장이 ~가 맞습니까?
    [Task]
    void IsMyWorkplace(string job){
        ThisTask.Complete(job.CompareTo(workplace.buildingData.buildingPreset.name) == 0);
    }

    [Task]
    void IsItDayTime(){
        ThisTask.Complete(GameManager.Instance.timeManager.IsItDayTime());
    }

    void CheckWorkplace(){
        if(workplace == null || workplace.buildingData.id != personData.workplaceID){
            workplace = GameManager.Instance.buildingManager.FindBuildingObjectWithID(this.personData.workplaceID);
        }
    }

    // 주워올만한 아이템이 있는지 탐색
    [Task]
    void SearchItemToPickUp(){
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

        if(nearestItem != null){
            think = "Going Pick Up " + nearestItem;
            animator.SetInteger("ThinkCode",1);
            this.target = nearestItem;
            aIDestination.target = this.target.transform;
            animator.SetBool("HasAGoal",true);
            ThisTask.Succeed();
        }else{
            ThisTask.Fail();
        }
    }

    // 아이템을 줍기
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

    // 아이템을 얼마나 가지고 있나요
    [Task]
    void DoIHaveItemMoreThen(int amount){
        ThisTask.Complete( personData.items.Count > amount );
    }

    // 아이템 갖다 넣으러 가기
    [Task]
    void GoPutInItem(){
        animator.SetInteger("ThinkCode",3);
        CheckWorkplace();
        this.target = workplace.gameObject;
        aIDestination.target = workplace.gameObject.transform;
        animator.SetBool("HasAGoal",true);
        ThisTask.Succeed();
    }

    // 아이템을 갖다 넣기
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

    [Task]
    void SearchingTreeToChop(){
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
            think = "Going Chop Chop " + nearestTree;
            animator.SetInteger("ThinkCode",2);
            this.target = nearestTree;
            aIDestination.target = this.target.transform;
            this.target.GetComponent<Hittable>().EntityDestroyEventHandler += LoseMyTarget;
            animator.SetBool("HasAGoal",true);
            ThisTask.Succeed();
        }else{
            ThisTask.Fail();
        }
    }

    public void Sleep(){
        LoseMyTarget();
        this.personData.sleep = true;
        animator.SetBool("Sleep",true);
        think = "Go to sleep! Good Night!";
    }

    void LoseMyTarget() {
        target = null;
        aIDestination.target = this.transform;
        animator.SetBool("HasAGoal",false);
        // animator.SetInteger("ThinkCode",0);
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

    public void SleepAndRecharging(){
        if(this.personData.stamina < 100){
            this.personData.stamina+=5;
        }
    }

    public void Tire(){
        if(this.personData.stamina > 0){
            this.personData.stamina--;
        }
    }
}
