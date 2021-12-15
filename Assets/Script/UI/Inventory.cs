using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    
    public ItemSlotData itemHeldInHand;
    [SerializeField] Image itemHeldInHandImage;
    [SerializeField] Text itemHeldInHandAmount;
    public ItemSlotData[] itemData;
    int itemSpace = 20;

    [SerializeField] List<ItemSlotData> dataView;

    private void Start() {
        itemHeldInHand = ItemSlotData.Create(ItemManager.GetItemPresetFromCode(0));
        itemData = new ItemSlotData[itemSpace];
        for (int i = 0; i < itemSpace; i++){
            itemData[i] = ItemSlotData.Create(ItemManager.GetItemPresetFromCode(0));
        }
    }

    private void Update() {
        itemHeldInHandImage.transform.position = Input.mousePosition;
    }

    public int GetItemAmount(string itemName){
        int result = 0;
        byte findingCode = ItemManager.GetCodeFromItemName(itemName);
        for (int i = 0; i < itemData.Length; i++){
            if(itemData[i].code == findingCode){
                result += itemData[i].amount;
            }
        }
        return result;
    }

    public bool ConsumeItem(string itemName, int amount){
        if(GetItemAmount(itemName) < amount)
            return false;

        byte findingCode = ItemManager.GetCodeFromItemName(itemName);

        for (int i = 0; i < itemData.Length; i++){
            if(itemData[i] != null && itemData[i].code == findingCode){
                if(itemData[i].amount > amount){
                    itemData[i].amount -= amount;
                    amount = 0;
                }else{
                    amount -= itemData[i].amount;
                    itemData[i].amount = 0;
                }
                
            }
        }
        return true;
    }
    
    public bool AddItem(ItemSlotData addedItemData){
        bool result = false;

        if(addedItemData.code == 0){
            return false;
        }

        for (int i = 0; i < itemData.Length; i++){
            if(itemData[i].code == addedItemData.code){
                itemData[i].amount += addedItemData.amount;
                result = true;
                break;
            }
        }

        if(result == false){
            for (int i = 0; i < itemData.Length; i++){
                if(itemData[i].code == 0){
                    itemData[i] = addedItemData;
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    public void ClickLeft(ItemSlot inventorySlot){
        if(itemHeldInHand.code != 0 && inventorySlot.data.code != 0 &&itemHeldInHand.code == inventorySlot.data.code){
            itemHeldInHand.amount += inventorySlot.data.amount;
            inventorySlot.data.code = 0;
            inventorySlot.data.amount = 0;
        }else{
            ItemSlotData.Swap(itemHeldInHand,inventorySlot.data);
        }
        itemHeldInHandImage.sprite = itemHeldInHand.itemPreset.itemSprite;
        itemHeldInHandAmount.text = (itemHeldInHand.code==0) ? "" : itemHeldInHand.amount.ToString();
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