using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemData
{
    public byte code;
    public int amount;
    public float positionX;
    public float positionY;
    public ItemPreset itemPreset{
        get{
            return GameManager.Instance.inventoryManager.GetItemPresetFromCode(code);
            }
        }

    public void Use(){
        itemPreset.Use(this);
    }

    public static ItemData create(ItemPreset preset){
        ItemData result = new ItemData();
        result.code = GameManager.Instance.inventoryManager.GetCodeFromItemPreset(preset);
        result.amount = 1;
        return result;
    }
}
