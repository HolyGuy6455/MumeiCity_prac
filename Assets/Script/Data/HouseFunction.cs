using System;

[Serializable]
public class HouseFunction : IFacilityFunction{
    public int[] personIDList;

    public HouseFunction(int personIDListLength){
        personIDList = new int[personIDListLength];
    }

    public void ReloadMediocrityData(BuildingData buildingData){
        string[] splitString = buildingData.content.Split();
        personIDList = new int[splitString.Length -1];
        for (int i = 0; i < splitString.Length - 1; i++){
            personIDList[i] = int.Parse(splitString[i]);
        }
    }

    public void SaveMediocrityData(BuildingData buildingData){
        string result = "";
        for (int i = 0; i < this.personIDList.Length; i++){
            result += this.personIDList[i];
            result += " ";
        }
        buildingData.content = result;
    }
}