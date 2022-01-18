using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemPickupData itemData;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start() {
        IconSpriteUpdate();
    }

    public void IconSpriteUpdate(){
        if(itemData == null)
            return;
        spriteRenderer.sprite = itemData.itemPreset.itemSprite;
    }

    public void PickUp(){
        bool isItDone = GameManager.Instance.inventory.AddItem(this.ProcessToItemSlotData());
        GameManager.Instance.sence_.CleanReservation();
        if(isItDone){
            Destroy(gameObject);
        }
    }

    public ItemSlotData ProcessToItemSlotData(){
        return ItemSlotData.Create(itemData.itemPreset);
    }
}
