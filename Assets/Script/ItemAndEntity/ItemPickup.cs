using UnityEngine;

public class ItemPickup : Interactable
{
    public ItemPickupData item;

    public override void Interact()
    {
        base.Interact();

        this.PickUp();
    }

    void Start() {
        IconSpriteUpdate();
    }

    public void IconSpriteUpdate(){
        if(item == null)
            return;
        this.GetComponentInChildren<SpriteRenderer>().sprite = item.itemPreset.icon;
    }

    void PickUp(){
        bool isItDone = GameManager.Instance.inventory.AddItem(this.ProcessToItemSlotData());
        if(isItDone){
            Destroy(gameObject);
        }
    }

    public ItemSlotData ProcessToItemSlotData(){
        return ItemSlotData.Create(item.itemPreset);
    }
}
