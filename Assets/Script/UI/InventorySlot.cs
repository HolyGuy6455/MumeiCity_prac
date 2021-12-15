using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public delegate void InventorySlotEvent(InventorySlot inventorySlot, PointerEventData eventData);
    public InventorySlotEvent onFocusCallback;
    public InventorySlotEvent onClickCallback;
    public Image icon;
    [UnityEngine.Serialization.FormerlySerializedAs("amount")] public Text amountText;
    ItemSlotData _data;
    public ItemSlotData data{
        get{
            return _data;
        }
        set{
            _data._updateUI -= UpdateUI;
            _data = value;
            _data._updateUI += UpdateUI;
        }
    }

    public void UpdateUI(){
        if(data == null || data.code == 0){
            icon.sprite = null;
            icon.enabled = false;
            amountText.text = "";
        }else{
            icon.sprite = ItemManager.GetItemPresetFromCode(data.code).itemSprite;
            icon.enabled = true;
            amountText.text = data.amount.ToString();
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
