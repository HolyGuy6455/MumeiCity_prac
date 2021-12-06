using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "MumeiCity/Item", order = 0)]
public class ItemPreset : ScriptableObject {
    public string itemName = "new item";
    public Sprite icon = null;

    public virtual void Use(ItemData itemData){
        Debug.Log("Using "+ itemName); 
    }
}