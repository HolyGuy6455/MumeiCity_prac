using System;

[Serializable]
public class HouseData : MediocrityData{
    public int[] personIDList;

    public HouseData(int personIDListLength){
        personIDList = new int[personIDListLength];
    }

    public override void ReloadMediocrityData(){
        string[] splitString = content.Split();
        personIDList = new int[splitString.Length];
        for (int i = 0; i < splitString.Length; i++){
            personIDList[i] = int.Parse(splitString[i]);
        }
    }

    public override void SaveMediocrityData(){
        string result = "";
        for (int i = 0; i < this.personIDList.Length; i++){
            result += this.personIDList[i];
            result += " ";
        }
        this.content = result;
    }
}