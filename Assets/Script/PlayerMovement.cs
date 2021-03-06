using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    public float jumpPower = 7.0f;
    public Rigidbody rigidBody;
    public Animator animator;
    Vector3 movement;
    public Transform shadow;
    private bool isJump = false;
    RaycastHit hit;
    bool isRaycastHit;
    int groundLayerMask;

    void Start()
    {
        groundLayerMask = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Building"));
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if(Input.GetButtonDown("Jump")/* && IsGrounded()*/){
            isJump = true;
        }

        if(Input.GetButtonDown("Fire2")){
            Interactable interactable = GameManager.Instance.GetFirstInteractable();
            if(interactable != null){
                Debug.Log(interactable);
                interactable.Interact();
            }
        }

        if(Input.GetKeyDown(KeyCode.C)){
            GameManager.Instance.buildingManager.Build();
        }

        if(GameManager.Instance.GetToolNowHold().name == "Axe"){
            if(Input.GetButtonDown("Fire1")){
                animator.SetBool("Choping",true);
            }else if(Input.GetButtonUp("Fire1")){
                animator.SetBool("Choping",false);
            }
        }

        // Debug.Log("IsGrounded : " + IsGrounded());
        // rigidBody.useGravity = !IsGrounded();

        if(movement.x <= -0.01f){
            transform.localScale = new Vector3(-1f,1f,1f);
        }else if(movement.x >= 0.01f){
            transform.localScale = new Vector3(1f,1f,1f);
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
        shadow.position = hit.point;

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
