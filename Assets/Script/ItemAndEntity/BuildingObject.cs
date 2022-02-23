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
    public List<EffectiveTool> removalTool;
    [SerializeField] Animator animator;
 
    private void Start() {
        // TODO
        // 내가 에디터에서 만들때와 BuildingManager에서 만들때
        // Initilize()의 실행 시점이 달라야하는데 그렇지않아 문제가 생긴다
        // 이 부분은 조금 더 고민이 필요해 보인다. 일단 지금은 덮어두기식으로 간단하게 넘어간다

        buildingData.positionX = ((int)Mathf.Round(this.transform.position.x));
        buildingData.positionY = ((int)Mathf.Round(this.transform.position.y));
        buildingData.positionZ = ((int)Mathf.Round(this.transform.position.z));
        WorkPlace workPlace = this.GetComponent<WorkPlace>();

        switch (buildingData.buildingPreset.name){
            case "ForesterHut":
                SuperintendentData superintendentData = new SuperintendentData();
                buildingData.mediocrityData = superintendentData;
                superintendentData.workList = new bool[workPlace.taskInfos.Count];
                break;
            case "Tent":
                buildingData.mediocrityData = new HouseData(12);
                break;
            case "Bistro":
                ManufacturerData manufacturerData = new ManufacturerData();
                buildingData.mediocrityData = manufacturerData;
                manufacturerData.amount = new int[3];
                manufacturerData.dueDate = new int[3];
                break;
            default:
                break;
        }
        Initialize(this.buildingData);
    }

    // 오브젝트의 초기화
    public void Initialize(BuildingData buildingData){
        BuildingPreset buildingPreset = buildingData.buildingPreset;
        this.buildingData = buildingData;
        spriteRenderer.sprite = buildingPreset.sprite;

        Hittable hittable = GetComponent<Hittable>();
        hittable.SetEffectiveTool(removalTool);

        if(shadowObject != null){
            Vector3 shadowPostion = new Vector3();
            shadowPostion.x = this.transform.position.x;
            shadowPostion.y = (this.transform.position.y+this.transform.position.z);
            shadowObject.transform.position = shadowPostion;
        }
    }

    [ContextMenu("Manual Initialize")]
    void ManualInitialize(){
        Initialize(this.buildingData);
    }

    private void OnDestroy() {
        GameManager gameManager = GameManager.Instance;
        if(gameManager != null){
            gameManager.buildingManager.astarPath.Scan();
        }
    }

}
