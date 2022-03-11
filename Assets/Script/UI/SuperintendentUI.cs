using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SuperintendentUI : CommonTaskUI{
    [SerializeField] SuperintendentTaskUI[] superintendentTaskUIArray;
    [SerializeField] BuildingObject buildingObj;
    [SerializeField] SuperintendentFunction superintendentData;
    [SerializeField] Text titleText;

    public override void UpdateUI(){
        base.UpdateUI();
        buildingObj = GameManager.Instance.interactingBuilding;
        superintendentData = buildingObj.buildingData.facilityFunction as SuperintendentFunction;
        Debug.Log("workList 2 " + superintendentData.workList);
        superintendentData.ReloadMediocrityData(buildingObj.buildingData);
        WorkPlace workPlace = buildingObj.GetComponent<WorkPlace>();
        if(workPlace == null){
            return;
        }
        for (int i = 0; i < workPlace.taskInfos.Count; i++){
            superintendentTaskUIArray[i].ChangeValue(superintendentData.workList[i]);
            superintendentTaskUIArray[i].UpdateUI(workPlace.taskInfos[i]);
        }
        for (int i = workPlace.taskInfos.Count; i < 3; i++){
            superintendentTaskUIArray[i].UpdateUI(null);
        }
        titleText.text = buildingObj.buildingData.buildingPreset.name;
    }

    public void Permit(int index){
        superintendentData.workList[index] = superintendentTaskUIArray[index].toggle.isOn;
    }
}
