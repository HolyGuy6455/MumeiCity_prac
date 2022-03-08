using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipArea    : MonoBehaviour
                            , IPointerEnterHandler
                            , IPointerExitHandler{
    [SerializeField] string contents;
    public void OnPointerEnter(PointerEventData eventData){
        MousePointer.instance.ShowTooltip(contents);
        GameManager.Instance.mouseOnUI = true;
    }
    
    public void OnPointerExit(PointerEventData eventData){
        MousePointer.instance.HideTooltip();
        GameManager.Instance.mouseOnUI = false;
    }
}
