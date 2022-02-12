using System;

[Serializable]
public class ManufacturerData : MediocrityData{
    // 아이템 생산하는곳 추가정보
    public int[] amount = new int[3];
    public int[] dueDate  = new int[3];

    public override void ReloadMediocrityData(){
        string[] splitString = this.content.Split('/');
        string[] splitString_Amount = splitString[0].Split();
        string[] splitString_DueDate = splitString[1].Split();
        amount = new int[splitString_Amount.Length];
        dueDate = new int[splitString_DueDate.Length];
        for (int i = 0; i < amount.Length; i++){
            amount[i] = int.Parse(splitString_Amount[i]);
            dueDate[i] = int.Parse(splitString_DueDate[i]);
        }
    }

    public override void SaveMediocrityData(){
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
        this.content = result;
    }
}