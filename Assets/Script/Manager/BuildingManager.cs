using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

/*
 * 건설 관련된 내용 전체를 관리하는 객체
 */
public class BuildingManager : MonoBehaviour {
    public GameManager.UpdateUI onToolChangedCallback;
    public GameObject constructure;
    [SerializeField] List<BuildingPreset> dictionaryAxe = new List<BuildingPreset>();
    [SerializeField] List<BuildingPreset> dictionaryLantern = new List<BuildingPreset>();
    [SerializeField] List<BuildingPreset> dictionaryShovel = new List<BuildingPreset>();
    [SerializeField] List<BuildingPreset> dictionaryPickaxe = new List<BuildingPreset>();
    [SerializeField] List<BuildingPreset> dictionaryBucket = new List<BuildingPreset>();
    [SerializeField] List<BuildingPreset> dictionaryHammer = new List<BuildingPreset>();
    [SerializeField] List<BuildingPreset> dictionaryKnife = new List<BuildingPreset>();
    [SerializeField] List<BuildingPreset> dictionaryFryingPan = new List<BuildingPreset>();
    public Dictionary<Tool, List<BuildingPreset>> buildingDictionary; 
    public Dictionary<string, BuildingPreset> presetDictionary;
    public ConstructionArea constructionArea;
    [SerializeField] BuildingPreset buildingPreset;
    public GameObject buildingsParent;
    public int lastID = 1;

    public AstarPath astarPath;

    private void Start() {
        buildingDictionary = new Dictionary<Tool, List<BuildingPreset>>();
        presetDictionary = new Dictionary<string, BuildingPreset>();
        List<Tool> tools = GameManager.Instance.tools;
        foreach (Tool tool in tools){
            switch (tool.name){
                case "Axe":
                    buildingDictionary.Add(tool, dictionaryAxe);
                    break;
                case "Lantern":
                    buildingDictionary.Add(tool, dictionaryLantern);
                    break;
                case "Shovel":
                    buildingDictionary.Add(tool, dictionaryShovel);
                    break;
                case "Pickaxe":
                    buildingDictionary.Add(tool, dictionaryPickaxe);
                    break;
                case "Hammer":
                    buildingDictionary.Add(tool, dictionaryHammer);
                    break;
                case "Bucket":
                    buildingDictionary.Add(tool, dictionaryBucket);
                    break;
                case "Knife":
                    buildingDictionary.Add(tool, dictionaryKnife);
                    break;
                case "FryingPan":
                    buildingDictionary.Add(tool, dictionaryFryingPan);
                    break;
                default:
                    break;
            }
        }
        foreach (var buildingDictionaryKeyAndValue in buildingDictionary){
            int index = 0;
            foreach (BuildingPreset buildingPreset in buildingDictionaryKeyAndValue.Value){
                buildingPreset.toolType = buildingDictionaryKeyAndValue.Key.name;
                buildingPreset.toolTypeIndex = index++;
                presetDictionary[buildingPreset.name] = buildingPreset;
            }
        }

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
        byte result = 0;
        switch (buildingPreset.toolType)
        {
            case "Bucket":
                result = 0;
                break;
            case "Knife":
                result = 1;
                break;
            case "Lantern":
                result = 2;
                break;
            case "Axe":
                result = 3;
                break;
            case "Shovel":
                result = 4;
                break;
            case "FryingPan":
                result = 5;
                break;
            case "Chisel":
                result = 6;
                break;
            case "Pickaxe":
                result = 7;
                break;
            default:
                break;
        }
        result = (byte)(result<<5);
        result += (byte)(buildingPreset.toolTypeIndex);
        return result;
    }

    public static BuildingPreset GetBuildingPreset(byte buildingCode){
        string result = "None";
        switch (buildingCode>>5)
        {
            case 0:
                result = "Bucket";
                break;
            case 1:
                result = "Knife";
                break;
            case 2:
                result = "Lantern";
                break;
            case 3:
                result = "Axe";
                break;
            case 4:
                result = "Shovel";
                break;
            case 5:
                result = "FryingPan";
                break;
            case 6:
                result = "Chisel";
                break;
            case 7:
                result = "Pickaxe";
                break;
            default:
                break;
        }
        
        return GetBuildingPreset(result,(int)(buildingCode&31));
    }

    public static BuildingPreset GetBuildingPreset(string ToolType, int index){
        BuildingManager buildingManager = GameManager.Instance.buildingManager;
        BuildingPreset result = null;
        switch (ToolType){
            // case "Bucket":
            //     result = dictionaryAxe[index];
            //     break;
            // case "Knife":
            //     result = 1;
            //     break;
            case "Lantern":
                result = buildingManager.dictionaryLantern[index];
                break;
            case "Axe":
                result = buildingManager.dictionaryAxe[index];
                break;
            case "Shovel":
                result = buildingManager.dictionaryShovel[index];
                break;
            case "FryingPan":
                result = buildingManager.dictionaryFryingPan[index];
                break;
            case "Knife":
                result = buildingManager.dictionaryKnife[index];
                break;
            // case "Chisel":
            //     result = 6;
            //     break;
            case "Pickaxe":
                result = buildingManager.dictionaryPickaxe[index];
                break;
            default:
                break;
        }
        return result;
    }

    public void SetBuildingPreset(BuildingPreset buildingData){
        this.buildingPreset = buildingData;
        constructionArea.SetBuildingData(buildingData);
    }

    public bool Build(){
        // 장애물이 있으면 못지어요
        if(constructionArea.isThereObstacle()){
            Debug.Log("There is Obstacles");
            return false;
        }
        // 무슨 건물 지을지 결정 안했으면 못지어요
        if(this.buildingPreset == null){
            Debug.Log("buildingPreset is null");
            return false;
        }
        // 재료가 충분한지 확인
        Inventory inventoryManager = GameManager.Instance.inventory;
        List<BuildingMaterial> materialList = buildingPreset.materialList;
        bool doWeHaveMaterialEnough = true;
        foreach (BuildingMaterial material in materialList){
            if(inventoryManager.GetItemAmount(material.name) < material.amount){
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
        foreach (BuildingMaterial material in materialList){
            inventoryManager.ConsumeItem(material.name,material.amount);
        }
        // 현재 플레이어 위치를 기준으로 건설을 한다
        Vector3 location = new Vector3();
        location.x = Mathf.Round(constructionArea.transform.position.x);
        location.y = Mathf.Round(constructionArea.transform.position.y);
        location.z = Mathf.Round(constructionArea.transform.position.z);

        Debug.Log("Build Position "+location);
        GameObject Built = Instantiate(constructure,location,Quaternion.identity);
        Built.transform.SetParent(buildingsParent.transform);

        // 해당 건물이 가질 Building Data를 생성한다
        BuildingObject BuiltObject = Built.GetComponent<BuildingObject>();
        BuildingData buildingData = BuiltObject.buildingData;
        buildingData.code = GetBuildingCode(buildingPreset);
        buildingData.id = lastID++;
        const int itemSpace = 4;
        buildingData.items = new ItemSlotData[itemSpace];
        for (int i = 0; i < itemSpace; i++){
            buildingData.items[i] = ItemSlotData.Create(ItemManager.GetItemPresetFromCode(0));
        }
        switch (buildingPreset.name){
            case "Forester":
                buildingData.mediocrityData = new ForesterData();
                break;
            case "Tent":
                buildingData.mediocrityData = new HouseData(4);
                break;
            default:
                break;
        }

        // DropItem을 설정
        if(buildingPreset.dropAmounts.Count > 0){
            Built.AddComponent<ItemDroper>().InitializeItemDrop(buildingPreset);
        }
        Hittable hittable = Built.GetComponent<Hittable>();
        Tool.ToolType toolType = Tool.ToolType.NONE;
        switch (buildingPreset.toolType){
            case "Axe":
                toolType = Tool.ToolType.AXE;
                break;
            case "Knife":
                toolType = Tool.ToolType.KNIFE;
                break;
            case "FryingPan":
                toolType = Tool.ToolType.FRYINGPAN;
                break;
            case "Pickaxe":
                toolType = Tool.ToolType.PICKAXE;
                break;
            default:
                break;
        }
        hittable.effectiveTool = toolType;
        astarPath.Scan();

        return true;
    }

}