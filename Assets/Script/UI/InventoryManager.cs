using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
    
    public GameManager.UpdateUI onItemChangedCallback;
    public List<ItemData> items = new List<ItemData>();
    public int itemSpace = 20;
    public GameObject inventoryUI;
    [SerializeField] private List<ItemPreset> itemPresetList = new List<ItemPreset>();
    

    public ItemPreset GetItemPresetFromCode(byte code){
        int index = (int)code;
        return itemPresetList[index];
    }

    public byte GetCodeFromItemPreset(ItemPreset preset){
        for (int i = 0; i < itemPresetList.Count; i++){
            if(itemPresetList[i] == preset){
                return (byte)i;
            }
        }
        return (byte)(0);
    }
    public byte GetCodeFromItemName(string name){
        for (int i = 0; i < itemPresetList.Count; i++){
            if(itemPresetList[i].itemName == name){
                return (byte)i;
            }
        }
        return (byte)(0);
    }

    public int GetItemAmount(string itemName){
        int result = 0;
        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].code == GetCodeFromItemName(itemName)){
                result += items[i].amount;
            }
        }
        return result;
    }

    public bool ConsumeItem(string itemName, int amount){
        if(GetItemAmount(itemName) < amount)
            return false;

        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].code == GetCodeFromItemName(itemName)){
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
    
    public bool AddItem(ItemData item){
        // if(item.isDefaultItem){
        //     return false;
        // }
        if(items.Count >= itemSpace){
            Debug.Log("not enough slot");
            return false;
        }

        ItemData added = null;
        for (int i = 0; i < items.Count; i++){
            if(items[i] != null && items[i].code == item.code){
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

    public void RemoveItem(ItemData item){
        ItemData removed = null;
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].code == item.code){
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