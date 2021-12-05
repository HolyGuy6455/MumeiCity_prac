using UnityEngine;

public class Interactable : MonoBehaviour
{

    // public float radius = 3f;
    public bool isInteractable = false;
    // public Transform player;

    // private void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, radius);
    // }

    // Start is called before the first frame update
    void Start()
    {
        // player = GameManager.Instance.PlayerTransform;
    }

    // Update is called once per frame
    void Update()
    {
        // float distance = Vector2.Distance(player.position, transform.position);
        // if(!isInteractable && (distance < radius) ){
        //     Debug.Log(gameObject.name + "Interactable");
        //     isInteractable = true;
        //     GameManager.Instance.AddInteractable(this);
        // }else if(isInteractable && (distance > radius)){
        //     Debug.Log(gameObject.name + "no more Interactable");
        //     isInteractable = false;
        //     GameManager.Instance.RemoveInteractable(this);
        // }

        // if(player == null){
        //     player = GameManager.Instance.PlayerTransform;
        // }
    }

    private void OnDestroy() {
        GameManager.Instance.RemoveInteractable(this);
    }

    public virtual void Interact(){

    }
}
