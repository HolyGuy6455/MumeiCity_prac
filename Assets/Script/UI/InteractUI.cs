using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] RectTransform UIRectTransform;
    [SerializeField] Camera camera;
    [SerializeField] Animator animator;
    public bool visible{
        get{
            return animator.GetBool("isVisible");
        }
        set{
            animator.SetBool("isVisible",value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveUIPositionFromTransform(Transform targetTransform){
        Vector2 ViewportPosition=camera.WorldToScreenPoint(targetTransform.position);
        UIRectTransform.position = new Vector3(ViewportPosition.x,ViewportPosition.y,0);
    }
}
