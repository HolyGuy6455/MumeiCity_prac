using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    public float jumpPower = 7.0f;
    public Rigidbody rigidBody;
    public Animator animator;
    public Vector3 movement;
    public Transform shadow;
    private bool isJump = false;
    RaycastHit hit;
    bool isRaycastHit;
    int groundLayerMask;
    public bool stop;
    [SerializeField] GameObject spriteObject;

    // [SerializeField] Sence sence;

    void Start()
    {
        groundLayerMask = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Building"));
    }

    void Update()
    {
        if(stop){
            movement.x = 0;
            movement.z = 0;

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.z);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetBool("IsJumping",!IsGrounded());
            return;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if(Input.GetButtonDown("Jump")/* && IsGrounded()*/){
            isJump = true;
        }

        if(Input.GetButtonDown("Fire2")){
            Interactable interactable = GameManager.Instance.nearestInteractable;
            if(interactable != null){
                interactable.Interact();
            }
        }

        if(Input.GetButton("Fire1")){
            Tool toolNowHold = GameManager.Instance.GetToolNowHold();
            switch (toolNowHold.name){
                case "Axe":
                    animator.SetInteger("ActCode",1);
                    break;
                case "Knife":
                    animator.SetInteger("ActCode",2);
                    break;
                case "FryingPan":
                    animator.SetInteger("ActCode",3);
                    break;
                case "Pickaxe":
                    animator.SetInteger("ActCode",4);
                    break;
                case "Shovel":
                    animator.SetInteger("ActCode",5);
                    break;
                default:
                    animator.SetInteger("ActCode",0);
                    break;
            }
        }else if(Input.GetButtonUp("Fire1")){
            animator.SetInteger("ActCode",0);
        }

        animator.SetBool("IsJumping",!IsGrounded());

        // Debug.Log("IsGrounded : " + IsGrounded());
        // rigidBody.useGravity = !IsGrounded();

        if(movement.x <= -0.01f){
            spriteObject.transform.localScale = new Vector3(-1f,1f,1f);
        }else if(movement.x >= 0.01f){
            spriteObject.transform.localScale = new Vector3(1f,1f,1f);
        }

        if(movement.z <= -0.01f){
            animator.SetFloat("Backward",0);
        }else if(movement.z >= 0.01f){
            animator.SetFloat("Backward",1);
        }
    }

    public bool IsGrounded() {
        return Physics.Raycast(this.transform.position, Vector3.down, 1.0f, groundLayerMask);
    }

    // void TakeDamage(int damage){
    //     health -= damage;
    //     healthBar.SetHealth(health);
    // }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        // Gizmos.DrawLine(rigidBody.position,rigidBody.position + movement);
        // if(isRaycastHit)
        //     Gizmos.DrawWireSphere(hit.point, 0.5f);
        
    }

    void FixedUpdate() {
        isRaycastHit = Physics.Raycast(rigidBody.position, new Vector3(0,-1, 0), out hit, 20, groundLayerMask);
        shadow.position = hit.point + new Vector3(0,0,-0.2f);

        bool HorizontalRayCast = Physics.Raycast(rigidBody.position + new Vector3(0,0.5f,0), new Vector3(movement.x,0,0), 0.5f, groundLayerMask);
        bool VerticalRayCast = Physics.Raycast(rigidBody.position + new Vector3(0,0.5f,0), new Vector3(0,0,movement.z), 0.5f, groundLayerMask);
        if(HorizontalRayCast)
            movement.x = 0;
        if(VerticalRayCast)
            movement.z = 0;
        rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
        if(isJump){
            rigidBody.AddForce(new Vector3(movement.x/3,1,movement.z/3)*jumpPower,ForceMode.Impulse);
            isJump = false;
        }
        // this.transform.position = Vector3.Lerp(this.transform.position,Input.mousePosition,Time.deltaTime * 10);
    }
}
