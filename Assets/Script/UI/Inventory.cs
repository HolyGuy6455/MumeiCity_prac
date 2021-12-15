using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    
    public ItemSlotData itemHeldInHand;
    [SerializeField] Image itemHeldInHandImage;
    ItemSlotData[] slots;
    int itemSpace = 20;

    [SerializeField] List<ItemSlotData> dataView;

    private void Start() {
        itemHeldInHand = null;
        slots = new ItemSlotData[itemSpace];
        for (int i = 0; i < itemSpace; i++){
            slots[i] = ItemSlotData.Create(ItemManager.GetItemPresetFromCode(0));
        }
        // slots = itemsParents.GetComponentsInChildren<InventorySlot>();
        // foreach (InventorySlot slot in slots){
        //     slot.onFocusCallback += OnFocusSlot;
        //     slot.onClickCallback += OnClickSlot;
        //     slot.data = null;
        // }
    }

    private void Update() {
        itemHeldInHandImage.transform.position = Input.mousePosition;
    }

    private void updatedSlot(){
        // dataView = new List<ItemSlotData>();
        // foreach (InventorySlot slot in slots){
        //     if(slot.data != null)
        //         dataView.Add(slot.data);
        // }
    }
    
    public int GetItemAmount(string itemName){
        int result = 0;
        byte findingCode = ItemManager.GetCodeFromItemName(itemName);
        for (int i = 0; i < slots.Length; i++){
            if(slots[i].code == findingCode){
                result += slots[i].amount;
            }
        }
        return result;
    }

    public bool ConsumeItem(string itemName, int amount){
        if(GetItemAmount(itemName) < amount)
            return false;

        byte findingCode = ItemManager.GetCodeFromItemName(itemName);

        for (int i = 0; i < slots.Length; i++){
            if(slots[i] != null && slots[i].code == findingCode){
                if(slots[i].amount > amount){
                    slots[i].amount -= amount;
                    amount = 0;
                }else{
                    amount -= slots[i].amount;
                    slots[i].amount = 0;
                }
                
            }
        }
        updatedSlot();
        return true;
    }
    
    public bool AddItem(ItemSlotData addedItemData){
        bool result = false;

        for (int i = 0; i < slots.Length; i++){
            if(slots[i] != null && slots[i].code == addedItemData.code){
                ItemSlotData added = slots[i];
                added.amount += addedItemData.amount;
                result = true;
                break;
            }
        }

        if(result == false){
            for (int i = 0; i < slots.Length; i++){
                if(slots[i] == null){
                    slots[i] = addedItemData;
                    result = true;
                    break;
                }
            }
        }
        updatedSlot();
        return result;
    }

    // private void OnFocusSlot(InventorySlot inventorySlot, PointerEventData eventData){
        // for (int i = 0; i < slots.Length; i++){
        //     if(slots[i] == inventorySlot){
        //         Debug.Log("slot is "+i);
        //     }
        // }
    // }

    // private void OnClickSlot(InventorySlot inventorySlot, PointerEventData eventData){

        // if(eventData.button == PointerEventData.InputButton.Left){
        //     if(itemHeldInHand != null && inventorySlot.data != null &&itemHeldInHand.code == inventorySlot.data.code){
        //         itemHeldInHand.amount += inventorySlot.data.amount;
        //         inventorySlot.data = null;
        //     }else{
        //         ItemSlotData tempData = itemHeldInHand;
        //         itemHeldInHand = inventorySlot.data;
        //         inventorySlot.data = tempData;
        //     }

        // }else if (eventData.button == PointerEventData.InputButton.Right){
        //     if(itemHeldInHand == null && inventorySlot.data != null){
        //         int amount = inventorySlot.data.amount/2;
        //         Debug.Log("item slot divide! : "+amount);
        //     }else if(itemHeldInHand != null){
        //         if(inventorySlot.data == null){
        //             // inventorySlot.data = itemHeldInHand.CopyEmpty();
        //             inventorySlot.data.amount += 1;
        //             itemHeldInHand.amount -= 1;
        //             if(itemHeldInHand.amount <= 0){
        //                 itemHeldInHand = null;
        //             }
        //         }else if(itemHeldInHand.code == inventorySlot.data.code) {
        //             inventorySlot.data.amount += 1;
        //             itemHeldInHand.amount -= 1;
        //             if(itemHeldInHand.amount <= 0){
        //                 itemHeldInHand = null;
        //             }
        //         }
        //     }
        // }

        // if(itemHeldInHand != null){
        //     itemHeldInHandImage.sprite = itemHeldInHand.itemPreset.itemSprite;
        //     itemHeldInHandImage.gameObject.SetActive(true);
        // }else{
        //     itemHeldInHandImage.sprite = null;
        //     itemHeldInHandImage.gameObject.SetActive(false);
        // }
        // inventorySlot.UpdateUI();
        // updatedSlot();
    // }
}