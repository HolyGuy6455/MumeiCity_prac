using UnityEngine;
using System.Collections.Generic;
using FMODUnity;

public class ItemPickup : MonoBehaviour, ITiemEventRebindInfo
{
    public ItemPickupData itemPickupData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TimeEventQueueTicket disapearEvent;
    [SerializeField] Animator animator;
    [SerializeField] bool isPicked;
    [SerializeField] float inhaleSpeed = 1.5f;
    [SerializeField] float inhaleAcceleration = 1.5f;
    [SerializeField] StudioEventEmitter studioEventEmitter;
    string ticketName;
    

    void Start() {
        IconSpriteUpdate();
        if(itemPickupData.ID == 0){
            itemPickupData.ID = GameManager.Instance.itemManager.IssueIDOne();
        }
        ticketName = "ItemPickup_"+itemPickupData.ID+"_Update";
        disapearEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1,ticketName, UpdatePerSecond);

        isPicked = false;
    }

    private void Update() {
        if(!isPicked){
            return;
        }

        this.transform.position = 
            this.transform.position
            + Vector3.Normalize(GameManager.Instance.PlayerTransform.position - this.transform.position)
            * Time.deltaTime * inhaleSpeed;
        inhaleSpeed += Time.deltaTime * inhaleAcceleration;

        float distance = Vector3.Distance(GameManager.Instance.PlayerTransform.position, this.transform.position);
        if(distance <= 1.0f){
            PickUp();
        }
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
        studioEventEmitter.Play();
        GameManager.Instance.sence_.CleanReservation();
        if(isItDone){
            GameManager.Instance.achievementManager.AddTrial("PickUpItem",1);
            if(itemPickupData.itemName.CompareTo("Berry") == 0){
                GameManager.Instance.achievementManager.AddTrial("GetBerry",1);
            }
            Destroy(gameObject);
        }
    }

    public void Selected(){
        isPicked = true;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<BoxCollider>().isTrigger = true;
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
