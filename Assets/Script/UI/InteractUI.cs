using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] RectTransform UIRectTransform;
    [SerializeField] Camera camera;
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
        // Debug.Log(ViewportPosition);
        UIRectTransform.position = new Vector3(ViewportPosition.x,ViewportPosition.y,0);
        // Vector2 WorldObject_ScreenPosition=new Vector2(
        // ((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
        // ((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));
    }
}
