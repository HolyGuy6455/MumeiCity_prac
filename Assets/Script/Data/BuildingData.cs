using System;

/*
 * 각 건물이 가지고 있는 정보이자, 저장 포맷
 */
[Serializable]
public class BuildingData {
    public int positionX;
    public int positionY;
    public int positionZ;
    public byte code;
    public ItemSlotData[] items;

    public BuildingPreset buildingPreset{
        get{
            return BuildingManager.GetBuildingPreset(code);
            }
        }
}