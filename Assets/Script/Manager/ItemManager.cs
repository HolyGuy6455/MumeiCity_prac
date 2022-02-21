using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
    [SerializeField] private List<ItemPreset> itemPresetList = new List<ItemPreset>();
    [SerializeField] private List<ItemData> itemDataList = new List<ItemData>();
    public GameObject itemPickupPrefab;
    public GameObject itemPickupParent;

    public static ItemPreset GetItemPresetFromCode(byte code){
        int index = (int)code;
        ItemManager mySelf = GameManager.Instance.itemManager;
        return mySelf.itemPresetList[index];
    }

    public static byte GetCodeFromItemPreset(ItemPreset preset){
        List<ItemPreset> itemPresetList = GameManager.Instance.itemManager.itemPresetList;
        for (int i = 0; i < itemPresetList.Count; i++){
            if(itemPresetList[i] == preset){
                return (byte)i;
            }
        }
        return (byte)(0);
    }
    public static byte GetCodeFromItemName(string name){
        List<ItemPreset> itemPresetList = GameManager.Instance.itemManager.itemPresetList;
        for (int i = 0; i < itemPresetList.Count; i++){
            if(itemPresetList[i].name == name){
                return (byte)i;
            }
        }
        return (byte)(0);
    }

    
}