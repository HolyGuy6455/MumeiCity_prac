using System.Collections.Generic;
using UnityEngine;

public class ChestFunction : IFacilityFunction{

    public void ReloadMediocrityData(BuildingData buildingData){
        string[] splitString = buildingData.content.Split('/');
        List<ItemDropInfo> dropAmounts = new List<ItemDropInfo>();
        for (int i = 0; i < splitString.Length - 1; i++){
            ItemDropInfo itemDropInfo = new ItemDropInfo();
            string[] splitString_Sub = splitString[i].Split('-');
            itemDropInfo.itemName = splitString_Sub[0];
            itemDropInfo.chance = float.Parse(splitString_Sub[1]);
            itemDropInfo.itemDropType = ItemDropInfo.ItemDropType.DESTROY;
            dropAmounts.Add(itemDropInfo);
        }
        Debug.Log("chest load " + buildingData.id);
        BuildingObject building = GameManager.Instance.buildingManager.FindBuildingObjectWithID(buildingData.id);
        ItemDroper itemDroper = building.GetComponent<ItemDroper>();
        itemDroper.InitializeItemDrop(dropAmounts);
    }

    public void SaveMediocrityData(BuildingData buildingData){
        Debug.Log("chest save " + buildingData.id);
        BuildingObject building = GameManager.Instance.buildingManager.FindBuildingObjectWithID(buildingData.id);
        ItemDroper itemDroper = building.GetComponent<ItemDroper>();
        List<ItemDropInfo> dropAmounts = itemDroper.GetDropInfos();
        string result = "";
        for (int i = 0; i < dropAmounts.Count; i++){
            result += dropAmounts[i].itemName;
            result += "-";
            result += dropAmounts[i].chance;
            result += "/";
        }
        buildingData.content = result;
    }
}