using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemPickupData itemData;

    void Start() {
        IconSpriteUpdate();
    }

    public void IconSpriteUpdate(){
        if(itemData == null)
            return;
        this.GetComponentInChildren<SpriteRenderer>().sprite = itemData.itemPreset.itemSprite;
    }

    public void PickUp(){
        bool isItDone = GameManager.Instance.inventory.AddItem(this.ProcessToItemSlotData());
        GameManager.Instance.sence.CleanReservation();
        if(isItDone){
            Destroy(gameObject);
        }
    }

    public ItemSlotData ProcessToItemSlotData(){
        return ItemSlotData.Create(itemData.itemPreset);
    }
}
