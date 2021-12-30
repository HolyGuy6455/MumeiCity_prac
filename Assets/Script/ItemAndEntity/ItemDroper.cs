using UnityEngine;

public class ItemDroper : MonoBehaviour{
    public static float RANGE = 5.0f;
    public static float JUMP_POWER = 8.0f;

    public void InitializeItemDrop(BuildingPreset buildingPreset){
        Hittable hittable = this.GetComponent<Hittable>();

        foreach (ItemDropInfo itemDropInfo in buildingPreset.dropAmounts){
            switch (itemDropInfo.itemDropType){
                case ItemDropInfo.ItemDropType.DESTROY:
                    hittable.EntityDestroyEventHandler += itemDropInfo.DropItem;
                    break;
                case ItemDropInfo.ItemDropType.HIT:
                    hittable.EntityHitEventHandler += itemDropInfo.DropItem;
                    break;
                default:
                    break;
            }
        }
    }

}
