using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
    [SerializeField] private List<ItemPreset> itemPresetList = new List<ItemPreset>();
    public GameObject itemPickupPrefab;

    public ItemPreset GetItemPresetFromCode(byte code){
        int index = (int)code;
        return itemPresetList[index];
    }

    public byte GetCodeFromItemPreset(ItemPreset preset){
        for (int i = 0; i < itemPresetList.Count; i++){
            if(itemPresetList[i] == preset){
                return (byte)i;
            }
        }
        return (byte)(0);
    }
    public byte GetCodeFromItemName(string name){
        for (int i = 0; i < itemPresetList.Count; i++){
            if(itemPresetList[i].itemName == name){
                return (byte)i;
            }
        }
        return (byte)(0);
    }

    
}