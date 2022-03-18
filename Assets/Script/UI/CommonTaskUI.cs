using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommonTaskUI : MonoBehaviour{
    [SerializeField] ItemSlot[] StorageSlots;
    [SerializeField] GameObject mainView;
    [SerializeField] GameObject unableView;
    // Start is called before the first frame update
    protected virtual void Start(){
        foreach (ItemSlot slot in StorageSlots){
            slot.onFocusCallback += OnFocusSlot;
            slot.onClickCallback += OnClickSlot;
        }
    }

    public virtual void UpdateUI(){
        ItemSlotData[] itemSlotData = GameManager.Instance.interactingBuilding.buildingData.items;
        for (int i = 0; i < itemSlotData.Length; i++){
            StorageSlots[i].itemSlotData = itemSlotData[i];
            StorageSlots[i].UpdateUI();
        }

        BuildingObject buildingObj = GameManager.Instance.interactingBuilding;
        WorkPlace workPlace = buildingObj.GetComponent<WorkPlace>();
        if(workPlace == null){
            return;
        }
        if(workPlace.hiringPerson && buildingObj.buildingData.workerID == 0){
            mainView.SetActive(false);
            unableView.SetActive(true);
        }else{
            mainView.SetActive(true);
            unableView.SetActive(false);
        }
    }

    void OnFocusSlot(ItemSlot inventorySlot, PointerEventData eventData){
        if(inventorySlot.itemSlotData.itemData.isNone()){
            return;
        }
    }

    void OnClickSlot(ItemSlot inventorySlot, PointerEventData eventData){
        GameManager.Instance.inventory.ClickLeft(inventorySlot);
        inventorySlot.UpdateUI();
    }
}
