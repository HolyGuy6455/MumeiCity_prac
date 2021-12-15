using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    
    // public GameManager.UpdateUI onItemChangedCallback;
    // public List<ItemSlotData> items = new List<ItemSlotData>(); 
    
    // ItemSlotData[] slots;
    // public GameObject inventoryUI;
    [SerializeField] Transform itemsParents;
    public ItemSlotData itemHeldInHand;
    [SerializeField] Image itemHeldInHandImage;
    InventorySlot[] slots;

    [SerializeField] List<ItemSlotData> dataView;
    // public int itemSpace = 20;

    private void Start() {
        itemHeldInHand = null;
        slots = itemsParents.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots){
            slot.onFocusCallback += OnFocusSlot;
            slot.onClickCallback += OnClickSlot;
            slot.data = null;
        }
    }

    private void Update() {
        itemHeldInHandImage.transform.position = Input.mousePosition;
    }

    private void updatedSlot(){
        dataView = new List<ItemSlotData>();
        foreach (InventorySlot slot in slots){
            if(slot.data != null)
                dataView.Add(slot.data);
        }
    }
    
    public int GetItemAmount(string itemName){
        int result = 0;
        byte findingCode = GameManager.Instance.itemManager.GetCodeFromItemName(itemName);
        for (int i = 0; i < slots.Length; i++){
            if(slots[i].data != null && slots[i].data.code == findingCode){
                result += slots[i].data.amount;
            }
        }
        return result;
    }

    public bool ConsumeItem(string itemName, int amount){
        if(GetItemAmount(itemName) < amount)
            return false;

        byte findingCode = GameManager.Instance.itemManager.GetCodeFromItemName(itemName);

        for (int i = 0; i < slots.Length; i++){
            if(slots[i].data != null && slots[i].data.code == findingCode){
                if(slots[i].data.amount > amount){
                    slots[i].data.amount -= amount;
                    slots[i].UpdateUI();
                    amount = 0;
                }else{
                    amount -= slots[i].data.amount;
                    slots[i].data = null;
                    slots[i].UpdateUI();
                }
                
            }
        }
        updatedSlot();
        return true;
    }
    
    public bool AddItem(ItemSlotData addedItemData){
        bool result = false;

        for (int i = 0; i < slots.Length; i++){
            if(slots[i].data != null && slots[i].data.code == addedItemData.code){
                ItemSlotData added = slots[i].data;
                added.amount += addedItemData.amount;
                slots[i].UpdateUI();
                result = true;
                break;
            }
        }

        if(result == false){
            for (int i = 0; i < slots.Length; i++){
                if(slots[i].data == null){
                    slots[i].data = addedItemData;
                    slots[i].UpdateUI();
                    result = true;
                    break;
                }
            }
        }
        updatedSlot();
        return result;
    }

    private void OnFocusSlot(InventorySlot inventorySlot, PointerEventData eventData){
        for (int i = 0; i < slots.Length; i++){
            if(slots[i] == inventorySlot){
                Debug.Log("slot is "+i);
            }
        }
    }

    private void OnClickSlot(InventorySlot inventorySlot, PointerEventData eventData){

        if(eventData.button == PointerEventData.InputButton.Left){
            if(itemHeldInHand != null && inventorySlot.data != null &&itemHeldInHand.code == inventorySlot.data.code){
                itemHeldInHand.amount += inventorySlot.data.amount;
                inventorySlot.data = null;
            }else{
                ItemSlotData tempData = itemHeldInHand;
                itemHeldInHand = inventorySlot.data;
                inventorySlot.data = tempData;
            }

        }else if (eventData.button == PointerEventData.InputButton.Right){
            if(itemHeldInHand == null && inventorySlot.data != null){
                int amount = inventorySlot.data.amount/2;
                Debug.Log("item slot divide! : "+amount);
            }else if(itemHeldInHand != null){
                if(inventorySlot.data == null){
                    inventorySlot.data = itemHeldInHand.CopyEmpty();
                    inventorySlot.data.amount += 1;
                    itemHeldInHand.amount -= 1;
                    if(itemHeldInHand.amount <= 0){
                        itemHeldInHand = null;
                    }
                }else if(itemHeldInHand.code == inventorySlot.data.code) {
                    inventorySlot.data.amount += 1;
                    itemHeldInHand.amount -= 1;
                    if(itemHeldInHand.amount <= 0){
                        itemHeldInHand = null;
                    }
                }
            }
        }

        if(itemHeldInHand != null){
            itemHeldInHandImage.sprite = itemHeldInHand.itemPreset.itemSprite;
            itemHeldInHandImage.gameObject.SetActive(true);
        }else{
            itemHeldInHandImage.sprite = null;
            itemHeldInHandImage.gameObject.SetActive(false);
        }
        inventorySlot.UpdateUI();
        updatedSlot();
    }
}