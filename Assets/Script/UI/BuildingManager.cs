using UnityEngine;
using System.Collections.Generic;

/*
 * 건설 관련된 내용 전체를 관리하는 객체
 */
public class BuildingManager : MonoBehaviour {
    public GameManager.UpdateUI onToolChangedCallback;
    public GameObject Building;
    public List<BuildingPreset> dictionaryAxe = new List<BuildingPreset>();
    public List<BuildingPreset> dictionaryLantern = new List<BuildingPreset>();
    public List<BuildingPreset> dictionaryShovel = new List<BuildingPreset>();
    public Dictionary<Tool, List<BuildingPreset>> buildingDictionary;
    public ConstructionArea constructionArea;
    public BuildingPreset buildingPreset;
    [SerializeField]
    private HashSet<GameObject> _wholeBuildingSet = new HashSet<GameObject>();
    public HashSet<GameObject> wholeBuildingSet{get{return _wholeBuildingSet;}}
    public GameObject buildingsParents;

    private void Start() {
        buildingDictionary = new Dictionary<Tool, List<BuildingPreset>>();
        List<Tool> tools = GameManager.Instance.tools;
        foreach (Tool tool in tools)
        {
            switch (tool.ToolName)
            {
                case "Axe":
                    buildingDictionary.Add(tool, dictionaryAxe);
                    break;
                case "Lantern":
                    buildingDictionary.Add(tool, dictionaryLantern);
                    break;
                case "Shovel":
                    buildingDictionary.Add(tool, dictionaryShovel);
                    break;
                default:
                    break;
            }
        }
        foreach (var buildingDictionaryKeyAndValue in buildingDictionary){
            int index = 0;
            foreach (BuildingPreset buildingPreset in buildingDictionaryKeyAndValue.Value)
            {
                buildingPreset.toolType = buildingDictionaryKeyAndValue.Key.name;
                buildingPreset.toolTypeIndex = index++;
                Debug.Log(buildingPreset.name);
                Debug.Log(buildingPreset.toolType);
                Debug.Log(buildingPreset.toolTypeIndex);
            }
        }
        foreach (Transform childTransform in buildingsParents.transform)
        {
            _wholeBuildingSet.Add(childTransform.gameObject);
        }
    }

    public byte GetBuildingCode(BuildingPreset buildingPreset){
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
            case "Frying Pan":
                result = 5;
                break;
            case "Chisel":
                result = 6;
                break;
            case "Pickax":
                result = 7;
                break;
            default:
                break;
        }
        result = (byte)(result<<5);
        result += (byte)(buildingPreset.toolTypeIndex);
        return result;
    }

    public BuildingPreset GetBuildingPreset(byte buildingCode){
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
                result = "Frying Pan";
                break;
            case 6:
                result = "Chisel";
                break;
            case 7:
                result = "Pickax";
                break;
            default:
                break;
        }
        
        return GetBuildingPreset(result,(int)(buildingCode&31));
    }

    public BuildingPreset GetBuildingPreset(string ToolType, int index){
        BuildingPreset result = null;
        switch (ToolType)
        {
            // case "Bucket":
            //     result = dictionaryAxe[index];
            //     break;
            // case "Knife":
            //     result = 1;
            //     break;
            case "Lantern":
                result = dictionaryLantern[index];
                break;
            case "Axe":
                result = dictionaryAxe[index];
                break;
            case "Shovel":
                result = dictionaryShovel[index];
                break;
            // case "Frying Pan":
            //     result = 5;
            //     break;
            // case "Chisel":
            //     result = 6;
            //     break;
            // case "Pickax":
            //     result = 7;
            //     break;
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
        if(constructionArea.isThereObstacle()){
            return false;
        }

        InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        List<(string,int)> materialList = buildingPreset.GetMaterials();
        bool doWeHaveMaterialEnough = true;
        foreach ((string itemName, int itemAmount) material in materialList){
            if(inventoryManager.GetItemAmount(material.itemName) < material.itemAmount){
                doWeHaveMaterialEnough = false;
                Debug.Log("Not Enough Material");
                break;
            }
        }
        if(doWeHaveMaterialEnough == false){
            return false;
        }
        foreach ((string itemName, int itemAmount) material in materialList){
            inventoryManager.ConsumeItem(material.itemName,material.itemAmount);
        }

        Transform PlayerTransform = GameManager.Instance.PlayerTransform;
        Vector3 location = new Vector3();
        location.x = Mathf.Round(PlayerTransform.position.x+buildingPreset.relativeLocation.x);
        location.y = Mathf.Round(PlayerTransform.position.y+buildingPreset.relativeLocation.y);
        location.z = Mathf.Round(PlayerTransform.position.z+buildingPreset.relativeLocation.z);
        GameObject Built =  Instantiate(Building,location,Quaternion.identity);

        BuildingObject BuiltObject = Built.GetComponent<BuildingObject>();
        BuildingData buildingData = new BuildingData();
        buildingData.locationX = ((int)location.x);
        buildingData.locationY = ((int)location.y);
        buildingData.buildingCode = GetBuildingCode(buildingPreset);
        BuiltObject.Initialize(buildingData);

        return true;
    }
}