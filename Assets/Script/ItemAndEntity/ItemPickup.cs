using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemPickupData itemData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TimeEventQueueTicket disapearEvent;
    [SerializeField] Animator animator;
    string ticketName;

    void Start() {
        IconSpriteUpdate();
        ticketName = "ItemPickup_"+this.GetInstanceID()+"_Update";
        disapearEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1,ticketName, UpdatePerSecond);
    }

    public bool UpdatePerSecond(string ticketName){
        if(animator == null){
            return true;
        }
        itemData.leftSecond -= 1;
        animator.SetInteger("LeftSecond",itemData.leftSecond);
        if(itemData.leftSecond < 0){
            Destroy(this.gameObject);
        }
        return false;
    }

    [ContextMenu("UpdateItemData")]
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
