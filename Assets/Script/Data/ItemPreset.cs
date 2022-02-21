using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Item", menuName = "MumeiCity/Item", order = 0)]
public class ItemPreset : ScriptableObject {
    public Sprite itemSprite = null;
    [TextArea] public string info;
    public List<string> tags = new List<string>();
    public int efficiency;

    // public virtual void Use(ItemSlotData itemData){
    //     Debug.Log("Using "+ itemName); 
    // }
}

[Serializable]
public class ItemData{
    public string itemName;
    public Sprite itemSprite = null;
    [TextArea] public string info;
    public string tag;
    public int efficiency;
}