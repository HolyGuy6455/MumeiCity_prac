using System;

public class ManufacturerFunction : IFacilityFunction{
    // 아이템 생산하는곳 추가정보
    public int[] amount;
    public int[] dueDate;

    public ManufacturerFunction(int value){
        amount = new int[value];
        dueDate = new int[value];
    }

    public void ReloadMediocrityData(BuildingData buildingData){
        string[] splitString = buildingData.content.Split('/');
        string[] splitString_Amount = splitString[0].Split();
        string[] splitString_DueDate = splitString[1].Split();
        amount = new int[splitString_Amount.Length];
        dueDate = new int[splitString_DueDate.Length];
        for (int i = 0; i < amount.Length - 1; i++){
            amount[i] = int.Parse(splitString_Amount[i]);
            dueDate[i] = int.Parse(splitString_DueDate[i]);
        }
    }

    public void SaveMediocrityData(BuildingData buildingData){
        string result = "";
        for (int i = 0; i < amount.Length; i++){
            result += amount[i];
            result += " ";
        }
        result += "/";
        for (int i = 0; i < dueDate.Length; i++){
            result += dueDate[i];
            result += " ";
        }
        buildingData.content = result;
    }
}