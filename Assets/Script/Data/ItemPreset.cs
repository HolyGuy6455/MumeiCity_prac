using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "MumeiCity/Item", order = 0)]
public class ItemPreset : ScriptableObject {
    public string itemName = "new item";
    public Sprite itemSprite = null;
    [TextArea] public string info;

    // public virtual void Use(ItemSlotData itemData){
    //     Debug.Log("Using "+ itemName); 
    // }
}