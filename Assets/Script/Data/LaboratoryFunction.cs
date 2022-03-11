using System;
using UnityEngine;

public class LaboratoryFunction : IFacilityFunction{
    // 연구소 추가 정보
    public int[] dueDate;

    public LaboratoryFunction(int count){
        dueDate = new int[count];
        for (int i = 0; i < count; i++){
            dueDate[i] = int.MaxValue;
        }
    }

    public void ReloadMediocrityData(BuildingData buildingData){
        string[] splitString = buildingData.content.Split();
        for (int i = 0; i < splitString.Length - 1; i++){
            Debug.Log(i + " -" + splitString[i] + "- ");
            dueDate[i] = int.Parse(splitString[i]);
        }
    }

    public void SaveMediocrityData(BuildingData buildingData){
        string result = "";
        for (int i = 0; i < dueDate.Length; i++){
            result += dueDate[i];
        }
        buildingData.content = result;
    }
}