using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class ItemData{
    public string itemName;
    public Sprite itemSprite = null;
    [TextArea] public string info;
    public string tag;
    public int efficiency;

    public bool isThisName(string value){
        return (itemName.CompareTo(value) == 0);
    }

    public bool isNone(){
        return (itemName.CompareTo("None") == 0);
    }

    public bool isThisTag(string value){
        return (tag.CompareTo(value) == 0);
    }

    public static ItemData Instant(string name){
        
        return GameManager.Instance.itemManager.GetItemData(name);
    }
}