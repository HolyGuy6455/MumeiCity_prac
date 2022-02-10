using System;

[Serializable]
public class LaboratoryData : MediocrityData{
    // 연구소 추가 정보
    public float[] upgradeList;

    public override void ReloadMediocrityData(){
        string[] splitString = this.content.Split();
        for (int i = 0; i < upgradeList.Length; i++){
            upgradeList[i] = float.Parse(splitString[i]);
        }
    }

    public override void SaveMediocrityData(){
        string result = "";
        for (int i = 0; i < upgradeList.Length; i++){
            result += upgradeList[i];
        }
        this.content = result;
    }
}