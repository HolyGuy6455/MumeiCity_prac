using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PersonData : MediocrityData {
    public int id;
    public Vector3 position = new Vector3();
    public List<ItemSlotData> items = new List<ItemSlotData>();
    public int stamina;
    public int happiness;
    public int workplaceID;
    public int jobID;
    public int homeID;
    public bool sleep;
    public float growth = 1.0f;
}