using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUIBundle : MonoBehaviour{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ForesterUI foresterUI;
    [SerializeField] TentUI tentUI;
    // [SerializeField] MineUI mineUI;
    [SerializeField] ForesterUI mineUI;         // 임시용
    [SerializeField] ForesterUI foodStorageUI; // 임시용

    public void LoadInventoryUIData(){
        inventoryUI.LoadItemSlotData();
    }
    public void LoadForesterUIData(){
        foresterUI.LoadItemSlotData();
    }
    public void LoadMineUIData(){
        mineUI.LoadItemSlotData();
    }
    public void LoadFoodStorageUIData(){
        foodStorageUI.LoadItemSlotData();
    }
    public void LoadTentUIData(){
        tentUI.LoadItemSlotData();
        tentUI.LoadHouseData();
    }
}
