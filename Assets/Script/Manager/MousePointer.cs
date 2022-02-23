using UnityEngine;
using UnityEngine.InputSystem;

public class MousePointer : MonoBehaviour {
    [SerializeField] RectTransform mousePointerTransform;
    private void Awake() {
        // Cursor.visible = false;
    }

    public void OnMouseMove(InputAction.CallbackContext value){
        Vector2 mousePosition = value.ReadValue<Vector2>();
        mousePointerTransform.transform.position = mousePosition;
    }
}