using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour{
    public ToolType tool;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "HitBox"){
            Hittable hittable = other.gameObject.GetComponentInParent<Hittable>();
            hittable.Hit(tool);
        }
        if(tool == ToolType.CLAW && other.tag == "Player"){
            GameManager.Instance.playerMovement.Hurt();
        }
    }

}
