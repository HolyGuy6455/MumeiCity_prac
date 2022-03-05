using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUIBundle : MonoBehaviour{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] SuperintendentUI superintendentUI;
    [SerializeField] HouseUI houseUI;
    [SerializeField] ManufacturerUI manufacturerUI;
    [SerializeField] LaboratoryUI laboratoryUI;
    [SerializeField] AchievementUI achievementUI;


    public void LoadTaskUIData(string taskName){
        inventoryUI.LoadItemSlotData();
        switch (taskName){  
            case "Inventory":
                // do nothing!
                break; 
            case "House":
                houseUI.UpdateUI();
                break;
            case "Superintendent":
                superintendentUI.UpdateUI();
                break;
           case "Manufacturer":
                manufacturerUI.UpdateUI();
                break;
            case "Laboratory":
                laboratoryUI.UpdateUI();
                break;
            case "Achievement":
                achievementUI.UpdateUI();
                break;
            default:
                break;
        }
    }

    void SwitchActionMap(string actionMap){
        GameManager.Instance.SwitchActionMap(actionMap);
    }
}
