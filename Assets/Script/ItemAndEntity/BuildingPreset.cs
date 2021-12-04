using UnityEngine;
using System.Collections.Generic;

/*
 * 건물 종류에 따라 공통적으로 가지고 있는 속성
 */
[CreateAssetMenu(fileName = "Building", menuName = "MumeiCity/Building", order = 0)]
public class BuildingPreset : ScriptableObject {
    [SerializeField]
    private string buildingName = "new item";
    public string toolType;
    public int toolTypeIndex;
    [SerializeField]
    private Sprite _sprite = null;
    public Sprite sprite{get{return _sprite;}}
    [SerializeField]
    private List<string> itemNameList = new List<string>();
    [SerializeField]
    private List<int> itemAmountList = new List<int>();
    [SerializeField]
    private Vector3 _scale;
    public Vector3 scale{get{return _scale;}}
    [SerializeField]
    private Vector3 _relativeLocation;
    public Vector3 relativeLocation{get{return _relativeLocation;}}

    public List<(string,int)> GetMaterials(){
        List<(string,int)> result = new List<(string,int)>();
        string itemName = "";
        int itemAmount = 0;

        for (int i = 0; i < itemNameList.Count; i++){ 
            itemName = itemNameList[i];
            itemAmount = itemAmountList[i];
            result.Add((itemName,itemAmount));
        }

        return result;
    }
}