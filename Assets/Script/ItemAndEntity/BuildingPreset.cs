using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * 건물 종류에 따라 공통적으로 가지고 있는 속성
 */
[CreateAssetMenu(fileName = "Building", menuName = "MumeiCity/Building", order = 0)]
public class BuildingPreset : ScriptableObject {
    [SerializeField] private string buildingName = "new item";
    public string toolType;
    public int toolTypeIndex;
    [SerializeField] private Sprite _sprite = null;
    public Sprite sprite{get{return _sprite;}}
    public List<BuildingMaterial> materialList = new List<BuildingMaterial>();
    public List<ItemDropAmount> dropAmounts = new List<ItemDropAmount>();
    [SerializeField] private Vector3 _scale;
    public Vector3 scale{get{return _scale;}}
    [SerializeField] private Vector3 _relativeLocation;
    public Vector3 relativeLocation{get{return _relativeLocation;}}

}
[Serializable]
public class BuildingMaterial{
    public string name;
    public int amount;
}