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
    public AnimalData animalData;
    static int groundLayerMask;

    /*
    경계상태 구분

    */

    public virtual void Start() {
        groundLayerMask = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Building"));
        // aIPath.destination = new Vector3(0,0,0);
        hittable.HP = hpMax;
    }
    public virtual void Update() {
        animator.SetBool("isGrounded",IsGrounded());
        // targetVector3 = targetObject.transform.position;
        // aIPath.
    }

    public bool IsGrounded() {
        return Physics.Raycast(this.transform.position, Vector3.down, 1.0f, groundLayerMask);
    }

    void UpdatePath(){
        // if(seeker.IsDone()){
        //     seeker.StartPath(this.transform.position, target.position, OnPathComplete);
        // }
    }

    // 멍때리기
    [Task]
    public virtual void Idle(){
        // do nothing;
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
        if(targetVector3 == null){
            ThisTask.Succeed();
            return;
        }
        float distanceValue = Vector3.Distance(this.transform.position,targetVector3);
        ThisTask.Complete( distanceValue < distance );
    }
    // 경계상태인가
    [Task]
    public virtual void BeCautionedMoreThan(int cautionLevel){
        ThisTask.Complete( this.animalData.cautionLevel > cautionLevel );
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

    public virtual void Hear(string soundSource){
        // do nothing
    }
    
}
