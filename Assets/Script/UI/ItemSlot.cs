using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, 
                        IPointerEnterHandler, 
                        IPointerClickHandler{
    public delegate void InventorySlotEvent(ItemSlot inventorySlot, PointerEventData eventData);
    public InventorySlotEvent onFocusCallback;
    public InventorySlotEvent onClickCallback;
    public Image icon;
    [UnityEngine.Serialization.FormerlySerializedAs("amount")] public Text amountText;
    public ItemSlotData itemSlotData;

    public void UpdateUI(){
        if(itemSlotData == null || itemSlotData.itemData.isNone() || itemSlotData.amount == 0){
            icon.sprite = null;
            icon.enabled = false;
            amountText.text = "";
            itemSlotData.itemName = "None";
        }else{
            icon.sprite = ItemData.Instant(itemSlotData.itemName).itemSprite;
            icon.enabled = true;
            amountText.text = itemSlotData.amount.ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(onFocusCallback != null)
            onFocusCallback.Invoke(this, eventData);
    }

    public void OnPointerClick(PointerEventData eventData){
        if(onClickCallback != null)
            onClickCallback.Invoke(this, eventData);
    }
}
