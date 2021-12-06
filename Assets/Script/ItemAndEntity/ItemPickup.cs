using UnityEngine;

public class ItemPickup : Interactable
{
    public ItemData item;

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
        // Debug.Log("Picking up " + this.item.itemName);
        bool isItDone = GameManager.Instance.inventoryManager.AddItem(item);
        if(isItDone){
            Destroy(gameObject);
        }
    }
}
