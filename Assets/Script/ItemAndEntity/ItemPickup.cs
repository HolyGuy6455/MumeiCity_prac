using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemPickupData item;

    void Start() {
        IconSpriteUpdate();
    }

    public void IconSpriteUpdate(){
        if(item == null)
            return;
        this.GetComponentInChildren<SpriteRenderer>().sprite = item.itemPreset.itemSprite;
    }

    public void PickUp(){
        bool isItDone = GameManager.Instance.inventory.AddItem(this.ProcessToItemSlotData());
        GameManager.Instance.sence.CleanReservation();
        if(isItDone){
            Destroy(gameObject);
        }
    }

    public ItemSlotData ProcessToItemSlotData(){
        return ItemSlotData.Create(item.itemPreset);
    }
}
