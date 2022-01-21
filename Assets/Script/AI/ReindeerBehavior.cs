using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Pathfinding;

public class ReindeerBehavior : AnimalBehavior{
    [SerializeField] Transform spriteTransform;
    IAstarAI ai;

    public override void Start() {
        base.Start();
        animalData.animalName = "Reindeer";
    }

    public override void Update() {
        base.Update();
        // targetMarker.transform.position = targetVector3;
        if (targetVector3 != null && ai != null){
            ai.destination = targetVector3;
        }else{
            ai.destination = this.transform.position;
        }
        // animalData
        animator.SetFloat("DIstanceToTarget",distanceToTarget);

        float movementX = targetVector3.x - this.transform.position.x;
        if(movementX <= -0.01f){
            spriteTransform.localScale = new Vector3(-1f,1f,1f);
        }else if(movementX >= 0.01f){
            spriteTransform.localScale = new Vector3(1f,1f,1f);
        }
    }

    // 길찾기로 도망치기
    [Task]
    public void Runaway(float distance){
        Vector3 direction = (this.transform.position - targetObject.transform.position).normalized;
        targetVector3 = targetObject.transform.position + direction*distance;

        float distanceValue = Vector3.Distance(this.transform.position,targetObject.transform.position);
        distanceToTarget = distanceValue;

        if(distanceValue > distance - 0.1){
            Debug.Log("Runaway : "+distanceValue);
            targetVector3 = this.transform.position;
            distanceToTarget = 0 ;
            ThisTask.Succeed();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetVector3, 0.1f);
    }

    public override void Hear(string soundSource){
        if(animalData.cautionLevel < 1){
            animator.SetTrigger("Notice");
            animalData.cautionLevel = 1;
        }
    }
    
    void OnEnable () {
        ai = GetComponent<IAstarAI>();
        if (ai != null) ai.onSearchPath += Update;
    }

    void OnDisable () {
        if (ai != null) ai.onSearchPath -= Update;
    }
}
