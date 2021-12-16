using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ForesterUI : MonoBehaviour{
    [SerializeField] GameObject StorageSlotParent;
    [SerializeField] ItemSlot[] StorageSlots;
    [SerializeField] Toggle TreeToLogToggle;
    [SerializeField] Toggle PinrconeToTreeToggle;
    // Start is called before the first frame update
    void Start(){
        StorageSlots = StorageSlotParent.GetComponentsInChildren<ItemSlot>();
        foreach (ItemSlot slot in StorageSlots){
            slot.onFocusCallback += OnFocusSlot;
            slot.onClickCallback += OnClickSlot;
        }
    }

    public void LoadItemSlotData(){
        ItemSlotData[] itemSlotData = GameManager.Instance.interactingBuilding.items;
        for (int i = 0; i < itemSlotData.Length; i++){
            StorageSlots[i].data = itemSlotData[i];
            StorageSlots[i].UpdateUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFocusSlot(ItemSlot inventorySlot, PointerEventData eventData){
        if(inventorySlot.data.code == 0){
            return;
        }
    }

    private void OnClickSlot(ItemSlot inventorySlot, PointerEventData eventData){
        GameManager.Instance.inventory.ClickLeft(inventorySlot);
        inventorySlot.UpdateUI();
    }
}
