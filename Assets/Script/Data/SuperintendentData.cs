using System;

[Serializable]
public class SuperintendentData : MediocrityData{
    // 일하는곳 추가 정보
    public bool[] workList;

    public override void ReloadMediocrityData(){
        string[] splitString = this.content.Split();
        for (int i = 0; i < workList.Length; i++){
            workList[i] = (splitString[i].CompareTo("t")==0);
        }
    }

    public override void SaveMediocrityData(){
        string result = "";
        for (int i = 0; i < workList.Length; i++){
            result += (workList[i])?"t ":"f ";
        }
        this.content = result;
    }
}