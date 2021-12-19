using System;

[Serializable]
public class ForesterData : MediocrityData{
    public bool doingTreeToLog;
    public bool doingPineconeToTree;

    public ForesterData(){
        doingTreeToLog = true;
        doingPineconeToTree = true;
    }
}