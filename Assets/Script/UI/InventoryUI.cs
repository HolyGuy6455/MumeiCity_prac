using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour{
    [SerializeField] InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        slots = this.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots){
            slot.onFocusCallback += OnFocusSlot;
            slot.onClickCallback += OnClickSlot;
            slot.data = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFocusSlot(InventorySlot inventorySlot, PointerEventData eventData){

    }

    private void OnClickSlot(InventorySlot inventorySlot, PointerEventData eventData){

    }
}
