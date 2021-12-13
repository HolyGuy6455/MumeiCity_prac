using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sence : MonoBehaviour
{
    [SerializeField] List<GameObject> _whatTheySee = new List<GameObject>();
    public List<GameObject> whatTheySee{
        get{
            return _whatTheySee.FindAll(value => value != null);
            }
        }
    int groundLayerMask;
    Predicate<GameObject> _filter;
    public Predicate<GameObject> filter{set{_filter = value;}}

    private void Start() {
        groundLayerMask = LayerMask.NameToLayer("Ground");
    }

    public void resetFilter(){
        _filter = null;
    }

    public GameObject FindNearest(Vector3 position){
        if(_whatTheySee.Count == 0){
            return null;
        }
        List<GameObject> whatTheySeeWithFilter = _whatTheySee;
        if(_filter != null){
            whatTheySeeWithFilter = _whatTheySee.FindAll(_filter);
        }

        GameObject result = null;
        float minDistance = float.MaxValue;
        foreach (GameObject target in whatTheySeeWithFilter){
            if(result == null){
                result = target;
                minDistance = Vector3.Distance(position,target.transform.position);
                continue;
            }
            float distance = Vector3.Distance(position,target.transform.position);
            if(distance < minDistance){
                result = target;
                minDistance = distance;
            }
        }
        return result;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == groundLayerMask){
            return;
        }
        if(_whatTheySee.Contains(other.gameObject)){
            return;
        }
        _whatTheySee.Add(other.gameObject);
        _whatTheySee = whatTheySee.FindAll(value => value != null);
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == groundLayerMask){
            return;
        }
        _whatTheySee.Remove(other.gameObject);
        _whatTheySee = whatTheySee.FindAll(value => value != null);
    }

    
}
