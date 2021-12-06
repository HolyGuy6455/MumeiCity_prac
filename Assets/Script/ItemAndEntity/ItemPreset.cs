using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "MumeiCity/Item", order = 0)]
public class ItemPreset : ScriptableObject {
    public string itemName = "new item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public int amount = 1;

    public virtual void Use(){
        Debug.Log("Using "+ itemName); 
    }
}