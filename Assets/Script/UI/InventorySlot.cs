using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler
{
    public delegate void OnFocusedEvent(InventorySlot inventorySlot);
    public OnFocusedEvent onFocusedEvent;
    public Image icon;
    [UnityEngine.Serialization.FormerlySerializedAs("amount")] public Text amountText;
    public ItemSlotData data;

    // public ItemSlotData item{
    //     get{
    //         return _item;
    //     }
    //     set{
    //         _item = value;
    //     }
    // }
    
    public void OnPointerEnter(PointerEventData eventData){
        onFocusedEvent.Invoke(this);
    }


    // public void AddItem(ItemSlotData item){
    //     this.item = item;

    //     icon.sprite = GameManager.Instance.itemManager.GetItemPresetFromCode(item.code).icon;
    //     icon.enabled = true;
    //     removeButton.interactable = true;
    //     amountText.text = item.amount.ToString();
    // }

    // public void ClearSlot(){
    //     item = null;

    //     icon.sprite = null;
    //     icon.enabled = false;
    //     removeButton.interactable = false;
    //     amountText.text = "";
    // }



    // public void OnRemoveButton(){
    //     // GameManager.Instance.inventory.RemoveItem(item);
    // }

    // public void UseItem(){
    //     if(item != null){
    //         // item.Use();
    //     }
    // }
}
