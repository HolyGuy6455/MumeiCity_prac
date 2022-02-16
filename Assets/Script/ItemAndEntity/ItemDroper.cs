using UnityEngine;
using System.Collections.Generic;

public class ItemDroper : MonoBehaviour{
    public static float RANGE = 5.0f;
    public static float JUMP_POWER = 8.0f;
    [SerializeField] List<ItemDropInfo> dropAmounts;

    private void Start() {
        Hittable hittable = this.GetComponent<Hittable>();
        hittable.DeadEventHandler += this.DropItemWhenDestroy;
        hittable.HitEventHandler += this.DropItemWhenHit;
    }

    public void InitializeItemDrop(List<ItemDropInfo> dropAmounts){
        this.dropAmounts = dropAmounts;
    }

    public void DropItemWhenDestroy(Hittable hittable){
        Debug.Log("DropItemWhenDestroy");
        foreach (ItemDropInfo itemDropInfo in dropAmounts){
            if(itemDropInfo.itemDropType == ItemDropInfo.ItemDropType.DESTROY){
                itemDropInfo.DropItem(hittable);
            }
        }
    }

    public void DropItemWhenHit(Hittable hittable){
        foreach (ItemDropInfo itemDropInfo in dropAmounts){
            if(itemDropInfo.itemDropType == ItemDropInfo.ItemDropType.HIT){
                itemDropInfo.DropItem(hittable);
            }
        }
    }

}
