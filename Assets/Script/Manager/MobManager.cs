using System.Collections.Generic;
using UnityEngine;
using System;

public class MobManager : MonoBehaviour{

    [Serializable]
    public class MobPrefabInfo{
        public string name;
        public GameObject mobPrefab;
    }
    public GameObject theMotherOfNature;
    [SerializeField] List<MobPrefabInfo> mobPrefabInfos;
    Dictionary<string,GameObject> mobPrefabDictionary;
    
    private void Start() {
        mobPrefabDictionary = new Dictionary<string, GameObject>();
        foreach (MobPrefabInfo mobPrefabInfo in mobPrefabInfos){
            mobPrefabDictionary[mobPrefabInfo.name] = mobPrefabInfo.mobPrefab;
        }
    }

    public GameObject GetPrefab(string name){
        return mobPrefabDictionary[name];
    }
}