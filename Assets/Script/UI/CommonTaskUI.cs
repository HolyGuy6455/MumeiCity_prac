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
        ItemSlotData[] itemSlotData = GameManager.Instance.interactingBuilding.items;
        for (int i = 0; i < itemSlotData.Length; i++){
            StorageSlots[i].data = itemSlotData[i];
            StorageSlots[i].UpdateUI();
        }
    }

    void OnFocusSlot(ItemSlot inventorySlot, PointerEventData eventData){
        if(inventorySlot.data.code == 0){
            return;
        }
    }

    void OnClickSlot(ItemSlot inventorySlot, PointerEventData eventData){
        GameManager.Instance.inventory.ClickLeft(inventorySlot);
        inventorySlot.UpdateUI();
    }
}
