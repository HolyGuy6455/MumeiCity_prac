using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public Text amount;
    ItemSlotData item;
    

    public void AddItem(ItemSlotData item){
        this.item = item;

        icon.sprite = GameManager.Instance.itemManager.GetItemPresetFromCode(item.code).icon;
        icon.enabled = true;
        removeButton.interactable = true;
        amount.text = item.amount.ToString();
    }

    public void ClearSlot(){
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        amount.text = "";
    }

    public void OnRemoveButton(){
        // GameManager.Instance.inventory.RemoveItem(item);
    }

    public void UseItem(){
        if(item != null){
            // item.Use();
        }
    }
}
