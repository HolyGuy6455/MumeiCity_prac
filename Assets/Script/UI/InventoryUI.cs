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
            itemSlots[i].data = itemSlotData[i];
            itemSlots[i].UpdateUI();
        }
    }

    private void OnFocusSlot(ItemSlot inventorySlot, PointerEventData eventData){
        if(inventorySlot.data.code == 0){
            return;
        }
        ItemInfoImage.sprite = inventorySlot.data.itemPreset.itemSprite;
        ItemInfoName.text = inventorySlot.data.itemPreset.name;
        ItemInfoText.text = inventorySlot.data.itemPreset.info;

        itemTags[0].UpdateUI(inventorySlot.data.itemPreset.tags[0]);
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
