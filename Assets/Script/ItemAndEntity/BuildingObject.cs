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
        
        if(workPlace != null){
            switch (workPlace._gameTab){
                case GameManager.GameTab.SUPERINTENDENT:
                    SuperintendentData superintendentData = new SuperintendentData();
                    superintendentData.workList = new bool[workPlace.taskInfos.Count];
                    buildingData.mediocrityData = superintendentData;
                    break;

                case GameManager.GameTab.MANUFACTURER:
                    ManufacturerData manufacturerData = new ManufacturerData();
                    manufacturerData.amount = new int[workPlace.taskInfos.Count];
                    manufacturerData.dueDate = new int[workPlace.taskInfos.Count];
                    buildingData.mediocrityData = manufacturerData;
                    break;

                case GameManager.GameTab.LABORATORY:
                    LaboratoryData laboratoryData = new LaboratoryData();
                    laboratoryData.dueDate = new int[workPlace.taskInfos.Count];
                    for (int i = 0; i < workPlace.taskInfos.Count; i++){
                        laboratoryData.dueDate[i] = int.MaxValue;
                    }
                    buildingData.mediocrityData = laboratoryData;
                    break;

                default:
                    break;
            }
        }else{
            switch (buildingData.buildingPreset.name){
                case "Tent":
                    buildingData.mediocrityData = new HouseData(12);
                    // GameManager.Instance.peopleManager.ResetHouseInfomation();
                    break;
                default:
                    break;
            }
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

    public void ChangeGameTab(string gametab){
        GameManager.Instance.ChangeGameTab(gametab);
    }

    public void Demolish(){
        GameManager.Instance.achievementManager.AddTrial("demolish",1);
    }

}
