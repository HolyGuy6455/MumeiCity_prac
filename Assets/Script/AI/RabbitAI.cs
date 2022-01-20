using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour, IHeraingEar
{
    [SerializeField] Transform playerTransform;
    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] Animator animator;
    public float noticeDistance = 3.0f;
    public float jumpPower = 3.0f;
    bool isNoticed = false;
    int groundLayerMask;

    void Start(){
        groundLayerMask = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Building"));
        
        Hittable hittable = this.GetComponent<Hittable>();
        List<EffectiveTool> effectiveTools = new List<EffectiveTool>();
        effectiveTools.Add(new EffectiveTool(Tool.ToolType.KNIFE, 3));
        hittable.SetEffectiveTool(effectiveTools);
    }

    void Update(){
        animator.SetBool("isGrounded",IsGrounded());
    }

    public void Jump(){
        Vector3 movement = (this.transform.position-playerTransform.position).normalized;
        this.rigidbody.AddForce(new Vector3(movement.x,1,movement.z)*jumpPower,ForceMode.Impulse);
        if(movement.x <= -0.01f){
            transform.localScale = new Vector3(1f,1f,1f);
        }else if(movement.x >= 0.01f){
            transform.localScale = new Vector3(-1f,1f,1f);
        }
    }

    public bool IsGrounded() {
        return Physics.Raycast(this.transform.position, Vector3.down, 1.0f, groundLayerMask);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, noticeDistance);
    }
    
    public void Hear(string soundSource){
        if(!isNoticed){
            animator.SetTrigger("Notice");
            animator.SetBool("isNoticed",true);
            isNoticed = true;
        }
    }
}
