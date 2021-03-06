using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public Text amount;
    ItemData item;
    

    public void AddItem(ItemData item){
        this.item = item;

        icon.sprite = item.icon;
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
        GameManager.Instance.inventoryManager.RemoveItem(item);
    }

    public void UseItem(){
        if(item != null){
            item.Use();
        }
    }
}
