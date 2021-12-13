using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    
    public GameManager.UpdateUI onItemChangedCallback;
    public List<ItemSlotData> items = new List<ItemSlotData>();
    public int itemSpace = 20;
    public GameObject inventoryUI;
    
    public int GetItemAmount(string itemName){
        int result = 0;
        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].code == GameManager.Instance.itemManager.GetCodeFromItemName(itemName)){
                result += items[i].amount;
            }
        }
        return result;
    }

    public bool ConsumeItem(string itemName, int amount){
        if(GetItemAmount(itemName) < amount)
            return false;

        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].code == GameManager.Instance.itemManager.GetCodeFromItemName(itemName)){
                if(items[i].amount >= amount){
                    items[i].amount -= amount;
                    amount = 0;
                }else if(items[i].amount == amount){
                    amount = 0;
                    items.Remove(items[i]);
                }else{
                    amount -= items[i].amount;
                    items.Remove(items[i]);
                }
                
            }
        }

        if(onItemChangedCallback != null){
            onItemChangedCallback.Invoke();
        }
        
        return true;
    }
    
    public bool AddItem(ItemSlotData addedItemData){
        if(items.Count >= itemSpace){
            Debug.Log("not enough slot");
            return false;
        }

        ItemSlotData added = null;
        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].code == addedItemData.code){
                added = items[i];
                added.amount += addedItemData.amount;
                break;
            }
        }
        if(added == null){
            items.Add(addedItemData);
        }

        if(onItemChangedCallback != null){
            onItemChangedCallback.Invoke();
        }
        return true;
    }

    // public void RemoveItem(ItemSlotData item){
    //     ItemSlotData removed = null;
    //     for (int i = 0; i < items.Count; i++)
    //     {
    //         if(items[i].code == item.code){
    //             items[i].amount--;
    //             if(items[i].amount == 0){
    //                 removed = items[i];
    //                 break;
    //             }
    //         }
    //     }
    //     if(removed != null)
    //         items.Remove(removed);

        
    //     if(onItemChangedCallback != null){
    //         onItemChangedCallback.Invoke();
    //     }
    // }
}