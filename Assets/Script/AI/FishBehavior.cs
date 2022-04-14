using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Pathfinding;

public class FishBehavior : AnimalBehavior{
    IAstarAI ai;

    public override void Start() {
        base.Start();
        animalData.animalName = "Fish";
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
    }


    [Task]
    public override void RandomMove(){
        int count = 0;
        while(walkAround == false){
            targetVector3 = new Vector3();
            targetVector3.x = this.transform.position.x 
                                + Random.Range(walkAroundRangeMin, walkAroundRangeMax) * ((Random.Range(0,2)>0)?1:-1);
            targetVector3.y = this.transform.position.y;
            targetVector3.z = this.transform.position.z 
                                + Random.Range(walkAroundRangeMin, walkAroundRangeMax) * ((Random.Range(0,2)>0)?1:-1);
            targetVector3 *= (float)(100 + count)/100;
            if(GameManager.Instance.gridMapManager.amIInWater(targetVector3)){
                walkAround = true;
            }
            count++;
            if(count > 1000){
                walkAround = true;
            }
        }

        float distanceValue = Vector3.Distance(this.transform.position,targetVector3);
        distanceToTarget = distanceValue;
        
        if(distanceValue < 0.3f ){
            targetVector3 = this.transform.position;
            distanceToTarget = 0 ;
            ThisTask.Succeed();
            walkAround = false;
        }
    }

    [Task]
    public void GoToBait(){
        sence.filter = delegate(GameObject value){
            if(value == null)
                return false;
            if(value.tag != "Bait")
                return false;
            return true;
        };
        GameObject nearestItem = sence.FindNearest();
        if(nearestItem != null){
            targetVector3 = nearestItem.transform.position;
        }else{
            targetObject = null;
            ThisTask.Fail();
        }
    }

    [Task]
    public void Runaway(float distance){
        Vector3 direction = (this.transform.position - targetObject.transform.position).normalized;
        for (int i = 0; i < 10; i++){
            targetVector3 = targetObject.transform.position + direction*distance;    
            if(GameManager.Instance.gridMapManager.amIInWater(targetVector3)){
                break;
            }
            distance /= 2;
        }

        float distanceValue = Vector3.Distance(this.transform.position,targetObject.transform.position);
        distanceToTarget = distanceValue;

        if(distanceValue > distance - 0.1){
            Debug.Log("Runaway : "+distanceValue);
            targetVector3 = this.transform.position;
            distanceToTarget = 0 ;
            ThisTask.Succeed();
        }
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
