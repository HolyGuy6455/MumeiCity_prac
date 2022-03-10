using UnityEngine;
using System.Collections.Generic;

public class ItemPickup : MonoBehaviour, ITiemEventRebindInfo
{
    public ItemPickupData itemPickupData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TimeEventQueueTicket disapearEvent;
    [SerializeField] Animator animator;
    string ticketName;

    void Start() {
        IconSpriteUpdate();
        if(itemPickupData.ID == 0){
            itemPickupData.ID = GameManager.Instance.itemManager.IssueIDOne();
        }
        ticketName = "ItemPickup_"+itemPickupData.ID+"_Update";
        disapearEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1,ticketName, UpdatePerSecond);
    }

    public bool UpdatePerSecond(string ticketName){
        if(animator == null){
            return true;
        }
        itemPickupData.leftSecond -= 1;
        animator.SetInteger("LeftSecond",itemPickupData.leftSecond);
        if(itemPickupData.leftSecond < 0){
            Destroy(this.gameObject);
        }
        return false;
    }

    [ContextMenu("UpdateItemData")]
    public void IconSpriteUpdate(){
        if(itemPickupData == null)
            return;
        spriteRenderer.sprite = itemPickupData.itemData.itemSprite;
    }

    public void PickUp(){
        bool isItDone = GameManager.Instance.inventory.AddItem(this.ProcessToItemSlotData());
        GameManager.Instance.sence_.CleanReservation();
        if(isItDone){
            GameManager.Instance.achievementManager.AddTrial("PickUpItem",1);
            Destroy(gameObject);
        }
    }

    public ItemSlotData ProcessToItemSlotData(){
        return ItemSlotData.Create(itemPickupData.itemData);
    }

    public Dictionary<string, TimeManager.TimeEvent> GetDictionary(){
        Dictionary<string, TimeManager.TimeEvent> result = new Dictionary<string, TimeManager.TimeEvent>();

        string ticketName = "ItemPickup_"+itemPickupData.ID+"_Update";
        result[ticketName] = UpdatePerSecond;

        return result;
    }
}
