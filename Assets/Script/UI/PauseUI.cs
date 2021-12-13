using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PauseUI : MonoBehaviour{
    [TextArea] public string savefile;
    [SerializeField] string _fileName = "01.save";
    [SerializeField] GameObject itemPickupPrefab;
    class SaveForm {
        public Vector3 playerPosition = new Vector3();
        public List<BuildingData> buildingDatas = new List<BuildingData>();
        public List<ItemPickupData> itemPickupDatas = new List<ItemPickupData>();
        public List<PersonData> personDatas = new List<PersonData>();
    }


    public void SaveTheGame(){
        SaveForm saveForm = new SaveForm();
        // 플레이어 위치 저장하기
        saveForm.playerPosition = GameManager.Instance.PlayerTransform.position;
        // 건물 설정 저장하기
        GameObject buildingsParent = GameManager.Instance.buildingManager.buildingsParent;
        foreach (Transform childTransform in buildingsParent.transform){
            BuildingData buildingData = childTransform.GetComponent<BuildingObject>().buildingData;
            saveForm.buildingDatas.Add(buildingData);
        }
        // 아이템 위치 저장하기
        GameObject itemPickupParent = GameManager.Instance.itemPickupParent;
        foreach (Transform childTransform in itemPickupParent.transform){
            ItemPickupData itemPickupData = childTransform.GetComponent<ItemPickup>().item;
            itemPickupData.position = childTransform.position;
            saveForm.itemPickupDatas.Add(itemPickupData);
        }
        GameObject peopleParent = GameManager.Instance.peopleManager.theMotherOfWholePeople;
        foreach (Transform childTransform in peopleParent.transform){
            PersonData personData = childTransform.GetComponent<PersonCommonAI>().personData;
            personData.position = childTransform.position;
            saveForm.personDatas.Add(personData);
        }

        // 실제 파일로 저장
        savefile = JsonUtility.ToJson(saveForm,true);
        try{
            // C:\Users\사용자\AppData\LocalLow\DefaultCompany
            string path = Application.persistentDataPath + "/" + _fileName;
            File.WriteAllText(path, savefile);
        }
        catch (FileNotFoundException e){
            Debug.Log("The file was not found:" + e.Message);
        }
        catch (DirectoryNotFoundException e){
            Debug.Log("The directory was not found: " + e.Message);
        }
        catch (IOException e){
            Debug.Log("The file could not be opened:" + e.Message);
        }
    }

    public void LoadTheGame(){
        try{
            string path = Application.persistentDataPath + "/" + _fileName;
            if (File.Exists(path)){
                savefile = File.ReadAllText(path);
            }
        }
        catch (FileNotFoundException e){
            Debug.Log("The file was not found:" + e.Message);
        }
        catch (DirectoryNotFoundException e){
            Debug.Log("The directory was not found: " + e.Message);
        }
        catch (IOException e){
            Debug.Log("The file could not be opened:" + e.Message);
        }
        // string에서 데이터를 읽어들임
        SaveForm saveForm = JsonUtility.FromJson<SaveForm>(savefile);

        GameManager.Instance.PlayerTransform.position = saveForm.playerPosition;

        // 맵에 남아있는 건물들을 전부 없앤다
        GameObject buildingsParent = GameManager.Instance.buildingManager.buildingsParent;
        List<GameObject> wholeBuildingSet = GameManager.Instance.buildingManager.wholeBuildingSet();
        foreach (Transform childTransform in buildingsParent.transform){
            Destroy(childTransform.gameObject);
        }
        foreach (BuildingData data in saveForm.buildingDatas){
            // 데이터로부터 프리셋과 위치를 읽어온다
            BuildingPreset buildingPreset = data.buildingPreset;
            Vector3 location = new Vector3();
            location.x = data.positionX;
            location.y = data.positionY;
            location.z = data.positionZ;
            // 건물을 생성하고 데이터로 초기화한다
            GameObject Built =  Instantiate(GameManager.Instance.buildingManager.constructure,location,Quaternion.identity);
            BuildingObject BuiltObject = Built.GetComponent<BuildingObject>();
            Built.transform.SetParent(buildingsParent.transform);
            BuiltObject.Initialize(data);
            wholeBuildingSet.Add(Built);
            // DropItem을 설정
            if(buildingPreset.dropAmounts.Count > 0){
                ItemDroper itemDroper = Built.AddComponent<ItemDroper>();
                foreach (ItemDropAmount dropItem in buildingPreset.dropAmounts){
                    itemDroper.Add(dropItem);
                }
            }
        }

        // 맵에 남아있는 아이템을 전부 없앤다
        foreach (Transform childTransform in GameManager.Instance.itemPickupParent.transform){
            Destroy(childTransform.gameObject);
        }

        foreach (ItemPickupData data in saveForm.itemPickupDatas){
            // 데이터로부터 위치를 읽어온다
            Vector3 location = new Vector3();
            location = data.position;
            // 아이템 데이터로 픽업아이템을 생성한다
            GameObject itemObject = Instantiate(itemPickupPrefab,location,Quaternion.identity);
            ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
            itemPickup.item = data;
            itemPickup.IconSpriteUpdate();
            itemObject.transform.SetParent(GameManager.Instance.itemPickupParent.transform);
        }

        foreach (Transform childTransform in GameManager.Instance.peopleManager.theMotherOfWholePeople.transform){
            Destroy(childTransform.gameObject);
        }

        foreach (PersonData data in saveForm.personDatas){
            // 데이터로부터 위치를 읽어온다
            Vector3 location = new Vector3();
            location = data.position;
            // 데이터로 시민을 생성한다
            GameObject itemObject = Instantiate(GameManager.Instance.peopleManager.normalPerson,location,Quaternion.identity);
            PersonCommonAI personCommonAI = itemObject.GetComponent<PersonCommonAI>();
            personCommonAI.personData = data;
            personCommonAI.UpdateItemView();
            itemObject.transform.SetParent(GameManager.Instance.peopleManager.theMotherOfWholePeople.transform);
        }

    }
}
