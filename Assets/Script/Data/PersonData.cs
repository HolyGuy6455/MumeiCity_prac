using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PersonData{
    public Vector3 position = new Vector3();
    public List<ItemSlotData> items = new List<ItemSlotData>();
}