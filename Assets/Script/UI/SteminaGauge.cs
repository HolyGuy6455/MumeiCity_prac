using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteminaGauge : MonoBehaviour {
    [SerializeField] RectTransform UIRectTransform;
    [SerializeField] new Camera camera;
    [SerializeField] Animator animator;
    [SerializeField] Slider slider;
    [SerializeField] Image fill;
    [SerializeField] Color colorGreen;
    [SerializeField] Color colorRed;
    [SerializeField] bool visible;
    public float value = 0;
    public float consumption = 50;
    public float recharge = 5;
    public float exhaustionInDrown = 10;
    public float max = 100;
    public void MoveUIPositionFromTransform(){
        Transform targetTransform = GameManager.Instance.PlayerTransform;
        Vector2 ViewportPosition = camera.WorldToScreenPoint(targetTransform.position);
        ViewportPosition.y += 50;
        UIRectTransform.position = new Vector3(ViewportPosition.x,ViewportPosition.y,0);
    }

    public bool Update(){
        bool result = true;
        visible = true;
        if(value >= max){
            value = max;
            visible = false;
        }else if(value < 0){
            value = 0;
            result = false;
        }
        slider.value = value/max;
        animator.SetBool("Visible",visible);
        if(value < consumption){
            fill.color = colorRed;
        }else{
            fill.color = colorGreen;
        }
        return result;
    }

    public void SetSteminaMax(int value){
        this.max = value;
    }
}