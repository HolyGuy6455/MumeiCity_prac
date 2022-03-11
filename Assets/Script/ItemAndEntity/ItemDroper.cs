using UnityEngine;
using System.Collections.Generic;

public class ItemDroper : MonoBehaviour{
    public static float RANGE = 5.0f;
    public static float JUMP_POWER = 8.0f;
    [SerializeField] List<ItemDropInfo> dropAmounts;
    private void Awake() {
        Debug.Log("Awake - " + this.gameObject + " - " + this.gameObject.GetInstanceID());
    }

    public void InitializeItemDrop(List<ItemDropInfo> dropAmounts){
        this.dropAmounts = dropAmounts;
        foreach (ItemDropInfo item in this.dropAmounts){
            Debug.Log(item.itemName);
        }
        Debug.Log("InitializeItemDrop - " + this.gameObject + " - " + this.GetInstanceID());
    }

    public List<ItemDropInfo> GetDropInfos(){
        return dropAmounts;
    }

    public void DropItemWhenDestroy(){
        foreach (ItemDropInfo itemDropInfo in dropAmounts){
            if(itemDropInfo.itemDropType == ItemDropInfo.ItemDropType.DESTROY){
                itemDropInfo.DropItem(this.transform.position);
            }
        }
    }

    public void DropItemWhenHit(){
        foreach (ItemDropInfo itemDropInfo in dropAmounts){
            if(itemDropInfo.itemDropType == ItemDropInfo.ItemDropType.HIT){
                itemDropInfo.DropItem(this.transform.position);
            }
        }
    }

}
