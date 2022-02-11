using UnityEngine;
using System.Collections.Generic;

public class ItemDroper : MonoBehaviour{
    public static float RANGE = 5.0f;
    public static float JUMP_POWER = 8.0f;
    [SerializeField] List<ItemDropInfo> dropAmounts;

    public void InitializeItemDrop(List<ItemDropInfo> dropAmounts){
        Hittable hittable = this.GetComponent<Hittable>();

        // 새로 들어온 것들을 등록한다
        foreach (ItemDropInfo itemDropInfo in dropAmounts){
            switch (itemDropInfo.itemDropType){
                case ItemDropInfo.ItemDropType.DESTROY:
                    hittable.DeadEventHandler += itemDropInfo.DropItem;
                    break;
                case ItemDropInfo.ItemDropType.HIT:
                    hittable.HitEventHandler += itemDropInfo.DropItem;
                    break;
                default:
                    break;
            }
        }

        this.dropAmounts = dropAmounts;
    }

    public void RemoveItemDrop() {
        Hittable hittable = this.GetComponent<Hittable>();

        // 기존에 있던 것들을 먼저 지운다
        foreach (ItemDropInfo itemDropInfo in dropAmounts){
            switch (itemDropInfo.itemDropType){
                case ItemDropInfo.ItemDropType.DESTROY:
                    hittable.DeadEventHandler -= itemDropInfo.DropItem;
                    break;
                case ItemDropInfo.ItemDropType.HIT:
                    hittable.HitEventHandler -= itemDropInfo.DropItem;
                    break;
                default:
                    break;
            }
        }
    }

}
