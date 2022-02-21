using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SuperintendentUI : CommonTaskUI{
    [SerializeField] SuperintendentTaskUI[] superintendentTaskUIArray;
    [SerializeField] BuildingObject buildingObj;
    [SerializeField] SuperintendentData superintendentData;

    public override void UpdateUI(){
        base.UpdateUI();
        buildingObj = GameManager.Instance.interactingBuilding;
        superintendentData = buildingObj.buildingData.mediocrityData as SuperintendentData;
        WorkPlace workPlace = buildingObj.GetComponent<WorkPlace>();
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < workPlace.taskInfos.Count; i++){
            TaskInfo taskInfo = workPlace.taskInfos[i];
            SuperintendentTaskUI taskUI = superintendentTaskUIArray[i];
            taskUI.taskTitle.text = "- "+taskInfo.name+" -";
            taskUI.guideIamge.sprite = taskInfo.guideSprite;
            taskUI.toggle.isOn = superintendentData.workList[i];
            List<NecessaryResource> resources = taskInfo.necessaryResources;
            // 이런 더러운 코드 쓰는건 별로 안좋아하지만.....
            taskUI.resourceView1.UpdateResource((resources.Count > 0) ? resources[0] : null);
            taskUI.resourceView2.UpdateResource((resources.Count > 1) ? resources[1] : null);
            taskUI.resourceView3.UpdateResource((resources.Count > 2) ? resources[2] : null);
        }
    }
}
