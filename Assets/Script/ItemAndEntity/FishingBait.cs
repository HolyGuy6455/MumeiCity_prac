using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingBait : MonoBehaviour{
    [SerializeField] GameObject edge;
    [SerializeField] Rigidbody baitRigidbody;
    [SerializeField] LineRenderer fishingBaitLine;
    [SerializeField] Vector3 lastMovement;
    [SerializeField] bool lastMovementLock;
    [SerializeField] float ThrowPower = 11.0f;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator animator;
    [SerializeField] Sence sence;
    [SerializeField] bool intense;
    [SerializeField] new Camera camera;
    [SerializeField] RectTransform lureTransform;
    [SerializeField] Image fill;
    [SerializeField] FishBehavior fishBehavior;
    static List<string> nameList = new List<string>{"Fish"};

    private void Awake() {
        sence.filter = delegate(GameObject value){
            if(value == null)
                return false;

            AnimalBehavior animal = value.GetComponentInParent<AnimalBehavior>();
            if(animal == null)
                return false;

            return (nameList.Contains(animal.animalData.animalName));
        };
        sence.listChangeListener.AddListener(CheckVictim);
    }

    private void Update() {
        if(playerAnimator.GetInteger("Fishing") == 3){
            fishBehavior.transform.position = this.transform.position;
        }else if(intense){
            this.transform.position = fishBehavior.transform.position;
        }

        fishingBaitLine.SetPosition(0,edge.transform.position);
        fishingBaitLine.SetPosition(1,this.transform.position);

        Transform targetTransform = this.transform;
        Vector2 ViewportPosition = camera.WorldToScreenPoint(targetTransform.position);
        ViewportPosition.y += 50;
        lureTransform.position = new Vector3(ViewportPosition.x,ViewportPosition.y,0);
        
    }
    /*
     * Fishing
     * 0 -> 없었다
     * 1 -> 조졌다
     * 2 -> 승부다!
     * 3 -> 해냈다!
     */

    
    public void ThrowBait(){
        Debug.Log("ThrowBait");
        playerAnimator.SetInteger("Fishing",1);
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
        if(fishBehavior != null){
            fishBehavior.SetIntense(false);
            if(playerAnimator.GetInteger("Fishing") == 3){
                fishBehavior.GetComponentInChildren<BoxCollider>().enabled = false;
            }
        }
    }

    public void RemoveBait(){
        intense = false;
        this.gameObject.SetActive(false);
        if(playerAnimator.GetInteger("Fishing") == 3){
            fishBehavior.transform.position = GameManager.Instance.PlayerTransform.position;
            fishBehavior.GetComponent<Hittable>().Dead();
        }
        playerAnimator.SetInteger("Fishing",0);
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

    public void Intense(){
        if(intense){
            return;
        }
        this.animator.SetTrigger("Intense");
        intense = true;
        fishBehavior.SetIntense(true);
    }

    public bool IsThereVictim(){
        GameObject nearest = sence.FindNearest();
        if(nearest == null){
            fishBehavior = null;
            return false;
        }
        fishBehavior = nearest.GetComponentInParent<FishBehavior>();
        return fishBehavior != null;
    }

    private void CheckVictim(){
        if(intense){
            return;
        }
        playerAnimator.SetInteger("Fishing",(IsThereVictim())? 2 : 1);
    }

    public void SetFishingState(int value){
        playerAnimator.SetInteger("Fishing",value);
    }

}