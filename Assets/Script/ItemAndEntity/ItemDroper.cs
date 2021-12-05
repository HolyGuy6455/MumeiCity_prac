using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroper : MonoBehaviour
{
    public IEntityDestroyEvent whenDestroy;
    public GameObject itemPickupPrefab;
    public ItemData itemData;
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
        itemPickup.item = itemData;
        // itemPickup.player = GameManager.Instance.PlayerTransform;
        itemPickup.IconSpriteUpdate();
        itemPickup.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-100.0f, 100.0f),200,Random.Range(-100.0f, 100.0f)));
    }
}
