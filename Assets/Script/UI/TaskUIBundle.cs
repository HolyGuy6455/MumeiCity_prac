using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUIBundle : MonoBehaviour{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ForesterUI foresterUI;
    public void LoadInventoryUIData(){
        inventoryUI.LoadItemSlotData();
    }
    public void LoadForesterUIData(){
        foresterUI.LoadItemSlotData();
    }
}
