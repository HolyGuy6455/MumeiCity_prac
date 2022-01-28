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

    [SerializeField] Transform wolfSpawnPoint;
    [SerializeField] Transform rabbitSpawnPoint;
    [SerializeField] Transform reindeerSpawnPoint;
    
    private void Start() {
        mobPrefabDictionary = new Dictionary<string, GameObject>();
        foreach (MobPrefabInfo mobPrefabInfo in mobPrefabInfos){
            mobPrefabDictionary[mobPrefabInfo.name] = mobPrefabInfo.mobPrefab;
        }
    }

    public GameObject GetPrefab(string name){
        return mobPrefabDictionary[name];
    }

    public void Spawn(){
        GameObject[] animalObjects = GameObject.FindGameObjectsWithTag("Animal");
        for (int i = animalObjects.Length; i < 10; i++){
            float dice = UnityEngine.Random.Range(0.0f,1.0f);
            GameObject animalPrefab = null;
            Vector3 location = new Vector3();

            if(dice > 0.8f){
                location = wolfSpawnPoint.position;
                animalPrefab = mobPrefabDictionary["Wolf"];
            }else if(dice > 0.4f){
                location = rabbitSpawnPoint.position;
                animalPrefab = mobPrefabDictionary["Rabbit"];
            }else{
                location = reindeerSpawnPoint.position;
                animalPrefab = mobPrefabDictionary["Reindeer"];
            }

            // 데이터로 동물을 생성한다
            GameObject animalObject = Instantiate(animalPrefab,location,Quaternion.identity);
            AnimalBehavior animalBehavior = animalObject.GetComponent<AnimalBehavior>();
            animalObject.transform.SetParent(theMotherOfNature.transform);
        }
    }
}