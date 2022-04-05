using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Panda;

public class AnimalBehavior : MonoBehaviour, IHeraingEar{
    [SerializeField] protected AIPath aIPath;
    [SerializeField] protected Seeker seeker;
    [SerializeField] protected GameObject targetObject;
    [SerializeField] protected Vector3 targetVector3;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Sence sence;
    [SerializeField] protected Hittable hittable;
    [SerializeField] protected int hpMax;
    [SerializeField] protected Rigidbody animalRigidbody;
    [SerializeField] protected float distanceToTarget;
    public List<EffectiveTool> removalTool; //temperal
    public AnimalData animalData = new AnimalData();
    static int groundLayerMask;
    public float walkAroundRangeMin = 3;
    public float walkAroundRangeMax = 5;
    bool walkAround = false;

    /*
    경계상태 구분

    */

    public virtual void Start() {
        groundLayerMask = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Building"));
        hittable.HP = hpMax;
    }
    public virtual void Update() {
        animator.SetBool("isGrounded",IsGrounded());
        // targetVector3 = targetObject.transform.position;
        // aIPath.
    }

    public bool IsGrounded() {
        return Physics.Raycast(this.transform.position, Vector3.down, 0.5f, groundLayerMask);
    }

    void UpdatePath(){
        // if(seeker.IsDone()){
        //     seeker.StartPath(this.transform.position, target.position, OnPathComplete);
        // }
    }

    [Task]
    public virtual void RandomMove(){
        while(walkAround == false){
            targetVector3 = new Vector3();
            targetVector3.x = this.transform.position.x 
                                + Random.Range(walkAroundRangeMin, walkAroundRangeMax) * ((Random.Range(0,2)>0)?1:-1);
            targetVector3.y = this.transform.position.y;
            targetVector3.z = this.transform.position.z 
                                + Random.Range(walkAroundRangeMin, walkAroundRangeMax) * ((Random.Range(0,2)>0)?1:-1);
            if(!GameManager.Instance.gridMapManager.amIInWater(targetVector3)){
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

    // 멍때리기
    [Task]
    public virtual void Idle(){
        targetVector3 = this.transform.position;
        distanceToTarget = 0 ;
        walkAround = false;
        ThisTask.Succeed();
    }
    // 길찾기 없이 도망치기
    [Task]
    public virtual void PanicRunaway(){
    }
    // 길찾기로 도망치기
    [Task]
    public virtual void Runaway(){
    }
    // 목표에 접근하기
    [Task]
    public virtual void GoCloser(){
    }
    // 공격하기
    [Task]
    public virtual void Attack(){
    }
    // 체력이 이것보다 낮은지
    [Task]
    public virtual void HPPerLessThan(float value){
        float hpPercent = ( (float) hittable.HP ) / ( (float) hpMax );
        ThisTask.Complete( hpPercent < value );
    }
    // 목표와의 거리가 이거보다 가까운가
    [Task]
    public virtual void IsTheTargetCloserThan(float distance){
        if(targetObject != null){
            targetVector3 = targetObject.transform.position;
        }
        if(targetVector3 == null){
            ThisTask.Succeed();
            return;
        }
        float distanceValue = Vector3.Distance(this.transform.position,targetVector3);
        // Debug.Log("distanceValue : "+distanceValue);
        ThisTask.Complete( distanceValue < distance );
    }
    // 목표와의 거리가 이거보다 먼가
    [Task]
    public virtual void IsTheTargetFartherThan(float distance){
        if(targetObject != null){
            targetVector3 = targetObject.transform.position;
        }
        if(targetVector3 == null){
            ThisTask.Succeed();
            return;
        }
        float distanceValue = Vector3.Distance(this.transform.position,targetVector3);
        ThisTask.Complete( distanceValue > distance );
    }
    // 경계상태인가
    [Task]
    public virtual void BeCautionedGraterThan(int cautionLevel){
        ThisTask.Complete( this.animalData.cautionLevel > cautionLevel );
    }
    [Task]
    public virtual void CalmDown(){
        if(this.animalData.cautionLevel > 0){
            this.animalData.cautionLevel -= 1;
        }
    } 
    // 길들여진 상태인가
    [Task]
    public virtual void BeTamed(){
        ThisTask.Fail();
    }
    // 아이템 줍기
    [Task]
    public virtual void CollectItem(string itemTag){
    }
    // 플레이어를 타겟으로 삼기
    [Task]
    public virtual void SetTarget(string targetName){
        switch (targetName)
        {
            case "Player":
                targetObject = GameManager.Instance.PlayerTransform.gameObject;
                break;
            default:
                break;
        }
        ThisTask.Succeed();
    }

    [Task]
    public virtual void SetCautionLevel(int cautionLevel){
        this.animalData.cautionLevel = cautionLevel;
        ThisTask.Succeed();
    }

    [Task]
    public virtual void SetSpeed(float speed){
        this.aIPath.maxSpeed = speed;
        ThisTask.Succeed();
    }

    public virtual void Hear(string soundSource){
        // do nothing
    }
    
}
