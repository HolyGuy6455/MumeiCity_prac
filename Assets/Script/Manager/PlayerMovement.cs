using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float slowSpeed = 1.0f;
    [SerializeField] float jumpPower = 7.0f;
    [SerializeField] float jumpRange = 3.0f;
    [SerializeField] SteminaGauge steminaGauge;
    [SerializeField] TimeEventQueueTicket steminaRechargeEvent;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Animator animator;
    [SerializeField] bool isJump = false;
    [SerializeField] bool jumpCapable = true;
    [SerializeField] bool reflectCapable = true;
    [SerializeField] GameObject spriteObject;
    [SerializeField] SoundCollider playerSoundCollider;
    public Vector3 movement;
    public Transform shadow;
    [SerializeField] Animator shadowAnimator;
    [SerializeField] bool amIInWater;
    [SerializeField] bool amIOnABoat;
    [SerializeField] float playerHeight;
    RaycastHit hit;
    bool isRaycastHit;
    int groundLayerMask;
    public bool stop;
    [SerializeField] float ITEM_DROP_RANGE = 10.0f;
    [SerializeField] float ITEM_DROP_JUMP = 10.0f;
    Dictionary<ToolType,int> toolActionDictionary;
    [SerializeField] FMODUnity.StudioEventEmitter footstepSound;

    [SerializeField] Vector3 lastStandLand;
    [SerializeField] FishingBait fishingBait;

    // [SerializeField] Sence sence;
    public bool _amIOnABoat{
        get{
            return amIOnABoat;
        }
        set{
            amIOnABoat = value;
            animator.SetBool("Boat",amIOnABoat);
        }
    }
    public bool _amIInWater{
        get{
            return amIInWater;
        }
    }

    void Awake(){
        groundLayerMask = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Building"));
        toolActionDictionary = new Dictionary<ToolType, int>();
        toolActionDictionary[ToolType.AXE] = 1;
        toolActionDictionary[ToolType.KNIFE] = 2;
        toolActionDictionary[ToolType.FRYINGPAN] = 3;
        toolActionDictionary[ToolType.PICKAXE] = 4;
        toolActionDictionary[ToolType.SHOVEL] = 5;
        toolActionDictionary[ToolType.HAMMER] = 6;
        toolActionDictionary[ToolType.FISHINGROD] = 7;

        string ticketName = "Player_Stemina_Recharge";
        steminaRechargeEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1, ticketName, SteminaRecharge);
    }

    void Update(){
        if(stop){
            movement.x = 0;
            movement.z = 0;
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("IsJumping",!IsGrounded());
        animator.SetBool("Drown",amIInWater);

        if(reflectCapable){
            if(movement.x <= -0.01f){
                spriteObject.transform.localScale = new Vector3(-1f,1f,1f);
            }else if(movement.x >= 0.01f){
                spriteObject.transform.localScale = new Vector3(1f,1f,1f);
            }
        }

        steminaGauge.MoveUIPositionFromTransform();
    }

    public void OnMovement(InputAction.CallbackContext value){
        Vector2 inputMovement = value.ReadValue<Vector2>();
        movement.x = inputMovement.x;
        movement.z = inputMovement.y;

        fishingBait.SetLastMovement(movement.x,movement.z);

        if(movement.z <= -0.01f){
            animator.SetFloat("Backward",1);
        }else if(movement.z >= 0.01f){
            animator.SetFloat("Backward",-1);
        }
    }

    public void OnJump(InputAction.CallbackContext value){
        if(value.started){
            if(GameManager.Instance.presentGameTab == GameManager.GameTab.BUILDING){
                GameManager.Instance.buildingManager.BuildOnProcess();
            }else{
                OnJump();
            }
        }
    }

    public void OnJump(){
        if(steminaGauge.value < steminaGauge.consumption)
            return;
        if(amIOnABoat)
            return;
        if(jumpCapable && IsGrounded()){
            isJump = true;
        }
    }


    public void OnAttack(InputAction.CallbackContext value){
        if(value.started && !GameManager.Instance.mouseOnUI){
            ToolInfo toolNowHold = GameManager.Instance.GetToolInfoNowHold();
            if(!toolNowHold.isItEnable()){
                animator.SetInteger("ActCode",0);
            }else if(toolActionDictionary.ContainsKey(toolNowHold.toolType)){
                animator.SetInteger("ActCode",toolActionDictionary[toolNowHold.toolType]);
                if(toolActionDictionary[toolNowHold.toolType] == 7){
                    animator.SetTrigger("FishingTrigger");
                }
            }
        }else if(value.canceled){
            animator.SetInteger("ActCode",0);
        }
    }

    public void OnCrouch(InputAction.CallbackContext value){
        if(value.started){
            animator.SetFloat("Crouch", -1);
        }else if (value.canceled){
            animator.SetFloat("Crouch", 1);
        }
    }

    public void OnCrouch(bool value){
        animator.SetFloat("Crouch", (value)?-1:1);
    }

    public bool IsGrounded() {
        return Physics.Raycast(this.transform.position, Vector3.down, 1.2f, groundLayerMask);
    }

    public void Hurt(){
        animator.SetTrigger("Hurt");
        List<ItemSlotData> inventoryItems = GameManager.Instance.inventory.GetDataList();
        for (int i = 0; i < 3; i++){
            if(Random.Range(0.0f,1.0f)>0.8f){
                // 20확률로 아이템을 잃어버리지 않아요
                continue;
            }
            if(inventoryItems.Count == 0){
                break;
            }
            ItemSlotData selectedSlot = inventoryItems[Random.Range(0,inventoryItems.Count)];
            string dropItemName = selectedSlot.itemName;

            if(selectedSlot.itemData.isNone()){
                continue;
            }
            if(selectedSlot.amount == 1){
                selectedSlot.amount = 0;
                selectedSlot.itemName = "None";
            }else{
                selectedSlot.amount -= 1;
            }

            Vector3 popForce = new Vector3();
            popForce.x = Random.Range(-ITEM_DROP_RANGE, ITEM_DROP_RANGE);
            popForce.y = ITEM_DROP_JUMP;
            popForce.z = Random.Range(-ITEM_DROP_RANGE, ITEM_DROP_RANGE);
            Vector3 location = this.transform.position;

            GameObject itemObject = Instantiate(GameManager.Instance.itemManager.itemPickupPrefab,location,Quaternion.identity);
            ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
            itemPickup.itemPickupData = ItemPickupData.create(ItemData.Instant(dropItemName));
            itemPickup.itemPickupData.leftSecond = 12;
            itemPickup.IconSpriteUpdate();
            itemPickup.GetComponent<Rigidbody>().AddForce(popForce,ForceMode.Impulse);
            itemObject.transform.SetParent(GameManager.Instance.itemManager.itemPickupParent.transform);
        }
    }

    void FixedUpdate() {
        amIInWater = GameManager.Instance.gridMapManager.amIInWater(rigidBody.transform.position);
        playerHeight = GameManager.Instance.gridMapManager.amIInCave(rigidBody.transform.position);
        if(!amIInWater){
            lastStandLand = this.transform.position;
            this._amIOnABoat = false;
        }

        isRaycastHit = Physics.Raycast(rigidBody.position, new Vector3(0,-1, 0), out hit, 20, groundLayerMask);
        shadow.position = hit.point;
        if(amIInWater && rigidBody.position.y < 0){
            // shadow.position = new Vector3(shadow.position.x,0,shadow.position.z);
            shadowAnimator.SetBool("amIInWater",true);
            footstepSound.SetParameter("OnGround",1);
        }else{
            shadowAnimator.SetBool("amIInWater",false);
            footstepSound.SetParameter("OnGround",0);
        }
        footstepSound.SetParameter("Cave",1-playerHeight);

        bool HorizontalRayCast = Physics.Raycast(this.transform.position, new Vector3(movement.x,0,0), 0.5f, groundLayerMask);
        bool VerticalRayCast = Physics.Raycast(this.transform.position, new Vector3(0,0,movement.z), 0.5f, groundLayerMask);
        Vector3 movementTemp = movement * 1;
        if(HorizontalRayCast)
            movementTemp.x = 0;
        if(VerticalRayCast)
            movementTemp.z = 0;
        rigidBody.MovePosition(rigidBody.position + movementTemp * moveSpeed / slowSpeed * Time.fixedDeltaTime);
        if(isJump){
            rigidBody.AddForce(new Vector3(movement.x * jumpRange,1.0f * jumpPower,movement.z * jumpRange),ForceMode.Impulse);
            isJump = false;
            footstepSound.SetParameter("isJump",1);
            footstepSound.SetParameter("Movement",1);
            steminaGauge.value -= steminaGauge.consumption;
        }else if(rigidBody.velocity.y <= 0 && IsGrounded()){
            float movementSpeed = movementTemp.magnitude / slowSpeed;
            footstepSound.SetParameter("isJump",0);
            footstepSound.SetParameter("Movement",movementSpeed);
        }
    }

    public bool SteminaRecharge(string ticketName){
        steminaGauge.value += steminaGauge.recharge;
        if(amIInWater && !amIOnABoat){
            steminaGauge.value -= steminaGauge.exhaustionInDrown;
        }
        if(!steminaGauge.Update()){
            animator.SetTrigger("Hurt");
            this.transform.position = lastStandLand;
        }
        return false;
    }

    void SlowDown(float slowSpeed){
        if(!IsGrounded()){
            return;
        }
        this.slowSpeed = slowSpeed;
    }

    void EnableJump(){
        this.jumpCapable = true;
    }
    void DisableJump(){
        this.jumpCapable = false;
    }
    void EnableReflect(){
        this.reflectCapable = true;
    }
    void DisableReflect(){
        this.reflectCapable = false;
    }
    void LookAtMonitor(){
        animator.SetFloat("Backward",1);
    }

    public void ThrowBait(){
        fishingBait.ThrowBait();
    }

    public void TakeBackBait(){
        fishingBait.TakeBackBait();
    }

    public void RemoveBait(){
        fishingBait.RemoveBait();
    }

    public void IntenseFishing(){
        fishingBait.Intense();
    }
    
    public void LockLastMovement(string value){
        fishingBait.LockLastMovement((value.CompareTo("True") == 0 ) ? true : false);
    }

}
