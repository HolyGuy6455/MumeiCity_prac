using UnityEngine;

[System.Serializable]
public class ItemDropInfo{
    public ItemPreset preset;
    public float chance;
    public ItemDropType itemDropType;
    public enum ItemDropType{
        NONE,
        DESTROY,
        HIT
    }
    
    public void DropItem(Vector3 position){
        Vector3 location = new Vector3();
        location.x = position.x;
        location.y = position.y;
        location.z = position.z;

        float chanceTemp = this.chance;
        while (chanceTemp > 0){
            float roolDice = Random.Range(0.0f,1.0f);
            if(chanceTemp < roolDice){
                break;
            }
            chanceTemp -= 1.0f;

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
    
}