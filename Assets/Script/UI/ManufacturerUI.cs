using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ManufacturerUI : CommonTaskUI{
    [SerializeField] ManufacturerTaskUI[] manufacturerTaskUIArray;
    [SerializeField] BuildingObject buildingObj;
    [SerializeField] SuperintendentData superintendentData;
    [SerializeField] Slider[] processBars;
    WorkPlace workPlace;

    public override void UpdateUI(){
        base.UpdateUI();
        buildingObj = GameManager.Instance.interactingBuilding;
        superintendentData = buildingObj.buildingData.mediocrityData as SuperintendentData;
        workPlace = buildingObj.GetComponent<WorkPlace>();
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < workPlace.taskInfos.Count; i++){
            TaskInfo taskPreset = workPlace.taskInfos[i];
            ManufacturerTaskUI taskUI = manufacturerTaskUIArray[i];
            taskUI.taskTitle.text = "- "+taskPreset.name+" -";
            taskUI.guideIamge.sprite = taskPreset.guideSprite;
            // taskUI.toggle.isOn = superintendentData.workList[i];
            List<NecessaryResource> resources = taskPreset.necessaryResources;
            taskUI.resourceView1.UpdateResource((resources.Count > 0) ? resources[0] : null);
            taskUI.resourceView2.UpdateResource((resources.Count > 1) ? resources[1] : null);
            taskUI.resourceView3.UpdateResource((resources.Count > 2) ? resources[2] : null);
        }
    }

    private void Update() {
        if(GameManager.Instance.presentGameTab != GameManager.GameTab.MANUFACTURER){
            return;
        }
        if(buildingObj == null){
            return;
        }
        ManufacturerData manufacturerData = buildingObj.buildingData.mediocrityData as ManufacturerData;
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < 3; i++){
            try{
                TaskInfo taskInfo = workPlace.taskInfos[i];
                int requiredTime = taskInfo.requiredTime;
                int dueDate = manufacturerData.dueDate[i];
                int presentTime = GameManager.Instance.timeManager._timeValue;
                int remainingTime = dueDate-presentTime;
                float remainingPercent = (float)remainingTime/(float)requiredTime;
                processBars[i].value = 1.0f-remainingPercent;
            }
            catch (System.Exception){
                // do nothing
                // throw;
            }
            
        }
    }

    public void Manufacture(int index){
        string ticketName = "building_"+buildingObj.buildingData.id+"_make_"+index;

        TaskInfo taskInfo = workPlace.taskInfos[index];
        TimeEventQueueTicket ticket = GameManager.Instance.timeManager.AddTimeEventQueueTicket(taskInfo.requiredTime,ticketName, ManufactureComplete);

        ManufacturerData manufacturerData = buildingObj.buildingData.mediocrityData as ManufacturerData;
        manufacturerData.amount[index] += 1;
        manufacturerData.dueDate[index] = ticket._delay;
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

        UpdateUI();
        return true;
    }
}
