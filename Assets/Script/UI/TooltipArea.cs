using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipArea    : MonoBehaviour
                            , IPointerEnterHandler
                            , IPointerExitHandler{
    [SerializeField] string contents;
    public void OnPointerEnter(PointerEventData eventData){
        MousePointer.instance.ShowTooltip(contents);
    }
    
    public void OnPointerExit(PointerEventData eventData){
        MousePointer.instance.HideTooltip();
    }
}
