using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour{
    public Tool.ToolType tool;

    private void OnTriggerEnter(Collider other) {
        if(other.tag != "HitBox"){
            return;
        }
        Hittable hittable = other.gameObject.GetComponentInParent<Hittable>();
        Debug.Log(hittable);
        hittable.Hit(tool);
    }

}
