using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUIBundle : MonoBehaviour{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] SuperintendentUI foresterUI;
    [SerializeField] HouseUI houseUI;
    // [SerializeField] MineUI mineUI;
    [SerializeField] SuperintendentUI mineUI;         // 임시용
    [SerializeField] SuperintendentUI foodStorageUI; // 임시용

    public void LoadInventoryUIData(){
        inventoryUI.LoadItemSlotData();
    }
    public void LoadFoodStorageUIData(){
        inventoryUI.LoadItemSlotData();
        foodStorageUI.UpdateUI();
    }

    public void LoadTaskUIData(string taskName){
        inventoryUI.LoadItemSlotData();
        switch (taskName){   

            case "House":
                houseUI.UpdateUI();
                break;

            case "Superintendent":
                foresterUI.UpdateUI();
                break;

            default:
                break;
        }
    }

    void SwitchActionMap(string actionMap){
        GameManager.Instance.SwitchActionMap(actionMap);
    }
}
