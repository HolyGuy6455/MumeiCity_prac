using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Pathfinding;

public class WolfBehavior : AnimalBehavior{
    [SerializeField] float speed = 3;
    [SerializeField] float jumpPower = 3.0f;
    [SerializeField] Transform spriteTransform;
    [SerializeField] GameObject targetMarker;
    [SerializeField] bool jumpCapable;
    [SerializeField] bool moveCapable;
    [SerializeField] FMODUnity.StudioEventEmitter growlSound;
    IAstarAI ai;
    static List<string> preyList = new List<string>{"Rabbit","Reindeer"};

    public override void Start() {
        base.Start();
        InvokeRepeating("UpdatePath", 0f, 1f);
        moveCapable = true;
        animalData.animalName = "Wolf";
    }

    public override void Update() {
        base.Update();
        // targetMarker.transform.position = targetVector3;
        if (moveCapable && targetVector3 != null && ai != null){
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

        float distanceFromPlayer = Vector3.Distance(GameManager.Instance.PlayerTransform.position, this.transform.position);
        growlSound.SetParameter("Distance",distanceFromPlayer/20);
        animator.SetFloat("Tension", animalData.cautionLevel );
    }

    // 길찾기 없이 도망치기
    [Task]
    public override void PanicRunaway(){
        // targetVector3 = targetObject.transform.position;
        if(!jumpCapable){
            return;
        }
        Vector3 direction = -1*(this.transform.position - targetObject.transform.position);
        targetVector3 = this.transform.position + direction;
        Jump();
        ThisTask.Succeed();
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

    // 목표에 접근하기
    [Task]
    public void GoCloser(float distance){
        targetVector3 = targetObject.transform.position;

        float distanceValue = Vector3.Distance(this.transform.position,targetObject.transform.position);
        distanceToTarget = distanceValue;
        
        if(distanceValue < distance + 0.1){
            Debug.Log("GoCloser : "+distanceValue);
            targetVector3 = this.transform.position;
            distanceToTarget = 0 ;
            ThisTask.Succeed();
        }
    }

    [Task]
    public void LockTarget(){
        targetVector3 = targetObject.transform.position;
        moveCapable = false;
        ThisTask.Succeed();
    }

    // 공격하기
    [Task]
    public override void Attack(){
        if(!jumpCapable){
            return;
        }
        animator.SetTrigger("Attack");
        moveCapable = true;
        Jump();
        ThisTask.Succeed();
    }

    [Task]
    public void SearchItemToPickUp(string itemTag){
        sence.filter = delegate(GameObject value){
            if(value == null)
                return false;

            ItemPickup itemPickup = value.GetComponent<ItemPickup>();
            if(itemPickup == null)
                return false;
            if(itemTag == null)
                return true;

            return itemPickup.itemPickupData.itemData.isThisTag(itemTag);
        };
        GameObject nearestItem = sence.FindNearest();
        if(nearestItem != null){
            targetObject = nearestItem;
            ThisTask.Succeed();
        }else{
            targetObject = null;
            ThisTask.Fail();
        }
    }

    [Task]
    public void SearchPreyToHunt(){
        sence.filter = delegate(GameObject value){
            if(value == null)
                return false;

            AnimalBehavior animal = value.GetComponentInParent<AnimalBehavior>();
            if(animal == null)
                return false;

            return (preyList.Contains(animal.animalData.animalName));
        };
        GameObject nearestPrey = sence.FindNearest();
        // Debug.Log("nearestPrey : " + nearestPrey);
        if(nearestPrey != null){
            targetObject = nearestPrey;
            ThisTask.Succeed();
        }else{
            targetObject = null;
            ThisTask.Fail();
        }
    }

    [Task]
    public void PickItemUp(){
        if(targetObject == null){
            ThisTask.Fail();
        }
        ItemPickup itemPickup = targetObject.GetComponent<ItemPickup>();
        if(itemPickup == null){
            Debug.Log("Fail to Pick up the Item");
            ThisTask.Fail();
            return;
        }
        Destroy(targetObject);
        animalData.cautionLevel -= 5;
        GameManager.Instance.achievementManager.AddTrial("FeedWolf",1);
        ThisTask.Succeed();
    }

    public void Jump(){
        if(!jumpCapable){
            return;
        }
        Vector3 movement = -1*(this.transform.position-targetVector3).normalized;
        this.animalRigidbody.AddForce(new Vector3(movement.x,1,movement.z)*jumpPower,ForceMode.Impulse);
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
        
        if(animalData.cautionLevel == 0){
            // animator.SetTrigger("Notice");
            animalData.cautionLevel = 1;
            this.jumpCapable = false;
            Debug.Log("Notice!");

            aIPath.maxSpeed = 5;
            animator.SetFloat("Speed", 5 );
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
