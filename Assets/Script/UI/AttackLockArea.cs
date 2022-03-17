using UnityEngine;
using UnityEngine.EventSystems;

public class AttackLockArea    : MonoBehaviour
                            , IPointerEnterHandler
                            , IPointerExitHandler{
    public void OnPointerEnter(PointerEventData eventData){
        GameManager.Instance.mouseOnUI = true;
    }
    
    public void OnPointerExit(PointerEventData eventData){
        GameManager.Instance.mouseOnUI = false;
    }
}
