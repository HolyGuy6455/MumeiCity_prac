using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    
    // public GameManager.UpdateUI onItemChangedCallback;
    // public List<ItemSlotData> items = new List<ItemSlotData>(); 
    
    // ItemSlotData[] slots;
    // public GameObject inventoryUI;
    [SerializeField] Transform itemsParents;
    public ItemSlotData itemInHand;
    InventorySlot[] slots;
    // public int itemSpace = 20;

    private void Start() {
        // items = new ItemSlotData[itemSpace];
        slots = itemsParents.GetComponentsInChildren<InventorySlot>();
    }
    
    public int GetItemAmount(string itemName){
        int result = 0;
        byte findingCode = GameManager.Instance.itemManager.GetCodeFromItemName(itemName);
        for (int i = 0; i < slots.Length; i++){
            if(slots[i] != null && slots[i].data.code == findingCode){
                result += slots[i].data.amount;
            }
        }
        return result;
    }

    public bool ConsumeItem(string itemName, int amount){
        if(GetItemAmount(itemName) < amount)
            return false;

        byte findingCode = GameManager.Instance.itemManager.GetCodeFromItemName(itemName);

        for (int i = 0; i < slots.Length; i++){
            if(slots[i] != null && slots[i].data.code == findingCode){
                if(slots[i].data.amount > amount){
                    slots[i].data.amount -= amount;
                    amount = 0;
                }else{
                    amount -= slots[i].data.amount;
                    slots[i].data = null;
                }
                
            }
        }
        
        return true;
    }
    
    public bool AddItem(ItemSlotData addedItemData){
        bool result = false;

        for (int i = 0; i < slots.Length; i++){
            if(slots[i].data != null && slots[i].data.code == addedItemData.code){
                ItemSlotData added = slots[i].data;
                added.amount += addedItemData.amount;
                result = true;
                break;
            }
        }

        if(result == false){
            for (int i = 0; i < slots.Length; i++){
                if(slots[i].data == null){
                    slots[i].data = addedItemData;
                    result = true;
                }
            }
        }

        return result;
    }
}