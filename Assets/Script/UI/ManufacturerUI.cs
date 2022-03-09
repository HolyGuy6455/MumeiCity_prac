using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ManufacturerUI : CommonTaskUI{
    [SerializeField] ManufacturerTaskUI[] manufacturerTaskUIArray;
    [SerializeField] BuildingObject buildingObj;
    [SerializeField] ManufacturerData manufacturerData;
    [SerializeField] Text titleText;

    WorkPlace workPlace;

    public override void UpdateUI(){
        base.UpdateUI();
        buildingObj = GameManager.Instance.interactingBuilding;
        workPlace = buildingObj.GetComponent<WorkPlace>();
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < workPlace.taskInfos.Count; i++){
            manufacturerTaskUIArray[i].UpdateUI(workPlace.taskInfos[i]);
        }
        for (int i = workPlace.taskInfos.Count; i < 3; i++){
            manufacturerTaskUIArray[i].UpdateUI(null);
        }
        titleText.text = buildingObj.buildingData.buildingPreset.name;
    }

    private void Update() {
        if(GameManager.Instance.presentGameTab != GameManager.GameTab.MANUFACTURER){
            return;
        }
        if(buildingObj == null){
            return;
        }
        manufacturerData = buildingObj.buildingData.mediocrityData as ManufacturerData;
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < workPlace.taskInfos.Count; i++){
            try{
                TaskInfo taskInfo = workPlace.taskInfos[i];
                int requiredTime = taskInfo.requiredTime;
                int dueDate = manufacturerData.dueDate[i];
                int presentTime = GameManager.Instance.timeManager._timeValue;
                int remainingTime = dueDate-presentTime;
                float remainingPercent = (float)remainingTime/(float)requiredTime;
                if(dueDate <= presentTime){
                    remainingPercent = 1.0f;
                }
                manufacturerTaskUIArray[i].ChangeValue(1.0f-remainingPercent);
                manufacturerTaskUIArray[i].UpdateUI_Counter(manufacturerData.amount[i]);
            }
            catch (System.Exception){
                // do nothing
                // throw;
            }
            
        }
    }

    public void Manufacture(int index){
        if(manufacturerData.amount[index] >= 5){
            // 주문 5개 이상은 안받습니다
            return;
        }
        TaskInfo taskInfo = workPlace.taskInfos[index];
        // 재료가 충분히 있나요
        Inventory inventory = GameManager.Instance.inventory;
        foreach (NecessaryResource necessary in taskInfo.necessaryResources){
            int havingAmountValue = inventory.GetItemAmount(necessary.itemDataName);
            if(havingAmountValue < necessary.amount){
                Debug.Log("Not Enough Item!");
                return;
            }
        }
        foreach (NecessaryResource necessary in taskInfo.necessaryResources){
            inventory.ConsumeItem(necessary.itemDataName,necessary.amount);
        }
        TakeReservation(index);
        manufacturerData.amount[index] += 1;
        manufacturerTaskUIArray[index].UpdateUI(taskInfo);
    }

    private void TakeReservation(int index){
        TimeManager timeManager = GameManager.Instance.timeManager;
        TaskInfo taskInfo = workPlace.taskInfos[index];

        string ticketName = "building_"+buildingObj.buildingData.id+"_make_"+index;
        TimeEventQueueTicket ticket = timeManager.AddTimeEventQueueTicket(taskInfo.requiredTime,ticketName, ManufactureComplete);
        manufacturerTaskUIArray[index].ChangeValue(0.01f);
        
        if(ticket != null){
            manufacturerData.dueDate[index] = ticket._delay + timeManager._timeValue;
        }
    }

    public void Cancle(int index){
        TimeManager timeManager = GameManager.Instance.timeManager;
        string ticketName = "building_"+buildingObj.buildingData.id+"_make_"+index;
        TaskInfo taskInfo = workPlace.taskInfos[index];

        if(manufacturerData.amount[index] <= 0){
            // you can't do this!
            return;
        }else if(manufacturerData.amount[index] == 1){
            timeManager.RemoveTimeEventQueueTicket(ticketName);
            manufacturerData.dueDate[index] = timeManager._timeValue;
            manufacturerData.amount[index] = 0;
        }else{
            manufacturerData.amount[index] -= 1;
        }

        foreach (NecessaryResource necessary in taskInfo.necessaryResources){
            ItemSlotData itemSlotData = ItemSlotData.Create(ItemData.Instant(necessary.itemDataName));
            itemSlotData.amount = necessary.amount;
            GameManager.Instance.inventory.AddItem(itemSlotData);
        }

        manufacturerTaskUIArray[index].UpdateUI(taskInfo);
    }

    public bool ManufactureComplete(string ticketName){
        string[] stringSplit = ticketName.Split('_');
        BuildingObject buildingObject = GameManager.Instance.buildingManager.FindBuildingObjectWithID(int.Parse(stringSplit[1]));
        BuildingData buildingData = buildingObject.buildingData;
        int index = int.Parse(stringSplit[3]);
        WorkPlace workPlaceNow = buildingObject.GetComponent<WorkPlace>();
        NecessaryResource resultItem = workPlaceNow.taskInfos[index].resultItem;

        ItemSlotData itemSlotData = ItemSlotData.Create(ItemData.Instant(resultItem.itemDataName));
        itemSlotData.amount = resultItem.amount;
        buildingData.AddItem(itemSlotData);

        // 만약 예약이 1개 이상 남아있다면, 다음 작업 이어서 하기
        manufacturerData.amount[index] -= 1;
        Debug.Log("Remain : " + manufacturerData.amount[index]);
        if(manufacturerData.amount[index] > 0){
            TakeReservation(index);
        }

        TaskInfo taskInfo = workPlaceNow.taskInfos[index];
        manufacturerTaskUIArray[index].UpdateUI(taskInfo);

        UpdateUI();
        return true;
    }
}
