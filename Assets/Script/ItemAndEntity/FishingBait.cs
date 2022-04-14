using System.Collections.Generic;
using UnityEngine;

public class FishingBait : MonoBehaviour{
    [SerializeField] GameObject edge;
    [SerializeField] Rigidbody baitRigidbody;
    [SerializeField] LineRenderer fishingBaitLine;
    [SerializeField] Vector3 lastMovement;
    [SerializeField] bool lastMovementLock;
    [SerializeField] float ThrowPower = 11.0f;
    [SerializeField] Animator playerAnimator;

    private void Update() {
        fishingBaitLine.SetPosition(0,edge.transform.position);
        fishingBaitLine.SetPosition(1,this.transform.position);
    }

    
    public void ThrowBait(){
        Debug.Log("ThrowBait");
        playerAnimator.SetBool("Fishing",true);
        this.gameObject.SetActive(true);
        this.transform.position = edge.transform.position;
        lastMovement.y = 0;
        lastMovement = lastMovement.normalized;
        lastMovement.y = 1;
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3();
        rigidbody.AddForce(lastMovement*ThrowPower,ForceMode.Impulse);
    }

    public void TakeBackBait(){
        Vector3 vector3 = edge.transform.position - this.transform.position;
        vector3.y = 0;
        vector3 = vector3.normalized;
        vector3.y = 1;
        vector3 *= ThrowPower;
        this.GetComponent<Rigidbody>().AddForce(vector3,ForceMode.Impulse);
    }

    public void RemoveBait(){
        playerAnimator.SetBool("Fishing",false);
        this.gameObject.SetActive(false);
    }

    public void LockLastMovement(bool value){
        this.lastMovementLock = value;
    }

    public void SetLastMovement(float x, float z){
        if(lastMovementLock){
            return;
        }
        if(x != 0 || z != 0){
            lastMovement.x = x;
            lastMovement.z = z;
        }
    }

}