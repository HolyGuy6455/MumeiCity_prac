using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour{
    [SerializeField] ItemSlot[] itemSlots;
    [SerializeField] Image ItemInfoImage;
    [SerializeField] Text ItemInfoName;
    [SerializeField] Text ItemInfoText;
    [SerializeField] TagSlot[] itemTags;
    [SerializeField] RectTransform itemHeldInHandTransform;

    void Start(){
        itemSlots = this.GetComponentsInChildren<ItemSlot>();
        foreach (ItemSlot slot in itemSlots){
            slot.onFocusCallback += OnFocusSlot;
            slot.onClickCallback += OnClickSlot;
        }
    }

    public void LoadItemSlotData(){
        ItemSlotData[] itemSlotData = GameManager.Instance.inventory.itemData;
        for (int i = 0; i < itemSlots.Length; i++){
            itemSlots[i].itemSlotData = itemSlotData[i];
            itemSlots[i].UpdateUI();
        }
    }

    private void OnFocusSlot(ItemSlot inventorySlot, PointerEventData eventData){
        if(inventorySlot.itemSlotData.itemData.isNone()){
            return;
        }
        ItemInfoImage.sprite = inventorySlot.itemSlotData.itemData.itemSprite;
        ItemInfoName.text = inventorySlot.itemSlotData.itemData.itemName;
        ItemInfoText.text = inventorySlot.itemSlotData.itemData.info;

        itemTags[0].UpdateUI(inventorySlot.itemSlotData.itemData.tag);
    }

    private void OnClickSlot(ItemSlot inventorySlot, PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            GameManager.Instance.inventory.ClickLeft(inventorySlot);
        }
        inventorySlot.UpdateUI();
    }

    public void OnMouseMove(InputAction.CallbackContext value){
        Vector2 mousePosition = value.ReadValue<Vector2>();
        itemHeldInHandTransform.transform.position = mousePosition;
    }

}
