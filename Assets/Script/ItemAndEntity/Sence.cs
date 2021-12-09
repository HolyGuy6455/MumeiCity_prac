using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sence : MonoBehaviour
{
    [SerializeField] List<GameObject> _whatTheySee = new List<GameObject>();
    public List<GameObject> whatTheySee{
        get{
            return _whatTheySee.FindAll(value => value != null);
            }
        }
    int groundLayerMask;

    private void Start() {
        groundLayerMask = LayerMask.NameToLayer("Ground");
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
