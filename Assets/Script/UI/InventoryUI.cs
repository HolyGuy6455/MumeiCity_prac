using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParents;
    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start(){
        GameManager.Instance.inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParents.GetComponentsInChildren<InventorySlot>();
    }

    void UpdateUI(){
        GameManager gameManager = GameManager.Instance;
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < gameManager.inventory.items.Count){
                slots[i].AddItem(gameManager.inventory.items[i]);
            }else{
                slots[i].ClearSlot();
            }
        }
    }
}
