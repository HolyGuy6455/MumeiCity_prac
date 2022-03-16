using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * 건물 종류에 따라 공통적으로 가지고 있는 속성
 */
[Serializable]
public class BuildingPreset {
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
