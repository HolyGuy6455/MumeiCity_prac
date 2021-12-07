using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour{
    [TextArea] public string savefile;
    class SaveForm {
        public List<BuildingData> buildingDatas = new List<BuildingData>();
    }


    public void SaveTheGame(){
        SaveForm saveForm = new SaveForm();

        GameObject buildingsParent = GameManager.Instance.buildingManager.buildingsParent;
        foreach (Transform childTransform in buildingsParent.transform){
            BuildingData buildingData = childTransform.GetComponent<BuildingObject>().buildingData;
            saveForm.buildingDatas.Add(buildingData);
        }
        savefile = JsonUtility.ToJson(saveForm,true);
    }

    public void LoadTheGame(){
        GameObject buildingsParent = GameManager.Instance.buildingManager.buildingsParent;
        foreach (Transform childTransform in buildingsParent.transform){
            Destroy(childTransform.gameObject);
        }
        SaveForm saveForm = JsonUtility.FromJson<SaveForm>(savefile);
        GameManager.Instance.buildingManager.LoadBuilding(saveForm.buildingDatas);
    }
}
