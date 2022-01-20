using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class RabbitBehavior : AnimalBehavior{
    public float jumpPower = 3.0f;
    bool jumpCapable;

    // 길찾기 없이 도망치기
    [Task]
    public override void PanicRunaway(){
        // targetVector3 = targetObject.transform.position;
        Vector3 direction = -1*(this.transform.position - targetObject.transform.position);
        targetVector3 = this.transform.position + direction;
        Jump();
    }
    // 길찾기로 도망치기
    [Task]
    public override void Runaway(){
        
    }

    public void Jump(){
        if(!jumpCapable){
            return;
        }
        Vector3 movement = (this.transform.position-targetVector3).normalized;
        this.animalRigidbody.AddForce(new Vector3(movement.x,1,movement.z)*jumpPower,ForceMode.Impulse);
        if(movement.x <= -0.01f){
            transform.localScale = new Vector3(1f,1f,1f);
        }else if(movement.x >= 0.01f){
            transform.localScale = new Vector3(-1f,1f,1f);
        }
        jumpCapable = false;
    }

    void EnableJump(){
        this.jumpCapable = true;
    }
    void DisableJump(){
        this.jumpCapable = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetVector3, 0.1f);
    }

    public override void Hear(string soundSource){

    }
}
