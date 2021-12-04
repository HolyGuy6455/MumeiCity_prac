using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public Transform buildingParents;
    public List<BuildingPreset> buildingDataList;
    BuildingSlot[] slots;

    // Start is called before the first frame update
    void Start(){
        GameManager.Instance.buildingManager.onToolChangedCallback += UpdateUI;
        slots = buildingParents.GetComponentsInChildren<BuildingSlot>();
    }

    void UpdateUI(){
        Tool nowUsing = GameManager.Instance.GetToolNowHold();
        buildingDataList = GameManager.Instance.buildingManager.buildingDictionary[nowUsing];

        for (int i = 0; i < slots.Length; i++)
        {
            if(i < buildingDataList.Count){
                slots[i].AddItem(buildingDataList[i]);
            }else{
                slots[i].ClearSlot();
            }
        }
    }
}
