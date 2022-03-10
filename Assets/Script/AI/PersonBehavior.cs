using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using Panda;

public class PersonBehavior : MonoBehaviour, ITiemEventRebindInfo
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
    [SerializeField] SpriteRenderer hatSprite;
    public PersonData personData = new PersonData();
    
    /*
     ThinkCode 각각 무엇을 의미하는지 적어두기
     1 : 아이템을 줍는다
     2 : 나무를 벤다
     3 : 아이템을 저장하러 직장으로 복귀
     4 : 퇴근
     5 : 필요한 아이템을 꺼내러 간다
     6 : 낮잠(...)
     7 : 춤추기

     */

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

            return itemPickup.itemPickupData.itemData.isThisTag(itemTag);
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
        this.personData.items = this.personData.items.FindAll(item => !item.itemData.isNone());

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
        ToolType toolType = ToolType.NONE;
        switch (tool_name){
            case "Axe" : 
                toolType = ToolType.AXE;
                animator.SetFloat("Tool",0.1f);
                break;
            case "Pickaxe" : 
                toolType = ToolType.PICKAXE;
                animator.SetFloat("Tool",0.2f);
                break;
            case "Knife" : 
                toolType = ToolType.KNIFE;
                animator.SetFloat("Tool",0.3f);
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
        if(sence.whatTheySee.Count == 0){
            ThisTask.Fail();
        }
        foreach (GameObject targetObj in sence.whatTheySee){
            BuildingObject buildingObject = targetObj.GetComponent<BuildingObject>();
            Hittable hittable = targetObj.GetComponent<Hittable>();
            EffectiveTool effectiveTool = null;

            if(hittable == null){
                continue;
            }
            foreach (EffectiveTool effective in buildingObject.removalTool){
                if(effective.tool == hitCollision.tool){
                    effectiveTool = effective;
                    break;
                }
            }
            if(effectiveTool == null){
                continue;
            }

            if(hittable.HP < effectiveTool.minHP){
                continue;
            }
            
            // EffectiveTool effectiveTool 
            animator.SetInteger("ThinkCode",2);
            this.target = targetObj;
            aIDestination.target = this.target.transform;
            this.target.GetComponent<Hittable>().DeadEventHandler.AddListener(LoseMyTarget);
            animator.SetBool("HasAGoal",true);
            ThisTask.Succeed();
            return;
        }
        ThisTask.Fail();
        // 미친코드다;;; 반복문의 끝에 return이 있다니;;
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
                    if(itemSlot.itemData.isThisTag(itemTag)){
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
            ItemData itemData = ItemData.Instant(itemSlot.itemName);
            if(itemData.isThisTag(itemTag)){
                taken = itemSlot;
                break;
            }
        }
        if(taken == null){
            ThisTask.Fail();
            return;
        }

        this.personData.items.Add(ItemSlotData.Create(taken.itemData));
        taken.amount--;
        if(taken.amount <= 0){
            taken.itemName = "None";
        }
        UpdateItemView();
        ThisTask.Succeed();
    }
    [Task]
    void ConsumeItemByTag(string itemTag){
        Debug.Log("ConsumeItemByTag " + itemTag);
        this.personData.items = this.personData.items.FindAll(item =>
            (!item.itemData.isNone()) && (item.itemData.isThisTag(itemTag))
        );

        if(this.personData.items.Count > 0){
            ItemData itemData = ItemData.Instant(this.personData.items[0].itemName);
            switch (itemTag){
                case "Food":
                    this.personData.stamina += itemData.efficiency;
                    break;
                case "Gift":
                    this.personData.happiness += itemData.efficiency;
                    break;
                default:
                    break;
            }
            this.personData.items[0].amount -= 1;
            if(this.personData.items[0].amount <= 0){
                this.personData.items[0].itemName = "None";
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
            string ticketName = "person_"+this.personData.id+"_sleep";
            sleepEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1, ticketName, SleepAndRecharging);
        }
        ThisTask.Succeed();
    }
    [Task]
    void AwakeAct(){
        if(this.personData.sleep == false){
            ThisTask.Fail();
            return;
        }
        BuildingObject house =  GameManager.Instance.buildingManager.FindBuildingObjectWithID(personData.homeID);
        if(house != null){
            this.transform.position = house.transform.position + new Vector3(0,0,-2);
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
        int workTier = PeopleManager.Instance.GetJobInfo(personData.jobID).workTier;
        if(workTier < 1){
            ThisTask.Succeed();
            return;
        } 
        int happiness = this.personData.happiness;
        int happinessNeeds = GameManager.Instance.peopleManager._happinessStep[workTier-1];
        ThisTask.Complete( happiness > happinessNeeds );
    }

    [Task]
    void AmIAdult(){
        animator.SetBool("AmIAdult",(personData.growth >= 1.0f));
        ThisTask.Complete(personData.growth >= 1.0f);
    }
    [Task]
    void NapAct(){
        animator.SetInteger("ThinkCode",6);
    }
    [Task]
    void DanceAct(){
        animator.SetInteger("ThinkCode",7);
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
        this.personData.items = this.personData.items.FindAll(item => !item.itemData.isNone());

        int length = this.personData.items.Count;
        // Debug.Log("length? : "+length);
        for (int i = 0; i < length; i++){
            pocketItemSlots[i].SetActive(true);
            pocketItemSlots[i].transform.localPosition = new Vector3(((float)-length+1)/4 + ((float)i)/2,0.5f,0);
            pocketItemSlots[i].GetComponent<SpriteRenderer>().sprite = this.personData.items[i].itemData.itemSprite;
        }
        for (int i = length; i < pocketItemSlots.Count; i++){
            pocketItemSlots[i].SetActive(false);
        }
    }

    public bool SleepAndRecharging(string ticketName){
        // PeopleManager peopleManager = GameManager.Instance.peopleManager;
        // if(this.personData.stamina < 1000){
        //     this.personData.stamina += 5;
        // }
        if(personData.homeID == 0){
            List<BuildingObject> houseList = GameManager.Instance.buildingManager.wholeBuildingList();
            houseList = houseList.FindAll(buildingObject => buildingObject.buildingData.mediocrityData is HouseData);

            foreach (BuildingObject house in houseList){
                HouseData houseData = house.buildingData.mediocrityData as HouseData;
                for (int i = 0; i < houseData.personIDList.Length; i++){
                    if(houseData.personIDList[i] == 0){
                        // Debug.Log("집을 찾았어요 : " + house.buildingData.id);
                        houseData.personIDList[i] = personData.id;
                        personData.homeID = house.buildingData.id;
                        break;
                    }
                }
            }
        }else{
            if(personData.growth < 1.0f){
                personData.growth += 0.0005f;
            }
        }

        return false;
    }

    public void Tired(int amount){
        if(this.personData.stamina > 0){
            this.personData.stamina -= amount;
        }
    }

    public void UpdateHatImage(){
        this.hatSprite.sprite = PeopleManager.Instance.GetJobInfo(personData.jobID).hatImage;
    }

    public Dictionary<string, TimeManager.TimeEvent> GetDictionary(){
        Dictionary<string, TimeManager.TimeEvent> result = new Dictionary<string, TimeManager.TimeEvent>();

        string ticketName = "person_"+this.personData.id+"_sleep";
        result[ticketName] = SleepAndRecharging;

        return result;
    }
}
