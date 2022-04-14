using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrint : MonoBehaviour
{
    [SerializeField]
    List<Transform> transforms = new List<Transform>();
    [SerializeField]
    PlayerMovement player;
    Vector3 lastLocation;
    int index = 0;
    public float footPrintDistance = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        lastLocation = player.shadow.position;
    }

    private void FixedUpdate() {
        float distance = Vector3.Distance(lastLocation,player.shadow.position);
        bool inWater = player._amIInWater;
        if(distance > footPrintDistance && player.IsGrounded()){
            if(++index>transforms.Count-1){
                index = 0;
            }
            Vector3 playerMovement =  player.movement;
            transforms[index].position = player.shadow.position;
            if(inWater){
                transforms[index].rotation = new Quaternion();
            }else{
                transforms[index].LookAt(player.shadow.position+playerMovement);
                transforms[index].Rotate(new Vector3(90,0,0));
            }
            Animator animator = transforms[index].GetComponent<Animator>();
            animator.SetBool("InWater",inWater);
            animator.SetTrigger("Reset");
            lastLocation = player.shadow.position;
        }
    }
}
