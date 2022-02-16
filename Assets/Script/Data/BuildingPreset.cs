using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * 건물 종류에 따라 공통적으로 가지고 있는 속성
 */
[CreateAssetMenu(fileName = "Building", menuName = "MumeiCity/Building", order = 0)]
public class BuildingPreset : ScriptableObject {
    public GameObject gameObject;
    // public string buildingName;
    public Vector3 scale = new Vector3(1,1,1);
    public Vector3 relativeLocation;
    public Sprite sprite;
    [TextArea] public string info;
    public List<string> attributes;
    public int workTier;
    public List<TaskPreset> taskPresets;
    public Tool.ToolType buildTool;
    public List<EffectiveTool> removalTool;
    public int buildToolIndex;
    public List<NecessaryResource> resourceList = new List<NecessaryResource>();
    public BuildingPreset grownPreset;
    public int growUpTerm = 0;
    public byte code{
        get{
            return BuildingManager.GetBuildingCode(this);
        }
    }
}

[Serializable]
public class EffectiveTool{
    public Tool.ToolType tool;
    public int damage = 5;
    public int minHP = 0;
    public EffectiveTool(Tool.ToolType tool, int damage){
        this.tool = tool;
        this.damage = damage;
    }
}
