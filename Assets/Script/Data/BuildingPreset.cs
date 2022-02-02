using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * 건물 종류에 따라 공통적으로 가지고 있는 속성
 */
[CreateAssetMenu(fileName = "Building", menuName = "MumeiCity/Building", order = 0)]
public class BuildingPreset : ScriptableObject {
    // public string buildingName;
    public Vector3 scale = new Vector3(1,1,1);
    public Vector3 relativeLocation;
    public Sprite sprite;
    public int healthPointMax = 10;
    // public Sprite spriteBroken;
    [TextArea] public string info;
    public List<string> attributes;
    public bool interactable;
    public bool workplace;
    public int workTier;
    public int lightSourceIntensity;
    public Tool.ToolType buildTool;
    public List<EffectiveTool> removalTool;
    public int buildToolIndex;
    public List<BuildingResource> resourceList = new List<BuildingResource>();
    public List<ItemDropInfo> dropAmounts = new List<ItemDropInfo>();
    public BuildingPreset grownPreset;
    public BuildingPreset ruinPreset;
    public int growUpTerm = 0;
    public BrokenStyle brokenStyle;
    public byte code{
        get{
            return BuildingManager.GetBuildingCode(this);
        }
    }
    public enum BrokenStyle{
        NONE,
        COLLAPSE,
        FALL_DOWN,
        SWAP
    }
}
[Serializable]
public class BuildingResource{
    public ItemPreset preset;
    public int amount;
}
[Serializable]
public class EffectiveTool{
    public Tool.ToolType tool;
    public int damage;
    public EffectiveTool(Tool.ToolType tool, int damage){
        this.tool = tool;
        this.damage = damage;
    }
}
