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
            return GameManager.Instance.itemManager.GetItemPresetFromCode(code);
            }
        }

    public static ItemSlotData Create(ItemPreset preset){
        ItemSlotData result = new ItemSlotData();
        result.code = GameManager.Instance.itemManager.GetCodeFromItemPreset(preset);
        result.amount = 1;
        return result;
    }

    // 편의상 넣은것
    public ItemSlotData CopyEmpty(){
        ItemSlotData result = new ItemSlotData();
        result.code = this.code;
        result.amount = 0;
        return result;
    }
}
