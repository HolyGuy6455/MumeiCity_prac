using System;

[Serializable]
public class HouseData : MediocrityData{
    public int[] personIDList;

    public HouseData(int personIDListLength){
        personIDList = new int[personIDListLength];
    }
}