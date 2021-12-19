using System;

/*
 * 각 건물이 가지고 있는 정보이자, 저장 포맷
 */
[Serializable]
public class BuildingData : MediocrityData {
    public int id;
    public int positionX;
    public int positionY;
    public int positionZ;
    public byte code;
    public ItemSlotData[] items;
    public int workerID;
    public MediocrityData mediocrityData;

    public BuildingPreset buildingPreset{
        get{
            return BuildingManager.GetBuildingPreset(code);
            }
        }

    
    public bool AddItem(ItemSlotData itemSlotData){
        for (int i = 0; i < 4; i++){
            if(items[i].code == itemSlotData.code){
                items[i].amount += itemSlotData.amount;
                itemSlotData.amount = 0;
                itemSlotData.code = 0;
                return true;
            }
        }
        for (int i = 0; i < 4; i++){
            if(items[i].code == 0){
                items[i].code = itemSlotData.code;
                items[i].amount = itemSlotData.amount;
                itemSlotData.amount = 0;
                itemSlotData.code = 0;
                return true;
            }
        }
        return false;
    }

}