using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

/*
 * 건설 관련된 내용 전체를 관리하는 객체
 */
public class BuildingManager : MonoBehaviour {
    public GameManager.UpdateUI onToolChangedCallback;
    public GameObject constructureSample;
    [SerializeField] List<BuildingPreset> buildingPresets;
    public ConstructionArea constructionArea;
    [SerializeField] BuildingPreset nowBuilding;
    public GameObject buildingsParent;
    public GameObject lightAndShadow;
    public int lastID = 1;

    public AstarPath astarPath;

    public BuildingPreset _nowBuilding{get{return nowBuilding;}}

    private void Awake() {
        IssueID();
    }

    [ContextMenu("Manual Issue ID")]
    void IssueID(){
        List<BuildingObject> buildingList = wholeBuildingList();
        lastID = 0;
        foreach (BuildingObject buildingObject in buildingList){
            buildingObject.buildingData.id = lastID++;
        }
    }

    public BuildingObject FindBuildingObjectWithID(int findingID){
        BuildingObject result = null;
        List<BuildingObject> buildingList = wholeBuildingList();
        foreach (BuildingObject buildingObject in buildingList){
            if(buildingObject.buildingData.id == findingID){
                result = buildingObject;
                break;
            }
        }
        return result;
    }

    public List<BuildingObject> wholeBuildingList(){
        List<BuildingObject> result = new List<BuildingObject>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Building");

        foreach (GameObject child in objs){
            BuildingObject buildingObject = child.GetComponent<BuildingObject>();
            result.Add(buildingObject);
        }
        return result;
    }

    public static byte GetBuildingCode(BuildingPreset buildingPreset){
        BuildingManager buildingManager = GameManager.Instance.buildingManager;
        return ((byte)buildingManager.buildingPresets.IndexOf(buildingPreset));
    }

    public static BuildingPreset GetBuildingPreset(byte buildingCode){
        BuildingManager buildingManager = GameManager.Instance.buildingManager;
        // BuildingManager buildingManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BuildingManager>();
        int index = (int)buildingCode;
        return buildingManager.buildingPresets[index];
    }

    public static BuildingPreset GetBuildingPreset(string name){
        BuildingManager buildingManager = GameManager.Instance.buildingManager;
        foreach (BuildingPreset buildingPreset in buildingManager.buildingPresets){
            if(buildingPreset.name.CompareTo(name) == 0){
                return buildingPreset;
            }
        }
        return null;
    }

    public static List<BuildingPreset> GetGroupedListByBuildType(ToolType toolType){
        BuildingManager buildingManager = GameManager.Instance.buildingManager;
        List<BuildingPreset> result = buildingManager.buildingPresets;
        result = result.FindAll(buildingPreset => buildingPreset.buildTool == toolType);
        return result;
    }

    public void ReadyToBuild(BuildingPreset buildingData){
        this.nowBuilding = buildingData;
        constructionArea.SetBuildingData(buildingData);
    }

    public bool Build(){
        // 장애물이 있으면 못지어요
        if(constructionArea.isThereObstacle()){
            return false;
        }
        // 무슨 건물 지을지 결정 안했으면 못지어요
        if(this.nowBuilding == null){
            Debug.Log("buildingPreset is null");
            return false;
        }
        // 재료가 충분한지 확인
        Inventory inventoryManager = GameManager.Instance.inventory;
        List<NecessaryResource> materialList = nowBuilding.resourceList;
        bool doWeHaveMaterialEnough = true;
        foreach (NecessaryResource material in materialList){
            if(inventoryManager.GetItemAmount(material.itemDataName) < material.amount){
                doWeHaveMaterialEnough = false;
                Debug.Log("Not Enough Material");
                break;
            }
        }
        // 재료가 없으면 못지어요
        if(doWeHaveMaterialEnough == false){
            return false;
        }
        // 재료가 있다면 재료를 소모한다
        foreach (NecessaryResource material in materialList){
            inventoryManager.ConsumeItem(material.itemDataName,material.amount);
        }
        // 현재 플레이어 위치를 기준으로 건설을 한다
        Vector3 location = constructionArea.GetBuildingLocation();

        GameObject Built;
        if(nowBuilding.prefab != null){
            Built = Instantiate(nowBuilding.prefab,location,Quaternion.identity);
        }else{
            Built = Instantiate(constructureSample,location,Quaternion.identity);
        }
        

        Built.transform.SetParent(buildingsParent.transform);

        // 해당 건물이 가질 Building Data를 생성한다
        BuildingObject BuiltObject = Built.GetComponent<BuildingObject>();
        BuildingData buildingData = BuiltObject.buildingData;
        buildingData.code = GetBuildingCode(nowBuilding);
        buildingData.id = lastID++;
        const int itemSpace = 4;
        buildingData.items = new ItemSlotData[itemSpace];
        for (int i = 0; i < itemSpace; i++){
            buildingData.items[i] = ItemSlotData.Create(ItemData.Instant("None"));
        }
        switch (nowBuilding.name){
            case "ForesterHut":
                buildingData.facilityFunction = new SuperintendentFunction(3);
                break;
            case "Tent":
                buildingData.facilityFunction = new HouseFunction(12);
                break;
            default:
                break;
        }
        
        // 경로 재설정
        astarPath.Scan();

        return true;
    }

}