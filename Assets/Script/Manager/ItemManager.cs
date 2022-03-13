using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
    [SerializeField] private List<ItemData> itemDataList = new List<ItemData>();
    public GameObject itemPickupPrefab;
    public GameObject itemPickupParent;
    Dictionary<string,ItemData> dataDictionary;
    public int lastID = 1;

    public static ItemManager Instance{
        get{
            return GameManager.Instance.itemManager;
        }
    }

    public int IssueIDOne(){
        lastID++;
        return lastID;
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
        if(name.CompareTo("")==0){
            return dataDictionary["None"];    
        }
        if(dataDictionary == null){
            Initiate();
        }
        if(!dataDictionary.ContainsKey(name)){
            Debug.Log(name + " is not found in the item dictionary");
        }
        return dataDictionary[name];    
    }
    
}