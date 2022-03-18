using System.Collections.Generic;
using UnityEngine;

/*
 * 인게임에서 보이는 건물의 GameObject
 */
public class BuildingObject : MonoBehaviour
{
    public BuildingData buildingData;
    public SpriteRenderer spriteRenderer;
    public GameObject shadowObject;
    [SerializeField] Animator animator;
    Hittable hittable;
 
    private void Awake() {
        // TODO
        // 내가 에디터에서 만들때와 BuildingManager에서 만들때
        // Initilize()의 실행 시점이 달라야하는데 그렇지않아 문제가 생긴다
        // 이 부분은 조금 더 고민이 필요해 보인다. 일단 지금은 덮어두기식으로 간단하게 넘어간다

        buildingData.positionX = ((int)Mathf.Round(this.transform.position.x));
        buildingData.positionY = ((int)Mathf.Round(this.transform.position.y));
        buildingData.positionZ = ((int)Mathf.Round(this.transform.position.z));

        const int itemSpace = 4;
        buildingData.items = new ItemSlotData[itemSpace];
        for (int i = 0; i < itemSpace; i++){
            buildingData.items[i] = ItemSlotData.Create(ItemData.Instant("None"));
        }

        hittable = GetComponent<Hittable>();
        buildingData.hp = hittable.HP;

        Initialize(this.buildingData);
    }

    // 오브젝트의 초기화
    public void Initialize(BuildingData inputBuildingData){
        this.buildingData = inputBuildingData;
        WorkPlace workPlace = this.GetComponent<WorkPlace>();
        if(workPlace != null){
            switch (workPlace._gameTab){
                case GameManager.GameTab.SUPERINTENDENT:
                    this.buildingData.facilityFunction = new SuperintendentFunction(workPlace.taskInfos.Count);
                    break;

                case GameManager.GameTab.MANUFACTURER:
                    this.buildingData.facilityFunction = new ManufacturerFunction(workPlace.taskInfos.Count);
                    break;

                // case GameManager.GameTab.LABORATORY:
                //     this.buildingData.facilityFunction = new LaboratoryFunction(workPlace.taskInfos.Count);
                //     break;

                default:
                    break;
            }
        }else{
            switch (buildingData.buildingPreset.name){
                case "Tent":
                    this.buildingData.facilityFunction = new HouseFunction(4);
                    break;
                case "House":
                    this.buildingData.facilityFunction = new HouseFunction(9);
                    break;
                case "Chest":
                    this.buildingData.facilityFunction = new ChestFunction();
                    break;
                default:
                    break;
            }
        }

        BuildingPreset buildingPreset = buildingData.buildingPreset;
        spriteRenderer.sprite = buildingPreset.sprite;

        if(this.buildingData.facilityFunction != null){
            if(this.buildingData.content == null || this.buildingData.content.CompareTo("") == 0){
                buildingData.facilityFunction.SaveMediocrityData(buildingData);
            }
            buildingData.facilityFunction.ReloadMediocrityData(buildingData);
        }

        if(shadowObject != null){
            Vector3 shadowPostion = new Vector3();
            shadowPostion.x = this.transform.position.x;
            shadowPostion.y = (this.transform.position.y+this.transform.position.z);
            shadowObject.transform.position = shadowPostion;
        }

        hittable.HP = buildingData.hp;
    }

    [ContextMenu("Manual Initialize")]
    void ManualInitialize(){
        Initialize(this.buildingData);
    }

    public void ChangeGameTab(string gametab){
        GameManager.Instance.ChangeGameTab(gametab);
    }

    public void RefreshHP(){
        buildingData.hp = hittable.HP;
    }

    public void Demolish(){
        GameManager.Instance.achievementManager.AddTrial("demolish",1);
    }

    public void DropItemInside(){
        Vector3 location = this.transform.position;
        
        foreach (ItemSlotData item in buildingData.items){
            for (int i = 0; i < item.amount; i++){
                Vector3 popForce = new Vector3();
                popForce.x = Random.Range(-ItemDroper.RANGE, ItemDroper.RANGE);
                popForce.y = ItemDroper.JUMP_POWER;
                popForce.z = Random.Range(-ItemDroper.RANGE, ItemDroper.RANGE);

                GameObject itemObject = GameObject.Instantiate(GameManager.Instance.itemManager.itemPickupPrefab,location+popForce/10,Quaternion.identity);
                ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
                itemPickup.itemPickupData = ItemPickupData.create(ItemData.Instant(item.itemName));
                itemPickup.IconSpriteUpdate();
                
                itemPickup.GetComponent<Rigidbody>().AddForce(popForce,ForceMode.Impulse);
                itemObject.transform.SetParent(GameManager.Instance.itemManager.itemPickupParent.transform);
            }
        }
    }

}
