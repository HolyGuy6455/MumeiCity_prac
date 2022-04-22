using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LaboratoryUI : CommonTaskUI{
    [SerializeField] LaboratoryTaskUI[] laboratoryTaskUIArray;
    [SerializeField] BuildingObject buildingObj;
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
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < workPlace.taskInfos.Count; i++){
            TaskInfo taskInfo = workPlace.taskInfos[i];
            float percentage = GameManager.Instance.achievementManager.GetPercentage(taskInfo.resultUpgrade);

            if(percentage < 0){
                percentage = 0.0f;
            }else if(percentage > 1){
                percentage = 1.0f;
            }
            laboratoryTaskUIArray[i].ChangeValue(percentage);
        }
    }

    public void Research(int index){
        TimeManager timeManager = GameManager.Instance.timeManager;
        
        TaskInfo taskInfo = workPlace.taskInfos[index];
        string ticketName = "research_"+index+"_"+taskInfo.resultUpgrade;
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
        TimeEventQueueTicket ticket = timeManager.AddTimeEventQueueTicket(1,ticketName, Researching);
        laboratoryTaskUIArray[index].ChangeValue(0.01f);
        laboratoryTaskUIArray[index].taskInfo = taskInfo;
        laboratoryTaskUIArray[index].UpdateUI();
    }

    public void Cancle(int index){
        TimeManager timeManager = GameManager.Instance.timeManager;

        TaskInfo taskInfo = workPlace.taskInfos[index];
        string ticketName = "research_"+index+"_"+taskInfo.resultUpgrade;

        foreach (NecessaryResource necessary in taskInfo.necessaryResources){
            ItemSlotData itemSlotData = ItemSlotData.Create(ItemData.Instant(necessary.itemDataName));
            itemSlotData.amount = necessary.amount;
            GameManager.Instance.inventory.AddItem(itemSlotData);
        }

        timeManager.RemoveTimeEventQueueTicket(ticketName);

        laboratoryTaskUIArray[index].taskInfo = taskInfo;
        laboratoryTaskUIArray[index].ChangeValue(0.0f);
        laboratoryTaskUIArray[index].UpdateUI();

        GameManager.Instance.achievementManager.SetTrial(taskInfo.resultUpgrade,0);
    }

    public bool Researching(string ticketName){
        string[] stringSplit = ticketName.Split('_');
        GameManager.Instance.achievementManager.AddTrial(stringSplit[2],1);

        UpdateUI();
        return GameManager.Instance.achievementManager.isDone(stringSplit[2]);
    }

}
