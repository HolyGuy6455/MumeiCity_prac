using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class RabbitBehavior : AnimalBehavior{
    public float jumpPower = 3.0f;
    bool jumpCapable;

    public override void Start() {
        base.Start();
        animalData.animalName = "Rabbit";
    }

    // 길찾기 없이 도망치기
    [Task]
    public override void PanicRunaway(){
        // targetVector3 = targetObject.transform.position;
        if(!jumpCapable){
            return;
        }
        Vector3 direction = (this.transform.position - targetObject.transform.position);
        targetVector3 = this.transform.position + direction;
        Jump();
        ThisTask.Succeed();
    }

    [Task]
    public override void RandomMove(){
        base.RandomMove();
        Jump();
        ThisTask.Succeed();
    }

    public void Jump(){
        if(!jumpCapable){
            return;
        }
        Vector3 movement = (targetVector3 - this.transform.position).normalized;
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
        if(animalData.cautionLevel < 1){
            animator.SetTrigger("Notice");
            animalData.cautionLevel = 1;
            this.jumpCapable = false;
            Debug.Log("Notice!");
        }
    }
}
