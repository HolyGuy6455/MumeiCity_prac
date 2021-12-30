using UnityEngine;

[System.Serializable]
public class ItemDropInfo{
    public string name;
    public float chance;
    public ItemDropType itemDropType;
    public void DropItem(Component component){
        Vector3 location = new Vector3();
        location.x = component.transform.position.x;
        location.y = component.transform.position.y;
        location.z = component.transform.position.z;

        float chanceTemp = this.chance;
        while (chanceTemp > 0){
            float roolDice = Random.Range(0.0f,1.0f);
            if(chanceTemp < roolDice){
                break;
            }
            chanceTemp -= 1.0f;
            Debug.Log("Drop!");

            Vector3 popForce = new Vector3();
            popForce.x = Random.Range(-ItemDroper.RANGE, ItemDroper.RANGE);
            popForce.y = ItemDroper.JUMP_POWER;
            popForce.z = Random.Range(-ItemDroper.RANGE, ItemDroper.RANGE);

            GameObject itemObject = GameObject.Instantiate(GameManager.Instance.itemManager.itemPickupPrefab,location+popForce/10,Quaternion.identity);
            ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
            byte itemcode = ItemManager.GetCodeFromItemName(name);
            ItemPreset preset = ItemManager.GetItemPresetFromCode(itemcode);
            itemPickup.item = ItemPickupData.create(preset);
            itemPickup.IconSpriteUpdate();
            
            itemPickup.GetComponent<Rigidbody>().AddForce(popForce,ForceMode.Impulse);
            itemObject.transform.SetParent(GameManager.Instance.itemPickupParent.transform);
        }
    }
    public enum ItemDropType{
        NONE,
        DESTROY,
        HIT
    }
}