using UnityEngine;
using UnityEngine.UI;
using Unity​Engine.EventSystems;
/*
 * 건설 UI에서 보이는 건물 선택 버튼
 */
public class BuildingSlot : MonoBehaviour
{
    public Image icon;
    public BuildingPreset buildingPreset;
    

    public void AddItem(BuildingPreset buildingPreset){
        this.buildingPreset = buildingPreset;
        icon.sprite = buildingPreset.sprite;
        icon.enabled = true;
    }

    public void ClearSlot(){
        this.buildingPreset = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnRemoveButton(){
    }

    public void OnBuildButton(){
        if(buildingPreset != null){
            GameManager.Instance.buildingManager.SetBuildingPreset(this.buildingPreset);
            // bool isDone = GameManager.Instance.buildingManager.Build(this.buildingData);
            // if(!isDone){
            //     Debug.Log("Construction is Fail......");
            // }
        }else{
            Debug.Log("There is nothing....");
        }
    }
}
