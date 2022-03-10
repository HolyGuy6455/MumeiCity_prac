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
            laboratoryTaskUIArray[i].taskInfo = workPlace.taskInfos[i];
            laboratoryTaskUIArray[i].UpdateUI();
        }
        for (int i = workPlace.taskInfos.Count; i < 3; i++){
            laboratoryTaskUIArray[i].taskInfo = null;
            laboratoryTaskUIArray[i].UpdateUI();
        }
        titleText.text = buildingObj.buildingData.buildingPreset.name;
    }

    private void Update() {
        if(GameManager.Instance.presentGameTab != GameManager.GameTab.LABORATORY){
            return;
        }
        if(buildingObj == null){
            return;
        }
        laboratoryData = buildingObj.buildingData.mediocrityData as LaboratoryData;
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < workPlace.taskInfos.Count; i++){
            TaskInfo taskInfo = workPlace.taskInfos[i];
            int requiredTime = taskInfo.requiredTime;
            int dueDate = laboratoryData.dueDate[i];
            int presentTime = GameManager.Instance.timeManager._timeValue;
            int remainingTime = dueDate-presentTime;
            float remainingPercent = (float)remainingTime/(float)requiredTime;
            if(dueDate <= presentTime){
                remainingPercent = 0.0f;
            }else if(remainingTime > requiredTime){
                remainingPercent = 1.0f;
            }
            laboratoryTaskUIArray[i].ChangeValue(1.0f-remainingPercent);
        }
    }

    public void Research(int index){
        string ticketName = "building_"+buildingObj.buildingData.id+"_research_"+index;
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
        TimeEventQueueTicket ticket = timeManager.AddTimeEventQueueTicket(taskInfo.requiredTime,ticketName, ResearchComplete);
        laboratoryTaskUIArray[index].ChangeValue(0.01f);

        if(ticket != null){
            laboratoryData.dueDate[index] = ticket._delay + timeManager._timeValue;
        }

        laboratoryTaskUIArray[index].taskInfo = taskInfo;
        laboratoryTaskUIArray[index].UpdateUI();
    }

    public void Cancle(int index){
        TimeManager timeManager = GameManager.Instance.timeManager;
        string ticketName = "building_"+buildingObj.buildingData.id+"_research_"+index;

        TaskInfo taskInfo = workPlace.taskInfos[index];

        foreach (NecessaryResource necessary in taskInfo.necessaryResources){
            ItemSlotData itemSlotData = ItemSlotData.Create(ItemData.Instant(necessary.itemDataName));
            itemSlotData.amount = necessary.amount;
            GameManager.Instance.inventory.AddItem(itemSlotData);
        }

        timeManager.RemoveTimeEventQueueTicket(ticketName);
        laboratoryData.dueDate[index] = int.MaxValue;

        laboratoryTaskUIArray[index].taskInfo = taskInfo;
        laboratoryTaskUIArray[index].ChangeValue(0.0f);
        laboratoryTaskUIArray[index].UpdateUI();
    }

    public bool ResearchComplete(string ticketName){
        string[] stringSplit = ticketName.Split('_');
        BuildingObject buildingObject = GameManager.Instance.buildingManager.FindBuildingObjectWithID(int.Parse(stringSplit[1]));
        int index = int.Parse(stringSplit[3]);
        WorkPlace workPlaceNow = buildingObject.GetComponent<WorkPlace>();
        TaskInfo taskInfo = workPlaceNow.taskInfos[index];

        GameManager.Instance.achievementManager.AddTrial(taskInfo.resultUpgrade,1);

        laboratoryTaskUIArray[index].taskInfo = taskInfo;
        laboratoryTaskUIArray[index].ChangeValue(1.0f);
        laboratoryTaskUIArray[index].UpdateUI();

        UpdateUI();
        return true;
    }

}
