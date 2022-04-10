using UnityEngine;
using UnityEngine.Events;

public class DockInteract : MonoBehaviour{
    int lastActivatedTime = 0;

    public void BoardABoat(){
        PlayerMovement playerMovement = GameManager.Instance.playerMovement;
        Transform playerTransform =  GameManager.Instance.PlayerTransform;
        if(GameManager.Instance.timeManager._timeValue - lastActivatedTime < 5){
            return;
        }
        lastActivatedTime = GameManager.Instance.timeManager._timeValue;
        
        Vector3 newPosition = new Vector3();
        if(playerMovement._amIInWater && !playerMovement._amIOnABoat){
            newPosition = playerTransform.position;
        }else{
            newPosition = this.transform.position + (this.transform.position - playerTransform.position);
        }
        newPosition.y += 1;
        playerTransform.position = newPosition;
        playerMovement._amIOnABoat = !playerMovement._amIOnABoat;
    }
}
