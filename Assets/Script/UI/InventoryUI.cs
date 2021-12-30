using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour{
    [SerializeField] ItemSlot[] slots;
    [SerializeField] Image ItemInfoImage;
    [SerializeField] Text ItemInfoName;
    [SerializeField] Text ItemInfoText;

    void Start(){
        slots = this.GetComponentsInChildren<ItemSlot>();
        foreach (ItemSlot slot in slots){
            slot.onFocusCallback += OnFocusSlot;
            slot.onClickCallback += OnClickSlot;
        }
    }

    public void LoadItemSlotData(){
        ItemSlotData[] itemSlotData = GameManager.Instance.inventory.itemData;
        for (int i = 0; i < slots.Length; i++){
            slots[i].data = itemSlotData[i];
            slots[i].UpdateUI();
        }
    }

    private void OnFocusSlot(ItemSlot inventorySlot, PointerEventData eventData){
        if(inventorySlot.data.code == 0){
            return;
        }
        ItemInfoImage.sprite = inventorySlot.data.itemPreset.itemSprite;
        ItemInfoName.text = inventorySlot.data.itemPreset.name;
        ItemInfoText.text = inventorySlot.data.itemPreset.info;
    }

    private void OnClickSlot(ItemSlot inventorySlot, PointerEventData eventData){
        GameManager.Instance.inventory.ClickLeft(inventorySlot);
        inventorySlot.UpdateUI();
    }
}
