using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemSlotData{
    public string itemName;
    public int amount;

    public ItemData itemData{
        get{
            return ItemData.Instant(itemName);
            }
        }

    public static ItemSlotData Create(ItemData data){
        ItemSlotData result = new ItemSlotData();
        result.itemName = data.itemName;
        result.amount = (data.isNone())? 0 : 1;
        return result;
    }

    public static void Swap(ItemSlotData data1, ItemSlotData data2){
        string temp_name = data1.itemName;
        int temp_amount = data1.amount;
        data1.itemName = data2.itemName;
        data1.amount = data2.amount;
        data2.itemName = temp_name;
        data2.amount = temp_amount;
    }
}
