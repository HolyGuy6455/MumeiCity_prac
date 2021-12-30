using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * 건물 종류에 따라 공통적으로 가지고 있는 속성
 */
[CreateAssetMenu(fileName = "Building", menuName = "MumeiCity/Building", order = 0)]
public class BuildingPreset : ScriptableObject {
    // public string buildingName;
    public Vector3 scale;
    public Vector3 relativeLocation;
    public Sprite sprite;
    public List<string> attributes;
    public bool interactable;
    public bool workplace;
    public string toolType;
    public int toolTypeIndex;
    public List<BuildingMaterial> materialList = new List<BuildingMaterial>();
    public List<ItemDropAmount> dropAmounts = new List<ItemDropAmount>();
    public byte code{
        get{
            return BuildingManager.GetBuildingCode(this);
        }
    }
    /*
     * 기억 못하겠어서 적어두는 건물 코드
     * 65 = 집
     * 96 = 나무
     * 97 = 숲지기의 집
     *
     */

}
[Serializable]
public class BuildingMaterial{
    public string name;
    public int amount;
}