using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroper : MonoBehaviour
{
    public IEntityDestroyEvent whenDestroy;
    [SerializeField] GameObject itemPickupPrefab;
    [SerializeField] ItemPreset itemPreset;
    [SerializeField] int minAmount = 0;
    [SerializeField] int maxAmount = 1;
    [SerializeField] float range = 5.0f;
    [SerializeField] float jumpPower = 7.0f;
    void Start()
    {
        whenDestroy = this.GetComponent<IEntityDestroyEvent>();
        whenDestroy.EntityDestroyEventHandler += DropItem;
    }

    void DropItem(){
        Debug.Log("Item Drop");
        Vector3 location = new Vector3();
        location.x = this.transform.position.x;
        location.z = this.transform.position.z;

        GameObject itemObject = Instantiate(itemPickupPrefab,location,Quaternion.identity);
        ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
        itemPickup.item = ItemData.create(itemPreset);
        itemPickup.IconSpriteUpdate();
        itemPickup.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-range, range),jumpPower,Random.Range(-range, range)),ForceMode.Impulse);
        itemObject.transform.SetParent(GameManager.Instance.itemPickupParent.transform);
    }
}
