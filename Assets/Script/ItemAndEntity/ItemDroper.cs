using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDroper : MonoBehaviour
{
    public IEntityDestroyEvent whenDestroy;
    [SerializeField] List<ItemDropAmount> itemDropAmounts = new List<ItemDropAmount>();
    [SerializeField] float range = 5.0f;
    [SerializeField] float jumpPower = 8.0f;
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
        location.y = this.transform.position.y;
        location.z = this.transform.position.z;

        foreach (ItemDropAmount item in itemDropAmounts){
            float chance = item.chance;
            while (chance > 0){
                float roolDice = Random.Range(0.0f,1.0f);
                Debug.Log(chance + " vs " + roolDice);
                if(chance < roolDice){
                    break;
                }
                chance -= 1.0f;
                 
                Vector3 popForce = new Vector3();
                popForce.x = Random.Range(-range, range);
                popForce.y = jumpPower;
                popForce.z = Random.Range(-range, range);

                GameObject itemObject = Instantiate(GameManager.Instance.itemManager.itemPickupPrefab,location+popForce/10,Quaternion.identity);
                ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
                byte itemcode = ItemManager.GetCodeFromItemName(item.name);
                ItemPreset preset = ItemManager.GetItemPresetFromCode(itemcode);
                itemPickup.item = ItemPickupData.create(preset);
                itemPickup.IconSpriteUpdate();
                
                itemPickup.GetComponent<Rigidbody>().AddForce(popForce,ForceMode.Impulse);
                itemObject.transform.SetParent(GameManager.Instance.itemPickupParent.transform);
            }

        }
    }
}

[System.Serializable]
public class ItemDropAmount{
    public string name;
    public float chance;
}