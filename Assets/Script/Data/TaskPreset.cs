using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Task", menuName = "MumeiCity/Task", order = 0)]
public class TaskPreset : ScriptableObject {
    public Sprite guideSprite = null;
    [TextArea] public string info;
    public TaskType taskType;
    public List<NecessaryResource> necessaryResources;
    public int requiredTime;


    public enum TaskType
    {
        NONE,
        LABORATORY,
        MANUFACTURER,
        SUPERINTENDENT
    }

}