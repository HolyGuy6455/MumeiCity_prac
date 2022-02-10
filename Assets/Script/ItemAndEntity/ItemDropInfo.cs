using UnityEngine;

[System.Serializable]
public class ItemDropInfo{
    public ItemPreset preset;
    public float chance;
    public ItemDropType itemDropType;
    public void DropItem(Hittable hittable){
        Debug.Log("DropItem");
        Vector3 location = new Vector3();
        location.x = hittable.transform.position.x;
        location.y = hittable.transform.position.y;
        location.z = hittable.transform.position.z;

        float chanceTemp = this.chance;
        while (chanceTemp > 0){
            float roolDice = Random.Range(0.0f,1.0f);
            if(chanceTemp < roolDice){
                break;
            }
            chanceTemp -= 1.0f;
            Debug.Log("Drop! : " + preset.name);

            Vector3 popForce = new Vector3();
            popForce.x = Random.Range(-ItemDroper.RANGE, ItemDroper.RANGE);
            popForce.y = ItemDroper.JUMP_POWER;
            popForce.z = Random.Range(-ItemDroper.RANGE, ItemDroper.RANGE);

            GameObject itemObject = GameObject.Instantiate(GameManager.Instance.itemManager.itemPickupPrefab,location+popForce/10,Quaternion.identity);
            ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
            itemPickup.itemData = ItemPickupData.create(preset);
            itemPickup.IconSpriteUpdate();
            
            itemPickup.GetComponent<Rigidbody>().AddForce(popForce,ForceMode.Impulse);
            itemObject.transform.SetParent(GameManager.Instance.itemManager.itemPickupParent.transform);
        }
    }
    public enum ItemDropType{
        NONE,
        DESTROY,
        HIT
    }
}