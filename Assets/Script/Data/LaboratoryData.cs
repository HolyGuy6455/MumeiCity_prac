using System;

[Serializable]
public class LaboratoryData : MediocrityData{
    // 연구소 추가 정보
    public int[] dueDate = new int[3];

    public override void ReloadMediocrityData(){
        string[] splitString = this.content.Split();
        for (int i = 0; i < dueDate.Length; i++){
            dueDate[i] = int.Parse(splitString[i]);
        }
    }

    public override void SaveMediocrityData(){
        string result = "";
        for (int i = 0; i < dueDate.Length; i++){
            result += dueDate[i];
        }
        this.content = result;
    }
}