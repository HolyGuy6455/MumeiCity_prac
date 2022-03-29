using UnityEngine;
using System;
using System.Collections.Generic;

public class GuideManager : MonoBehaviour{
    public List<Guide> guides;
}

[Serializable]
public class Guide{
    public string name;
}