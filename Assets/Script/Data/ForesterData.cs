using System;

[Serializable]
public class ForesterHutData : MediocrityData{
    public bool doingTreeToLog = true;
    public bool doingSeedToTree = true;


    public override void ReloadMediocrityData(){
        string[] splitString = this.content.Split();
        doingTreeToLog = (splitString[0].CompareTo("t")==0);
        doingSeedToTree = (splitString[1].CompareTo("t")==0);
    }

    public override void SaveMediocrityData(){
        string result = "";
        result += (this.doingTreeToLog)?"t ":"f ";
        result += (this.doingSeedToTree)?"t ":"f ";
        this.content = result;
    }
}