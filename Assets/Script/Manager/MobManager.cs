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
    public GameObject mobSpawnerParent;
    [SerializeField] List<MobPrefabInfo> mobPrefabInfos;
    Dictionary<string,GameObject> mobPrefabDictionary;
    [SerializeField] List<MobSpawner> mobSpawners;
    
    private void Start() {
        mobPrefabDictionary = new Dictionary<string, GameObject>();
        foreach (MobPrefabInfo mobPrefabInfo in mobPrefabInfos){
            mobPrefabDictionary[mobPrefabInfo.name] = mobPrefabInfo.mobPrefab;
        }
        MobSpawner[] spawnersArray = mobSpawnerParent.GetComponentsInChildren<MobSpawner>();
        foreach (MobSpawner spawner in spawnersArray){
            mobSpawners.Add(spawner);
        }
    }

    public GameObject GetPrefab(string name){
        return mobPrefabDictionary[name];
    }

    public void Spawn(){
        foreach (MobSpawner spawner in mobSpawners){
            spawner.Spawn();
        }
    }
}