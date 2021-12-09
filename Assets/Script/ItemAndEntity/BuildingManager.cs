using UnityEngine;
using System.Collections.Generic;

/*
 * 건설 관련된 내용 전체를 관리하는 객체
 */
public class BuildingManager : MonoBehaviour {
    public GameManager.UpdateUI onToolChangedCallback;
    public GameObject constructure;
    public List<BuildingPreset> dictionaryAxe = new List<BuildingPreset>();
    public List<BuildingPreset> dictionaryLantern = new List<BuildingPreset>();
    public List<BuildingPreset> dictionaryShovel = new List<BuildingPreset>();
    public Dictionary<Tool, List<BuildingPreset>> buildingDictionary;
    public ConstructionArea constructionArea;
    public BuildingPreset buildingPreset;
    public GameObject buildingsParent;

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
    }

    public List<GameObject> wholeBuildingSet(){
        List<GameObject> result = new List<GameObject>();
        foreach (Transform childTransform in buildingsParent.transform){
            BuildingObject buildingObject = childTransform.GetComponent<BuildingObject>();
            result.Add(childTransform.gameObject);
        }
        return result;
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
        GameObject Built =  Instantiate(constructure,location,Quaternion.identity);
        Built.transform.SetParent(buildingsParent.transform);
        // 해당 건물이 가질 Building Data를 생성한다
        BuildingObject BuiltObject = Built.GetComponent<BuildingObject>();
        BuildingData buildingData = new BuildingData();
        buildingData.positionX = ((int)location.x);
        buildingData.positionY = ((int)location.y);
        buildingData.positionZ = ((int)location.z);
        buildingData.code = GetBuildingCode(buildingPreset);
        BuiltObject.Initialize(buildingData);

        // DropItem을 설정
        if(buildingPreset.dropAmounts.Count > 0){
            ItemDroper itemDroper = Built.AddComponent<ItemDroper>();
            foreach (ItemDropAmount dropItem in buildingPreset.dropAmounts){
                itemDroper.Add(dropItem);
            }
        }

        return true;
    }

}