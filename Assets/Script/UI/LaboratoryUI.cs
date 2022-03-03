using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LaboratoryUI : CommonTaskUI{
    [SerializeField] LaboratoryTaskUI[] laboratoryTaskUIArray;
    [SerializeField] BuildingObject buildingObj;
    [SerializeField] LaboratoryData laboratoryData;
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
            laboratoryTaskUIArray[i].UpdateUI(workPlace.taskInfos[i]);
        }
        for (int i = workPlace.taskInfos.Count; i < 3; i++){
            laboratoryTaskUIArray[i].UpdateUI(null);
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
        laboratoryData = buildingObj.buildingData.mediocrityData as LaboratoryData;
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < 3; i++){
            try{
                TaskInfo taskInfo = workPlace.taskInfos[i];
                int requiredTime = taskInfo.requiredTime;
                int dueDate = laboratoryData.dueDate[i];
                int presentTime = GameManager.Instance.timeManager._timeValue;
                int remainingTime = dueDate-presentTime;
                float remainingPercent = (float)remainingTime/(float)requiredTime;
                if(dueDate <= presentTime){
                    remainingPercent = 0.0f;
                }
                laboratoryTaskUIArray[i].ChangeValue(1.0f-remainingPercent);
            }
            catch (System.Exception){
                // do nothing
                // throw;
            }
            
        }
    }

    public void Research(int index){
        string ticketName = "building_"+buildingObj.buildingData.id+"_make_"+index;
        TimeManager timeManager = GameManager.Instance.timeManager;
        
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
        // 첫번째로 만들고 있는거 완성 예약
        TimeEventQueueTicket ticket = timeManager.AddTimeEventQueueTicket(taskInfo.requiredTime,ticketName, ManufactureComplete);
        laboratoryTaskUIArray[index].ChangeValue(0.01f);

        if(ticket != null){
            laboratoryData.dueDate[index] = ticket._delay + timeManager._timeValue;
        }

        laboratoryTaskUIArray[index].UpdateUI(taskInfo);
    }

    public void Cancle(int index){
        TimeManager timeManager = GameManager.Instance.timeManager;
        string ticketName = "building_"+buildingObj.buildingData.id+"_make_"+index;

        TaskInfo taskInfo = workPlace.taskInfos[index];

        foreach (NecessaryResource necessary in taskInfo.necessaryResources){
            ItemSlotData itemSlotData = ItemSlotData.Create(ItemData.Instant(necessary.itemDataName));
            itemSlotData.amount = necessary.amount;
            GameManager.Instance.inventory.AddItem(itemSlotData);
        }

        timeManager.RemoveTimeEventQueueTicket(ticketName);
        laboratoryData.dueDate[index] = timeManager._timeValue;

        laboratoryTaskUIArray[index].UpdateUI(taskInfo);
    }

    public bool ManufactureComplete(string ticketName){
        string[] stringSplit = ticketName.Split('_');
        BuildingObject buildingObject = GameManager.Instance.buildingManager.FindBuildingObjectWithID(int.Parse(stringSplit[1]));
        BuildingData buildingData = buildingObject.buildingData;
        int index = int.Parse(stringSplit[3]);
        NecessaryResource resultItem = workPlace.taskInfos[index].resultItem;

        ItemSlotData itemSlotData = ItemSlotData.Create(ItemData.Instant(resultItem.itemDataName));
        itemSlotData.amount = resultItem.amount;
        buildingData.AddItem(itemSlotData);

        TaskInfo taskInfo = workPlace.taskInfos[index];
        laboratoryTaskUIArray[index].UpdateUI(taskInfo);

        UpdateUI();
        return true;
    }
}
