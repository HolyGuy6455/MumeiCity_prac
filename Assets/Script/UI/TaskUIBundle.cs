using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUIBundle : MonoBehaviour{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ForesterHutUI foresterUI;
    [SerializeField] TentUI tentUI;
    // [SerializeField] MineUI mineUI;
    [SerializeField] ForesterHutUI mineUI;         // 임시용
    [SerializeField] ForesterHutUI foodStorageUI; // 임시용

    public void LoadInventoryUIData(){
        inventoryUI.LoadItemSlotData();
    }
    public void LoadForesterUIData(){
        inventoryUI.LoadItemSlotData();
        foresterUI.UpdateUI();
    }
    public void LoadMineUIData(){
        inventoryUI.LoadItemSlotData();
        mineUI.UpdateUI();
    }
    public void LoadFoodStorageUIData(){
        inventoryUI.LoadItemSlotData();
        foodStorageUI.UpdateUI();
    }
    public void LoadTentUIData(){
        inventoryUI.LoadItemSlotData();
        tentUI.UpdateUI();
    }
}
