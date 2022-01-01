using System;

[Serializable]
public class ForesterHutData : MediocrityData{
    public bool doingTreeToLog;
    public bool doingPineconeToTree;

    public ForesterHutData(){
        doingTreeToLog = true;
        doingPineconeToTree = true;
    }
}