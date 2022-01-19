using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollider : Sence{
    public float volume;
    public string soundSource;
    void Start()
    {
        this.filter=delegate(GameObject gameObject){
            if(gameObject == null)
                return false;
            IHeraingEar heraingEar = gameObject.GetComponentInParent<IHeraingEar>();
            if(heraingEar == null)
                return false;
            return true;
        };
    }

    void Update(){
        volume = this.transform.localScale.sqrMagnitude;
        List<GameObject> whatTheySeeList = whatTheySee;
        if(whatTheySeeList.Count == 0){
            return;
        }
        foreach (GameObject gameObject in whatTheySeeList){
            IHeraingEar heraingEar = gameObject.GetComponentInParent<IHeraingEar>();
            heraingEar.Hear(soundSource);
        }
    }
}
