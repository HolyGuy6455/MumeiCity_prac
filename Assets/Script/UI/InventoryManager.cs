using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
    
    public GameManager.UpdateUI onItemChangedCallback;
    public List<ItemPreset> items = new List<ItemPreset>();
    public int itemSpace = 20;
    public GameObject inventoryUI;

    public int GetItemAmount(string itemName){
        int result = 0;
        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].itemName == itemName){
                result += items[i].amount;
            }
        }
        return result;
    }

    public bool ConsumeItem(string itemName, int amount){
        if(GetItemAmount(itemName) < amount)
            return false;

        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].itemName == itemName){
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
    
    public bool AddItem(ItemPreset item){
        if(item.isDefaultItem){
            return false;
        }
        if(items.Count >= itemSpace){
            Debug.Log("not enough slot");
            return false;
        }

        ItemPreset added = null;
        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].itemName == item.itemName){
                added = items[i];
                added.amount++;
                break;
            }
        }
        if(added == null){
            item.amount = 1;
            items.Add(item);
        }

        if(onItemChangedCallback != null){
            onItemChangedCallback.Invoke();
        }
        return true;
    }

    public void RemoveItem(ItemPreset item){
        ItemPreset removed = null;
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].itemName == item.itemName){
                items[i].amount--;
                if(items[i].amount == 0){
                    removed = items[i];
                    break;
                }
            }
        }
        if(removed != null)
            items.Remove(removed);

        
        if(onItemChangedCallback != null){
            onItemChangedCallback.Invoke();
        }
    }
}