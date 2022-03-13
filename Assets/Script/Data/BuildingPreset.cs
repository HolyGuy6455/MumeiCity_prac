using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * 건물 종류에 따라 공통적으로 가지고 있는 속성
 */
[CreateAssetMenu(fileName = "Building", menuName = "MumeiCity/Building", order = 0)]
public class BuildingPreset : ScriptableObject {
    public string buildingName;
    public GameObject prefab;
    public Vector3 scale = new Vector3(1,1,1);
    public float spriteScale = 1.0f;
    public Vector3 relativeLocation;
    public Sprite sprite;
    [TextArea] public string info;
    public List<string> attributes;
    public ToolType buildTool;
    public List<NecessaryResource> resourceList = new List<NecessaryResource>();
}

[Serializable]
public class BuildingPreset2nd {
    public string name;
    public GameObject prefab;
    public float spriteScale = 1.0f;
    public Vector3 scale = new Vector3(1,1,1);
    public Vector3 relativeLocation;
    public Sprite sprite;
    public string info;
    public List<string> attributes;
    public ToolType buildTool;
    public List<NecessaryResource> resourceList = new List<NecessaryResource>();
}

[Serializable]
public class EffectiveTool{
    public ToolType tool;
    public int damage = 5;
    public int minHP = 0;
    public EffectiveTool(ToolType tool, int damage){
        this.tool = tool;
        this.damage = damage;
    }
}
