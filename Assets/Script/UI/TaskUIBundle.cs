using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUIBundle : MonoBehaviour{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] SuperintendentUI superintendentUI;
    [SerializeField] HouseUI houseUI;
    [SerializeField] ManufacturerUI manufacturerUI;

    public void LoadInventoryUIData(){
        inventoryUI.LoadItemSlotData();
    }

    public void LoadTaskUIData(string taskName){
        inventoryUI.LoadItemSlotData();
        switch (taskName){   
            case "House":
                houseUI.UpdateUI();
                break;
            case "Superintendent":
                superintendentUI.UpdateUI();
                break;
           case "Manufacturer":
                manufacturerUI.UpdateUI();
                break;
            default:
                break;
        }
    }

    void SwitchActionMap(string actionMap){
        GameManager.Instance.SwitchActionMap(actionMap);
    }
}
