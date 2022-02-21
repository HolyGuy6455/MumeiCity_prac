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

    // public bool ReduceItem(byte ItemCode, int amount){
    //     for (int i = 0; i < 4; i++){
    //         if(items[i].code == ItemCode && items[i].amount > amount){
    //             items[i].amount -= amount;
    //             return true;
    //         }
    //     }
    //     return false;
    // }

}