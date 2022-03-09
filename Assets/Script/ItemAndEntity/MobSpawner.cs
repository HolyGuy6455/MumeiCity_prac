using System.Collections.Generic;
using UnityEngine;
using System;

public class MobSpawner : MonoBehaviour{
    [SerializeField] GameObject spawnMob;
    [SerializeField] string animalName;
    [SerializeField] Sence sence;
    [SerializeField] int minNumOfSpawn;

    private void Awake() {
        sence.filter = delegate(GameObject value){
            if(value == null)
                return false;

            AnimalBehavior animal = value.GetComponentInParent<AnimalBehavior>();
            if(animal == null)
                return false;
            
            return animalName.CompareTo(animal.animalData.animalName) == 0;
        };
    }

    public void Spawn(){
        Vector3 location = this.transform.position;
        int indexMax = minNumOfSpawn - sence.whatTheySee.Count;
        if(indexMax <= 0){
            return;
        }

        for (int i = 0; i < indexMax; i++){
            GameObject animalObject = Instantiate(spawnMob,location,Quaternion.identity);
            AnimalBehavior animalBehavior = animalObject.GetComponent<AnimalBehavior>();
            animalBehavior.animalData.animalName = animalName;
            animalObject.transform.SetParent(GameManager.Instance.mobManager.theMotherOfNature.transform);
            Debug.Log("Mob Spawn!! "+animalName);
        }
    }
}