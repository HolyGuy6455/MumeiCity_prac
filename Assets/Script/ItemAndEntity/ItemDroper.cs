using UnityEngine;
using System.Collections.Generic;

public class ItemDroper : MonoBehaviour{
    public static float RANGE = 5.0f;
    public static float JUMP_POWER = 8.0f;
    [SerializeField] List<ItemDropInfo> dropAmounts;

    public void InitializeItemDrop(List<ItemDropInfo> dropAmounts){
        this.dropAmounts = dropAmounts;
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
