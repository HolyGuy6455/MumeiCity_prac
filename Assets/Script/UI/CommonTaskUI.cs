using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommonTaskUI : MonoBehaviour{
    [SerializeField] GameObject StorageSlotParent;
    [SerializeField] ItemSlot[] StorageSlots;
    // Start is called before the first frame update
    protected virtual void Start(){
        foreach (ItemSlot slot in StorageSlots){
            slot.onFocusCallback += OnFocusSlot;
            slot.onClickCallback += OnClickSlot;
        }
    }

    public virtual void UpdateUI(){
        ItemSlotData[] itemSlotData = GameManager.Instance.interactingBuilding.buildingData.items;
        for (int i = 0; i < itemSlotData.Length; i++){
            StorageSlots[i].itemSlotData = itemSlotData[i];
            StorageSlots[i].UpdateUI();
        }
    }

    void OnFocusSlot(ItemSlot inventorySlot, PointerEventData eventData){
        if(inventorySlot.itemSlotData.itemData.isNone()){
            return;
        }
    }

    void OnClickSlot(ItemSlot inventorySlot, PointerEventData eventData){
        GameManager.Instance.inventory.ClickLeft(inventorySlot);
        inventorySlot.UpdateUI();
    }
}
