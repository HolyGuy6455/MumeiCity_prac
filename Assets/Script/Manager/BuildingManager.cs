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
    public int lastID = 1;

    public AstarPath astarPath;

    public BuildingPreset _nowBuilding{get{return nowBuilding;}}

    private void Start() {
        List<Tool> tools = GameManager.Instance.tools;

        List<BuildingObject> buildingList = wholeBuildingList();
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
        foreach (Transform childTransform in buildingsParent.transform){
            BuildingObject buildingObject = childTransform.GetComponent<BuildingObject>();
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

    public static List<BuildingPreset> GetGroupedListByBuildType(Tool.ToolType toolType){
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
            Debug.Log("There is Obstacles");
            return false;
        }
        // 무슨 건물 지을지 결정 안했으면 못지어요
        if(this.nowBuilding == null){
            Debug.Log("buildingPreset is null");
            return false;
        }
        // 재료가 충분한지 확인
        Inventory inventoryManager = GameManager.Instance.inventory;
        List<BuildingResource> materialList = nowBuilding.resourceList;
        bool doWeHaveMaterialEnough = true;
        foreach (BuildingResource material in materialList){
            if(inventoryManager.GetItemAmount(material.preset.name) < material.amount){
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
        foreach (BuildingResource material in materialList){
            inventoryManager.ConsumeItem(material.preset.name,material.amount);
        }
        // 현재 플레이어 위치를 기준으로 건설을 한다
        Vector3 location = new Vector3();
        location.x = Mathf.Round(constructionArea.transform.position.x);
        location.y = Mathf.Round(constructionArea.transform.position.y);
        location.z = Mathf.Round(constructionArea.transform.position.z);

        Debug.Log("Build Position "+location);
        GameObject Built = Instantiate(constructureSample,location,Quaternion.identity);
        Built.transform.SetParent(buildingsParent.transform);

        // 해당 건물이 가질 Building Data를 생성한다
        BuildingObject BuiltObject = Built.GetComponent<BuildingObject>();
        BuildingData buildingData = BuiltObject.buildingData;
        buildingData.code = GetBuildingCode(nowBuilding);
        buildingData.id = lastID++;
        const int itemSpace = 4;
        buildingData.items = new ItemSlotData[itemSpace];
        for (int i = 0; i < itemSpace; i++){
            buildingData.items[i] = ItemSlotData.Create(ItemManager.GetItemPresetFromCode(0));
        }
        switch (nowBuilding.name){
            case "ForesterHut":
                buildingData.mediocrityData = new ForesterHutData();
                break;
            case "Tent":
                buildingData.mediocrityData = new HouseData(4);
                break;
            default:
                break;
        }

        // DropItem을 설정
        if(nowBuilding.dropAmounts.Count > 0){
            Built.AddComponent<ItemDroper>().InitializeItemDrop(nowBuilding.dropAmounts);
        }
        Hittable hittable = Built.GetComponent<Hittable>();
        hittable.effectiveTool = nowBuilding.removalTool;
        
        // 경로 재설정
        astarPath.Scan();

        return true;
    }

}