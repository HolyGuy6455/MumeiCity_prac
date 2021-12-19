using System;

[Serializable]
public class HouseData : MediocrityData{
    public int[] personIDList;
    public int happiness;

    public HouseData(int personIDListLength){
        personIDList = new int[personIDListLength];
        happiness = 0;
    }
}