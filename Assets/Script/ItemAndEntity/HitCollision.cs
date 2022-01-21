using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour{
    public Tool.ToolType tool;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "HitBox"){
            Hittable hittable = other.gameObject.GetComponentInParent<Hittable>();
            hittable.Hit(tool);
        }
        if(tool == Tool.ToolType.CLAW && other.tag == "Player"){
            // Debug.Log("Player Hurt");
            GameManager.Instance.playerMovement.Hurt();
        }
    }

}
