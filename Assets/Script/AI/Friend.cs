using UnityEngine;
using Pathfinding;

public class Friend : MonoBehaviour{
    [SerializeField] AIDestinationSetter aIDestination; 
    [SerializeField] GameObject targetObject;
    [SerializeField] Animator freindAnimator;

    private void Update() {
        float distance = Vector3.Distance(this.transform.position, targetObject.transform.position);
        Vector3 direction = (targetObject.transform.position - this.transform.position).normalized;
        
        freindAnimator.SetFloat("MoveX",direction.x);
        freindAnimator.SetFloat("MoveY",direction.z);
        freindAnimator.SetFloat("Distance",distance);

        if(distance > 25.0f){
            this.transform.position = Vector3.Lerp(targetObject.transform.position,this.transform.position,0.95f);
        }else if(distance > 3.0f){
            aIDestination.target = targetObject.transform;
        }else{
            aIDestination.target = null;
        }
        Invoke("Spin",30.0f);
    }

    private void Spin(){
        freindAnimator.SetTrigger("Spin");
    }

}