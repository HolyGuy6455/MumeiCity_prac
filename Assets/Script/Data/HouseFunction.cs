using System;

[Serializable]
public class HouseFunction : IFacilityFunction{
    public int[] personIDList;
    BuildingData buildingData;

    public HouseFunction(int personIDListLength){
        personIDList = new int[personIDListLength];
    }

    public void ReloadMediocrityData(BuildingData buildingData){
        string[] splitString = buildingData.content.Split();
        personIDList = new int[splitString.Length -1];
        for (int i = 0; i < splitString.Length - 1; i++){
            personIDList[i] = int.Parse(splitString[i]);
        }
        this.buildingData = buildingData;
    }

    public void SaveMediocrityData(BuildingData buildingData){
        string result = "";
        for (int i = 0; i < this.personIDList.Length; i++){
            result += this.personIDList[i];
            result += " ";
        }
        buildingData.content = result;
    }

    public int GetEmptyRoomIndex(){
        for (int i = 0; i < personIDList.Length; i++){
            if(personIDList[i] == 0){
                return i;
            }
        }
        return -1;
    }

    public int GetPersonRoomIndex(int personID){
        for (int i = 0; i < personIDList.Length; i++){
            if(personIDList[i] == personID){
                return i;
            }
        }
        return -1;
    }

    public void LiveIn(int personID){
        PersonBehavior person = PeopleManager.FindPersonWithID(personID);
        person.personData.homeID = this.buildingData.id;
        personIDList[GetEmptyRoomIndex()] = personID;
        SaveMediocrityData(buildingData);
    }

    public void LiveOut(int personID){
        PersonBehavior person = PeopleManager.FindPersonWithID(personID);
        person.personData.homeID = 0;
        personIDList[GetPersonRoomIndex(personID)] = 0;
        SaveMediocrityData(buildingData);
    }

}