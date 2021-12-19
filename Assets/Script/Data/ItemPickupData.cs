using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemPickupData : MediocrityData{
    public byte code;
    public Vector3 position;
    public ItemPreset itemPreset{
        get{
            return ItemManager.GetItemPresetFromCode(code);
            }
        }

    public static ItemPickupData create(ItemPreset preset){
        ItemPickupData result = new ItemPickupData();
        result.code = ItemManager.GetCodeFromItemPreset(preset);
        return result;
    }
}
