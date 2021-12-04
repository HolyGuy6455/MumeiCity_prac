using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollision : MonoBehaviour
{
    public int test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag != "HitBox"){
            return;
        }
        Debug.Log("TriggerEnter "+other.gameObject);
        other.gameObject.GetComponentInParent<Hittable>().Hit();
    }

}
