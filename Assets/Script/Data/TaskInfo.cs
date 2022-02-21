using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class TaskInfo {
    public string name;
    public Sprite guideSprite = null;
    [TextArea] public string info;
    public TaskType taskType;
    public List<NecessaryResource> necessaryResources;
    public NecessaryResource resultItem;
    public string resultUpgrade;
    public int requiredTime;

    public enum TaskType
    {
        NONE,
        LABORATORY,
        MANUFACTURER,
        SUPERINTENDENT
    }

}