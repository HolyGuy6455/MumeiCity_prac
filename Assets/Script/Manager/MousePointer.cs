using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MousePointer : MonoBehaviour {
    [SerializeField] RectTransform mousePointerTransform;
    [SerializeField] Text tooltipText;
    [SerializeField] RectTransform tooltipTransform;
    private static MousePointer myself;
    public static MousePointer instance{get{return myself;}}
    private void Awake() {
        Cursor.visible = false;
        myself = this;
    }

    public void OnMouseMove(InputAction.CallbackContext value){
        Vector2 mousePosition = value.ReadValue<Vector2>();
        mousePointerTransform.transform.position = mousePosition;
    }

    public void ShowTooltip(string value){
        tooltipTransform.gameObject.SetActive(true);
        tooltipText.text = value;
        float width = tooltipText.preferredWidth;
        float height = tooltipText.preferredHeight;
        tooltipTransform.sizeDelta = new Vector2(width+10,height+10);
    }

    public void HideTooltip(){
        tooltipTransform.gameObject.SetActive(false);
    }
}