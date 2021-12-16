using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemSlotData{
    public byte code;
    public int amount;

    public ItemPreset itemPreset{
        get{
            return ItemManager.GetItemPresetFromCode(code);
            }
        }

    public static ItemSlotData Create(ItemPreset preset){
        ItemSlotData result = new ItemSlotData();
        result.code = ItemManager.GetCodeFromItemPreset(preset);
        if(result.code == 0){
            result.amount = 0;
        }else{
            result.amount = 1;
        }
        return result;
    }

    public static void Swap(ItemSlotData data1, ItemSlotData data2){
        byte temp_code = data1.code;
        int temp_amount = data1.amount;
        data1.code = data2.code;
        data1.amount = data2.amount;
        data2.code = temp_code;
        data2.amount = temp_amount;
    }
}
