using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * 건물 종류에 따라 공통적으로 가지고 있는 속성
 */
[CreateAssetMenu(fileName = "Building", menuName = "MumeiCity/Building", order = 0)]
public class BuildingPreset : ScriptableObject {
    [SerializeField] private string buildingName = "new item";
    [SerializeField] private Vector3 _scale;
    [SerializeField] private Vector3 _relativeLocation;
    [SerializeField] private Sprite _sprite = null;
    [SerializeField] private List<string> _attributes = new List<string>();
    public Vector3 scale{get{return _scale;}}
    public Vector3 relativeLocation{get{return _relativeLocation;}}
    public Sprite sprite{get{return _sprite;}}
    public List<string> attributes{get{return _attributes;}}
    public string toolType;
    public int toolTypeIndex;
    public List<BuildingMaterial> materialList = new List<BuildingMaterial>();
    public List<ItemDropAmount> dropAmounts = new List<ItemDropAmount>();

    public bool hasAttribute(string attribute){
        return _attributes.Contains(attribute);
    }

}
[Serializable]
public class BuildingMaterial{
    public string name;
    public int amount;
}