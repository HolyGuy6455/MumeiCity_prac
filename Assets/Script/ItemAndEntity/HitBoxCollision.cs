using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollision : MonoBehaviour{
    public int test;
    public Tool.ToolType tool;

    private void OnTriggerEnter(Collider other) {
        if(other.tag != "HitBox"){
            return;
        }
        // Debug.Log("TriggerEnter "+other.gameObject);
        other.gameObject.GetComponentInParent<Hittable>().Hit(tool);
    }

}
