using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag != "Interactable"){
            return;
        }
        // Debug.Log("TriggerEnter "+other.gameObject);
        GameManager.Instance.AddInteractable(other.GetComponent<Interactable>());
        // other.gameObject.GetComponentInParent<Hittable>().Hit();
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag != "Interactable"){
            return;
        }
        GameManager.Instance.RemoveInteractable(other.GetComponent<Interactable>());
    }
}
