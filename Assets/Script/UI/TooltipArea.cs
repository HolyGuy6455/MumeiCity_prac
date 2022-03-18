using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipArea    : MonoBehaviour
                            , IPointerEnterHandler
                            , IPointerExitHandler{
    public string content;
    public void OnPointerEnter(PointerEventData eventData){
        MousePointer.instance.ShowTooltip(content);
    }
    
    public void OnPointerExit(PointerEventData eventData){
        MousePointer.instance.HideTooltip();
    }
}
