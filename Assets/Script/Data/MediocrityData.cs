using System;
using UnityEngine;

[Serializable]
public class MediocrityData {
    [TextArea] public string content;
    public virtual void ReloadMediocrityData(){
        // do nothing;
    }

    public virtual void SaveMediocrityData(){
        // do nothing;
    }
}