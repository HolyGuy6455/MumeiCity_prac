using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "MumeiCity/Tool", order = 0)]
public class Tool : ScriptableObject {
    public Sprite icon = null;
    public ToolType toolType;
    public enum ToolType{
        NONE,
        FISHINGROD,
        KNIFE,
        LANTERN,
        AXE,
        SHOVEL,
        FRYINGPAN,
        CHISEL,
        PICKAXE,
        HAMMER,
        CLAW
    }
}