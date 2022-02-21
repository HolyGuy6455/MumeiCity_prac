using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float slowSpeed = 1.0f;
    [SerializeField] float jumpPower = 7.0f;
    [SerializeField] float jumpRange = 3.0f;
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
    [SerializeField] bool isInWater;
    RaycastHit hit;
    bool isRaycastHit;
    int groundLayerMask;
    public bool stop;
    [SerializeField] float ITEM_DROP_RANGE = 10.0f;
    [SerializeField] float ITEM_DROP_JUMP = 10.0f;
    Dictionary<string,int> toolActionDictionary;

    // [SerializeField] Sence sence;

    void Start()
    {
        groundLayerMask = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Building"));
        toolActionDictionary = new Dictionary<string, int>();
        toolActionDictionary["Axe"] = 1;
        toolActionDictionary["Knife"] = 2;
        toolActionDictionary["FryingPan"] = 3;
        toolActionDictionary["Pickaxe"] = 4;
        toolActionDictionary["Shovel"] = 5;
        toolActionDictionary["Hammer"] = 6;
    }

    void Update()
    {
        if(stop){
            movement.x = 0;
            movement.z = 0;
        }
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("IsJumping",!IsGrounded());

        if(reflectCapable){
            if(movement.x <= -0.01f){
                spriteObject.transform.localScale = new Vector3(-1f,1f,1f);
            }else if(movement.x >= 0.01f){
                spriteObject.transform.localScale = new Vector3(1f,1f,1f);
            }
        }
        
    }

    public void OnMovement(InputAction.CallbackContext value){
        Vector2 inputMovement = value.ReadValue<Vector2>();
        movement.x = inputMovement.x;
        movement.z = inputMovement.y;

        if(movement.z <= -0.01f){
            animator.SetFloat("Backward",1);
        }else if(movement.z >= 0.01f){
            animator.SetFloat("Backward",-1);
        }
    }

    public void OnJump(InputAction.CallbackContext value){
        if(value.started && jumpCapable && IsGrounded()){
            isJump = true;
        }
    }


    public void OnAttack(InputAction.CallbackContext value){
        if(value.started && !GameManager.Instance.mouseOnUI){
            Tool toolNowHold = GameManager.Instance.GetToolNowHold();
            if(toolActionDictionary.ContainsKey(toolNowHold.name)){
                animator.SetInteger("ActCode",toolActionDictionary[toolNowHold.name]);
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
        isInWater = GameManager.Instance.gridMapManager.amIInWater(rigidBody.transform.position);

        isRaycastHit = Physics.Raycast(rigidBody.position, new Vector3(0,-1, 0), out hit, 20, groundLayerMask);
        shadow.position = hit.point + new Vector3(0,0,-0.2f);
        if(isInWater && rigidBody.position.y < 0){
            shadow.position = new Vector3(shadow.position.x,0,shadow.position.z);
            shadowAnimator.SetBool("amIInWater",true);
        }else{
            shadowAnimator.SetBool("amIInWater",false);
        }
        

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
        }

        // Vector3 lightAndShadowPosition = new Vector3();
        // lightAndShadowPosition.x = -this.transform.position.x;
        // lightAndShadowPosition.y = -this.transform.position.z;
        // GameManager.Instance.buildingManager.lightAndShadow.transform.localPosition = lightAndShadowPosition;
        // this.transform.position = Vector3.Lerp(this.transform.position,Input.mousePosition,Time.deltaTime * 10);
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
}
