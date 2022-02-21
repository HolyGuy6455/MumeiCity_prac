using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
    [SerializeField] private List<ItemData> itemDataList = new List<ItemData>();
    public GameObject itemPickupPrefab;
    public GameObject itemPickupParent;
    Dictionary<string,ItemData> dataDictionary;

    public static ItemManager Instance{
        get{
            return GameManager.Instance.itemManager;
        }
    }

    private void Awake() {
        Initiate();
    }

    [ContextMenu("Initiate")]
    private void Initiate() {
        dataDictionary = new Dictionary<string, ItemData>();
        foreach (ItemData item in itemDataList){
            dataDictionary[item.itemName] = item;
        }
    }

    public ItemData GetItemData(string name){
        return dataDictionary[name];
    }
    
}