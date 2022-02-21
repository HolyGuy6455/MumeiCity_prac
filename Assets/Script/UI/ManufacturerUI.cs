using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ManufacturerUI : CommonTaskUI{
    [SerializeField] ManufacturerTaskUI[] manufacturerTaskUIArray;
    [SerializeField] BuildingData buildingData;
    [SerializeField] SuperintendentData superintendentData;
    [SerializeField] Slider[] processBars;

    public override void UpdateUI(){
        base.UpdateUI();
        buildingData = GameManager.Instance.interactingBuilding;
        superintendentData = buildingData.mediocrityData as SuperintendentData;
        List<TaskPreset> taskPresets = buildingData.buildingPreset.taskPresets;
        for (int i = 0; i < taskPresets.Count; i++){
            TaskPreset taskPreset = taskPresets[i];
            ManufacturerTaskUI taskUI = manufacturerTaskUIArray[i];
            taskUI.taskTitle.text = "- "+taskPreset.name+" -";
            taskUI.guideIamge.sprite = taskPreset.guideSprite;
            // taskUI.toggle.isOn = superintendentData.workList[i];
            List<NecessaryResource> resources = taskPreset.necessaryResources;
            // 이런 더러운 코드 쓰는건 별로 안좋아하지만.....
            taskUI.resourceView1.UpdateResource((resources.Count > 0) ? resources[0] : null);
            taskUI.resourceView2.UpdateResource((resources.Count > 1) ? resources[1] : null);
            taskUI.resourceView3.UpdateResource((resources.Count > 2) ? resources[2] : null);
        }
    }

    private void Update() {
        if(GameManager.Instance.presentGameTab != GameManager.GameTab.MANUFACTURER){
            return;
        }
        if(buildingData == null){
            return;
        }
        ManufacturerData manufacturerData = buildingData.mediocrityData as ManufacturerData;
        for (int i = 0; i < 3; i++){
            try{
                TaskPreset taskPreset = buildingData.buildingPreset.taskPresets[i];
                int requiredTime = taskPreset.requiredTime;
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
        string ticketName = "building_"+buildingData.id+"_make_"+index;
        Debug.Log(ticketName);
        TaskPreset taskPreset = buildingData.buildingPreset.taskPresets[index];
        TimeEventQueueTicket ticket = GameManager.Instance.timeManager.AddTimeEventQueueTicket(taskPreset.requiredTime,ticketName, ManufactureComplete);

        ManufacturerData manufacturerData = buildingData.mediocrityData as ManufacturerData;
        manufacturerData.amount[index] += 1;
        manufacturerData.dueDate[index] = ticket._delay;
    }

    public bool ManufactureComplete(string ticketName){
        string[] stringSplit = ticketName.Split('_');
        BuildingObject buildingObject = GameManager.Instance.buildingManager.FindBuildingObjectWithID(int.Parse(stringSplit[1]));
        BuildingData buildingData = buildingObject.buildingData;
        int index = int.Parse(stringSplit[3]);
        NecessaryResource resultItem = buildingData.buildingPreset.taskPresets[index].resultItem;

        ItemSlotData itemSlotData = ItemSlotData.Create(ItemData.Instant(resultItem.itemDataName));
        itemSlotData.amount = resultItem.amount;
        buildingData.AddItem(itemSlotData);

        UpdateUI();
        return true;
    }
}
