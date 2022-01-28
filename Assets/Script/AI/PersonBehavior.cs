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
    [SerializeField] HitCollision hitCollision;
    public PersonData personData = new PersonData();
    
    /*
     ThinkCode 각각 무엇을 의미하는지 적어두기
     1 : 아이템을 줍는다
     2 : 나무를 벤다
     3 : 아이템을 저장하러 직장으로 복귀
     4 : 퇴근
     5 : 필요한 아이템을 꺼내러 간다

     */

    // Update is called once per frame
    void Update(){
        if(GameManager.Instance.GetToolNowHold().name == "Shovel"){
            LoseMyTarget();
            return;
        }
    }

    // 목표를 향해 이동한다
    [Task]
    void MoveToDestination(){
        if(this.target == null){
            ThisTask.Fail();
            return;
        }
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
        try{
            ThisTask.Complete(job.CompareTo(workplace.buildingData.buildingPreset.name) == 0);
        }
        catch (System.NullReferenceException){
            ThisTask.Fail();            
        }
        
    }

    // 낮이예요?
    [Task]
    void IsItDayTime(){
        ThisTask.Complete(GameManager.Instance.timeManager.IsItDayTime());
    }

    // 회사 멀쩡한거 맞죠?
    void CheckWorkplace(){
        if(workplace == null || workplace.buildingData.id != personData.workplaceID){
            workplace = GameManager.Instance.buildingManager.FindBuildingObjectWithID(this.personData.workplaceID);
        }
    }

    // 주워올만한 아이템이 있는지 탐색
    [Task]
    void SearchItemToPickUp(){
        SearchItemToPickUp(null);
    }

    // 
    [Task]
    void SearchItemToPickUp(string itemTag){
        sence.filter = delegate(GameObject value){
            if(value == null)
                return false;

            ItemPickup itemPickup = value.GetComponent<ItemPickup>();
            if(itemPickup == null)
                return false;
            if(itemTag == null)
                return true;

            List<string> pickupItemTags = itemPickup.itemData.itemPreset.tags;
            foreach (string tag in pickupItemTags){
                if(itemTag.CompareTo(tag) == 0)
                    return true;
            }
            return false;
        };

        GameObject nearestItem = sence.FindNearest();

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
        if(personData.items.Count >= 6){
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
    void DoIHaveItemMoreThan(int amount){
        ThisTask.Complete( personData.items.Count > amount );
    }

    [Task]
    void DoIHaveStaminaMoreThan(int amount){
        ThisTask.Complete( personData.stamina > amount );
    }

    // 아이템 갖다 넣으러 가기
    [Task]
    void GoPutInItem(){
        animator.SetInteger("ThinkCode",3);
        CheckWorkplace();
        if(workplace==null){
            ThisTask.Fail();
            return;
        }
        this.target = workplace.gameObject;
        aIDestination.target = workplace.gameObject.transform;
        animator.SetBool("HasAGoal",true);
        ThisTask.Succeed();
    }

    // 아이템을 갖다 넣기
    void PutInItem(){
        this.personData.items = this.personData.items.FindAll(item => item.code != 0);

        if(this.personData.items.Count > 0){
            ItemSlotData itemSlotData = this.personData.items[0];
            if(workplace == null){
                return;
            }
            workplace.buildingData.AddItem(itemSlotData);
            UpdateItemView();
        }else{
            LoseMyTarget();
        }
    }

    [Task]
    void ChangeTool(string tool_name){
        Tool.ToolType toolType = Tool.ToolType.NONE;
        switch (tool_name){
            case "Axe" : 
                toolType = Tool.ToolType.AXE;
                break;
            case "Pickaxe" : 
                toolType = Tool.ToolType.PICKAXE;
                break;
            case "Knife" : 
                toolType = Tool.ToolType.KNIFE;
                break;
            
            default:
                break;
        }
        hitCollision.tool = toolType;
        ThisTask.Succeed();
    }

    // 아이템을 채집하기 위해 찾아보아요
    [Task]
    void SearchingToGathering(string tag){
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
                return preset.attributes.Contains(tag);
            };
        GameObject nearestTree = sence.FindNearest();
        if(nearestTree != null){
            // think = "Going Chop Chop " + nearestTree;
            animator.SetInteger("ThinkCode",2);
            // hitBoxCollision.tool = Tool.ToolType.AXE;
            this.target = nearestTree;
            aIDestination.target = this.target.transform;
            this.target.GetComponent<Hittable>().DeadEventHandler += LoseMyTarget;
            animator.SetBool("HasAGoal",true);
            ThisTask.Succeed();
        }else{
            ThisTask.Fail();
        }
    }

    // 필요한 아이템 찾기
    [Task]
    void SearchingToTakeOutByTag(string itemTag){
        think = "I'm looking for" + itemTag + "(tag)";
        animator.SetInteger("ThinkCode",5);
        List<BuildingObject> buildingList = GameManager.Instance.buildingManager.wholeBuildingList();
        buildingList = buildingList.FindAll(
            delegate(BuildingObject value){
                // if(buildingTag != null){
                //     List<string> attributes = value.buildingData.buildingPreset.attributes;
                //     if(!attributes.Contains(buildingTag)){
                //         return false;
                //     }
                // }
                ItemSlotData[] buildingItems = value.buildingData.items;
                foreach (ItemSlotData itemSlot in buildingItems){
                    ItemPreset itemPreset =  ItemManager.GetItemPresetFromCode(itemSlot.code);
                    if(itemPreset.tags.Contains(itemTag)){
                        return true;
                    }
                }
                return false;
            }
        );
        if(buildingList.Count < 1){
            ThisTask.Fail();
            return;
        }
        this.target = buildingList[0].gameObject;
        aIDestination.target = this.target.transform;
        animator.SetBool("HasAGoal",true);
        ThisTask.Succeed();
    }

    // 아이템 꺼내기
    [Task]
    void TakeOutByTag(string itemTag){
        BuildingObject buildingObject;
        try{
            buildingObject = this.target.GetComponent<BuildingObject>();
        }
        catch (System.NullReferenceException){
            ThisTask.Fail();
            return;
        }
        ItemSlotData[] buildingItems = buildingObject.buildingData.items;
        ItemSlotData taken = null;
        foreach (ItemSlotData itemSlot in buildingItems){
            ItemPreset itemPreset =  ItemManager.GetItemPresetFromCode(itemSlot.code);
            if(itemPreset.tags.Contains(itemTag)){
                taken = itemSlot;
                break;
            }
        }
        if(taken == null){
            ThisTask.Fail();
            return;
        }

        this.personData.items.Add(ItemSlotData.Create(taken.itemPreset));
        taken.amount--;
        if(taken.amount <= 0){
            taken.code = 0;
        }
        UpdateItemView();
        ThisTask.Succeed();
    }
    [Task]
    void ConsumeItemByTag(string itemTag){
        this.personData.items = this.personData.items.FindAll(item =>
            (item.code != 0) && (item.itemPreset.tags.Contains(itemTag))
        );

        if(this.personData.items.Count > 0){
            ItemPreset itemPreset = ItemManager.GetItemPresetFromCode(this.personData.items[0].code);
            switch (itemTag){
                case "Food":
                    this.personData.stamina += itemPreset.efficiency;
                    break;
                case "Gift":
                    this.personData.happiness += itemPreset.efficiency;
                    break;
                default:
                    break;
            }
            this.personData.items[0].amount -= 1;
            if(this.personData.items[0].amount <= 0){
                this.personData.items[0].code = 0;
            }
            UpdateItemView();
            ThisTask.Succeed();
        }else{
            ThisTask.Fail();
        }
    }

    // 집으로 갑시다
    [Task]
    void GoToHome(){
        think = "let i go home";
        animator.SetInteger("ThinkCode",4);
        BuildingObject home = GameManager.Instance.buildingManager.FindBuildingObjectWithID(this.personData.homeID);
        if(home == null){
            this.personData.homeID = 0;
            ThisTask.Fail();
            return;
        }
        this.target = home.gameObject;
        aIDestination.target = this.target.transform;
        animator.SetBool("HasAGoal",true);
        ThisTask.Succeed();
        return;
    }

    [Task]
    void SleepAct(){
        // if(this.personData.sleep == true){
        //     return;
        // }
        // LoseMyTarget();
        this.personData.sleep = true;
        animator.SetBool("Sleep",true);
        think = "Go to sleep! Good Night!";
        if(sleepEvent == null || !sleepEvent.isThisValid()){
            string ticketName = "person"+this.personData.id+"_sleep";
            sleepEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1, ticketName, true, SleepAndRecharging);
        }
        ThisTask.Succeed();
    }
    [Task]
    void AwakeAct(){
        if(this.personData.sleep == false){
            ThisTask.Fail();
            return;
        }
        this.personData.sleep = false;
        animator.SetBool("Sleep",false);
        ThisTask.Succeed();
    }

    [Task]
    void AmIHappyToWork(){
        int workplaceID = this.personData.workplaceID;
        BuildingObject buildingObject = GameManager.Instance.buildingManager.FindBuildingObjectWithID(workplaceID);
        if(buildingObject == null){
            // 백수는 직장이 없으니 직장에 만족하지 않은걸로 취급
            ThisTask.Fail();
            return;
        }
        int workTier = buildingObject.buildingData.buildingPreset.workTier;
        if(workTier < 1){
            ThisTask.Succeed();
            return;
        }
        int happiness = this.personData.happiness;
        int happinessNeeds = GameManager.Instance.peopleManager._happinessStep[workTier-1];
        ThisTask.Complete( happiness > happinessNeeds );
    }

    void LoseMyTarget() {
        target = null;
        aIDestination.target = this.transform;
        animator.SetBool("HasAGoal",false);
        // animator.SetInteger("ThinkCode",0);
    }

    void LoseMyTarget(Hittable component) {
        LoseMyTarget();
    }

    public void UpdateItemView(){
        this.personData.items = this.personData.items.FindAll(item => item.code != 0);

        int length = this.personData.items.Count;
        // Debug.Log("length? : "+length);
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
        // PeopleManager peopleManager = GameManager.Instance.peopleManager;
        // if(this.personData.stamina < 1000){
        //     this.personData.stamina += 5;
        // }

        // do nothing
        
    }

    public void Tired(int amount){
        if(this.personData.stamina > 0){
            this.personData.stamina -= amount;
        }
    }
}
