using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemPickupData
{
    public byte code;
    public Vector3 position;
    public ItemPreset itemPreset{
        get{
            return GameManager.Instance.itemManager.GetItemPresetFromCode(code);
            }
        }

    public static ItemPickupData create(ItemPreset preset){
        ItemPickupData result = new ItemPickupData();
        result.code = GameManager.Instance.itemManager.GetCodeFromItemPreset(preset);
        return result;
    }
}
