using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    
    public ItemSlotData itemHeldInHand;
    
    [SerializeField] Image itemHeldInHandImage;
    [SerializeField] Text itemHeldInHandAmount;
    [SerializeField] InventoryUI inventoryUI;
    public ItemSlotData[] itemData;
    int itemSpace = 20;

    private void Start() {
        itemHeldInHand = ItemSlotData.Create(ItemData.Instant("None"));
        itemData = new ItemSlotData[itemSpace];
        for (int i = 0; i < itemSpace; i++){
            itemData[i] = ItemSlotData.Create(ItemData.Instant("None"));
        }
    }

    // private void Update() {
        // itemHeldInHandTransform.transform.position = Input.mousePosition;
    // }

    public int GetItemAmount(string itemName){
        int result = 0;
        for (int i = 0; i < itemData.Length; i++){
            if(itemData[i].itemData.isThisName(itemName)){
                result += itemData[i].amount;
            }
        }
        return result;
    }

    public bool ConsumeItem(string itemName, int amount){
        if(GetItemAmount(itemName) < amount)
            return false;

        for (int i = 0; i < itemData.Length; i++){
            if(itemData[i] != null && itemData[i].itemData.isThisName(itemName)){
                if(itemData[i].amount > amount){
                    itemData[i].amount -= amount;
                    amount = 0;
                }else{
                    amount -= itemData[i].amount;
                    itemData[i].amount = 0;
                }
                
            }
        }
        inventoryUI.LoadItemSlotData();
        return true;
    }
    
    public bool AddItem(ItemSlotData addedItemData){
        bool result = false;

        if(addedItemData.itemData.isNone()){
            return false;
        }

        for (int i = 0; i < itemData.Length; i++){
            if(itemData[i].itemData.isThisName(addedItemData.itemName)){
                itemData[i].amount += addedItemData.amount;
                result = true;
                break;
            }
        }

        if(result == false){
            for (int i = 0; i < itemData.Length; i++){
                if(itemData[i].itemData.isNone()){
                    itemData[i] = addedItemData;
                    result = true;
                    break;
                }
            }
        }
        inventoryUI.LoadItemSlotData();
        return result;
    }

    public void ClickLeft(ItemSlot inventorySlot){
        if(!itemHeldInHand.itemData.isNone()
        && !inventorySlot.itemSlotData.itemData.isNone() 
        && itemHeldInHand.itemData.isThisName(inventorySlot.itemSlotData.itemName))
        {
            itemHeldInHand.amount += inventorySlot.itemSlotData.amount;
            inventorySlot.itemSlotData.itemName = "None";
            inventorySlot.itemSlotData.amount = 0;
        }else{
            ItemSlotData.Swap(itemHeldInHand,inventorySlot.itemSlotData);
            inventorySlot.UpdateUI();
        }
        itemHeldInHandImage.sprite = itemHeldInHand.itemData.itemSprite;
        itemHeldInHandAmount.text = (itemHeldInHand.itemData.isNone()) ? "" : itemHeldInHand.amount.ToString();
    }

    public List<ItemSlotData> GetDataList(){
        List<ItemSlotData> result = new List<ItemSlotData>();
        for (int i = 0; i < itemData.Length; i++){
            if(itemData[i].itemData.isNone()){
                continue;
            }
            result.Add(itemData[i]);
        }
        return result;
    }
}