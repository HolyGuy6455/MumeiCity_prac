using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemPickupData{
    public string itemName;
    public Vector3 position;
    public int leftSecond = 300;
    public int ID;
    public ItemData itemData{
        get{
            return ItemData.Instant(itemName);
            }
        }

    public static ItemPickupData create(ItemData data){
        ItemPickupData result = new ItemPickupData();
        result.itemName = data.itemName;
        return result;
    }
}
