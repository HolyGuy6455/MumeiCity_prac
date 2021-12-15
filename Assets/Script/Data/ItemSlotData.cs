using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemSlotData{
    public byte code;
    public int amount;
    public GameManager.UpdateUI _updateUI;

    public ItemPreset itemPreset{
        get{
            return ItemManager.GetItemPresetFromCode(code);
            }
        }

    public static ItemSlotData Create(ItemPreset preset){
        ItemSlotData result = new ItemSlotData();
        result.code = ItemManager.GetCodeFromItemPreset(preset);
        result.amount = 1;
        return result;
    }

    public void UpdateUI(){
        if(_updateUI != null){
            _updateUI.Invoke();
        }
    }
}
