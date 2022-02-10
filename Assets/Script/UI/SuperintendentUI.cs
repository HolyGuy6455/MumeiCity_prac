using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SuperintendentUI : CommonTaskUI{
    [SerializeField] SuperintendentTaskUI[] superintendentTaskUIArray;
    [SerializeField] BuildingData buildingData;
    [SerializeField] SuperintendentData superintendentData;

    public override void UpdateUI(){
        base.UpdateUI();
        buildingData = GameManager.Instance.interactingBuilding;
        superintendentData = buildingData.mediocrityData as SuperintendentData;
        List<TaskPreset> taskPresets = buildingData.buildingPreset.taskPresets;
        for (int i = 0; i < taskPresets.Count; i++){
            TaskPreset taskPreset = taskPresets[i];
            SuperintendentTaskUI taskUI = superintendentTaskUIArray[i];
            taskUI.taskTitle.text = "- "+taskPreset.name+" -";
            taskUI.guideIamge.sprite = taskPreset.guideSprite;
            taskUI.toggle.isOn = superintendentData.workList[i];
            List<NecessaryResource> resources = taskPreset.necessaryResources;
            // 이런 더러운 코드 쓰는건 별로 안좋아하지만.....
            taskUI.resourceView1.UpdateResource((resources.Count > 0) ? resources[0] : null);
            taskUI.resourceView2.UpdateResource((resources.Count > 1) ? resources[1] : null);
            taskUI.resourceView3.UpdateResource((resources.Count > 2) ? resources[2] : null);
        }
    }
}
