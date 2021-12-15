using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroper : MonoBehaviour
{
    public IEntityDestroyEvent whenDestroy;
    [SerializeField] List<ItemDropAmount> itemDropAmounts = new List<ItemDropAmount>();
    [SerializeField] float range = 5.0f;
    [SerializeField] float jumpPower = 7.0f;
    void Start()
    {
        whenDestroy = this.GetComponent<IEntityDestroyEvent>();
        whenDestroy.EntityDestroyEventHandler += DropItem;
    }

    public void Add(ItemDropAmount itemDropAmount){
        this.itemDropAmounts.Add(itemDropAmount);
    }

    void DropItem(){
        Debug.Log("Item Drop");
        Vector3 location = new Vector3();
        location.x = this.transform.position.x;
        location.z = this.transform.position.z;

        foreach (ItemDropAmount item in itemDropAmounts){
            GameObject itemObject = Instantiate(GameManager.Instance.itemManager.itemPickupPrefab,location,Quaternion.identity);
            ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
            byte itemcode = ItemManager.GetCodeFromItemName(item.name);
            ItemPreset preset = ItemManager.GetItemPresetFromCode(itemcode);
            itemPickup.item = ItemPickupData.create(preset);
            itemPickup.IconSpriteUpdate();
            itemPickup.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-range, range),jumpPower,Random.Range(-range, range)),ForceMode.Impulse);
            itemObject.transform.SetParent(GameManager.Instance.itemPickupParent.transform);
        }
    }
}

[System.Serializable]
public class ItemDropAmount{
    public string name;
    public float chance;
}