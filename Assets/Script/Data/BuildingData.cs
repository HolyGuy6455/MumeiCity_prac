using System;

/*
 * 각 건물이 가지고 있는 정보이자, 저장 포맷
 */
[Serializable]
public class BuildingData {
    public int id;
    public int positionX;
    public int positionY;
    public int positionZ;
    public int hp;
    public byte code;
    public ItemSlotData[] items;
    public int workerID;
    public IFacilityFunction facilityFunction;
    public string content;
    

    public BuildingPreset buildingPreset{
        get{
            return BuildingManager.GetBuildingPreset(code);
            }
        }

    public bool AddItem(ItemSlotData itemSlotData){
        for (int i = 0; i < 4; i++){
            if(items[i].itemName == itemSlotData.itemName){
                items[i].amount += itemSlotData.amount;
                itemSlotData.amount = 0;
                itemSlotData.itemName = "None";
                return true;
            }
        }
        for (int i = 0; i < 4; i++){
            if(items[i].itemData.isNone()){
                items[i].itemName = itemSlotData.itemName;
                items[i].amount = itemSlotData.amount;
                itemSlotData.amount = 0;
                itemSlotData.itemName = "None";
                return true;
            }
        }
        return false;
    }

}