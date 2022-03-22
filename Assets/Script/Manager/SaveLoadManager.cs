using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour{
    [TextArea] public string savefile;
    // [SerializeField] string _fileName = "01.save";
    [SerializeField] GameObject itemPickupPrefab;
    public ManufacturerUI manufacturerUI;
    public LaboratoryUI laboratoryUI;
    public PauseUI pauseUI;
    private Dictionary<string, TimeManager.TimeEvent> rebindDictionary;
    class SaveForm {
        public int timeValue;
        public Vector3 playerPosition = new Vector3();
        public ItemSlotData[] itemData;
        public List<BuildingData> buildingDatas = new List<BuildingData>();
        public List<ItemPickupData> itemPickupDatas = new List<ItemPickupData>();
        public List<PersonData> peopleData = new List<PersonData>();
        public List<AnimalData> animalsData = new List<AnimalData>();
        public int lastBuildingID;
        public int lastPersonID;
        public List<TimeEventQueueTicket> timeEventQueueTickets;
        public List<Achievement> achievements;
    }
    SaveMetaFile saveMetaFile;
    public SaveMetaFile _saveMetaFile{
        get{
            if(saveMetaFile is null){
                saveMetaFile = new SaveMetaFile();
                saveMetaFile.metaName = new List<string>();
                saveMetaFile.metaName.Add( "Wait, you can't access this index.");
                saveMetaFile.metaName.Add("None");
                saveMetaFile.metaName.Add("None");
                saveMetaFile.metaName.Add("None");
            }
            return saveMetaFile;
        }
    }
    [Serializable]
    public class SaveMetaFile{
        public List<string> metaName;
    }

    private void Awake() {
        LoadTheMeta();
    }

    public void SetMetaTitle(int index){
        _saveMetaFile.metaName[index] = DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss"));
        SaveTheMeta();
        pauseUI.UpdateUI();
    }

    public void SaveTheMeta(){
        string metaStr = JsonUtility.ToJson(saveMetaFile,true);
        try{
            // C:\Users\사용자\AppData\LocalLow\DefaultCompany
            string path = Application.persistentDataPath + "/" + "meta.json";
            File.WriteAllText(path, metaStr);
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

    public void LoadTheMeta(){
        string metaStr = "";
        try{
            string path = Application.persistentDataPath + "/" + "meta.json";
            if (File.Exists(path)){
                metaStr = File.ReadAllText(path);
            }
        }
        catch (FileNotFoundException e){
            Debug.Log("The file was not found:" + e.Message);
            SaveTheMeta();
            return;
        }
        catch (DirectoryNotFoundException e){
            Debug.Log("The directory was not found: " + e.Message);
        }
        catch (IOException e){
            Debug.Log("The file could not be opened:" + e.Message);
        }

        saveMetaFile = JsonUtility.FromJson<SaveMetaFile>(metaStr);
        pauseUI.UpdateUI();
    }


    public void SaveTheGame(string fileName){
        SaveForm saveForm = new SaveForm();

        // 시간을 저장
        saveForm.timeValue = GameManager.Instance.timeManager._timeValue;
        // 플레이어 위치 저장하기
        saveForm.playerPosition = GameManager.Instance.PlayerTransform.position;
        // 플레이어 아이템 저장하기
        saveForm.itemData = GameManager.Instance.inventory.itemData;
        // 건물, 사람의 할당되지 않은 마지막 ID 저장하기
        saveForm.lastBuildingID = GameManager.Instance.buildingManager.lastID;
        saveForm.lastPersonID = GameManager.Instance.peopleManager.lastID;
        // 건물 설정 저장하기
        GameObject[] buildingObjects = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject buildingObj in buildingObjects){
            BuildingData buildingData = buildingObj.GetComponent<BuildingObject>().buildingData;
            if(buildingData.facilityFunction != null)
                buildingData.facilityFunction.SaveMediocrityData(buildingData);
            saveForm.buildingDatas.Add(buildingData);
        }
        // 아이템 위치 저장하기
        GameObject itemPickupParent = GameManager.Instance.itemManager.itemPickupParent;
        foreach (Transform childTransform in itemPickupParent.transform){
            ItemPickupData itemPickupData = childTransform.GetComponent<ItemPickup>().itemPickupData;
            itemPickupData.position = childTransform.position;
            saveForm.itemPickupDatas.Add(itemPickupData);
        }

        // 사람들 위치 저장하기
        GameObject[] peopleObjects = GameObject.FindGameObjectsWithTag("Person");
        foreach (GameObject personObj in peopleObjects){
            PersonData personData = personObj.GetComponent<PersonBehavior>().personData;
            personData.position = personObj.transform.position;
            saveForm.peopleData.Add(personData);
        }

        // 동물들 위치 저장하기
        GameObject[] animalObjects = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject animalObj in animalObjects){
            AnimalBehavior animalBehavior = animalObj.GetComponent<AnimalBehavior>();
            Debug.Log("animal " + (animalBehavior));
            Debug.Log("is it null? " + (animalBehavior is null));
            Debug.Log("gameObj " + (animalObj));
            Debug.Log("animalData " + (animalBehavior.animalData is null));
            Debug.Log("position " + animalBehavior.animalData.position);
            animalBehavior.animalData.position = animalBehavior.transform.position;
            saveForm.animalsData.Add(animalBehavior.animalData);
        }

        // 타임티켓이랑 진행과제 저장
        saveForm.timeEventQueueTickets = GameManager.Instance.timeManager._waitingList;
        saveForm.achievements = GameManager.Instance.achievementManager.GenerateSaveForm();

        // 실제 파일로 저장
        savefile = JsonUtility.ToJson(saveForm,true);
        try{
            // C:/Users/user/AppData/LocalLow/HolyGuy6455/MumeiNoToshi
            string path = Application.persistentDataPath + "/" + fileName;
            Debug.Log("path : " + path);
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

    public void LoadTheGame(string fileName){
        try{
            string path = Application.persistentDataPath + "/" + fileName;
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

        // 새로 생성한 객체들과 끊어진 타임 티켓과 연결시켜줄 녀석
        rebindDictionary = new Dictionary<string, TimeManager.TimeEvent>();

        // 시간을 불러온다
        GameManager.Instance.timeManager._timeValue = saveForm.timeValue;
        // 플레이어의 위치를 불러온다
        GameManager.Instance.PlayerTransform.position = saveForm.playerPosition;
        // 플레이어 아이템을 불러온다
        GameManager.Instance.inventory.itemData = saveForm.itemData;

        // 할당되지 않은 건물, 사람 마지막 ID 읽어온다
        GameManager.Instance.buildingManager.lastID = saveForm.lastBuildingID;
        GameManager.Instance.peopleManager.lastID = saveForm.lastPersonID;

        // 맵에 남아있는 건물들을 전부 없앤다
        GameObject[] buildingObjects = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject buildingObj in buildingObjects){
            Destroy(buildingObj);
        }
        GameObject buildingsParent = GameManager.Instance.buildingManager.buildingsParent;
        GameObject buildingsPrefab = GameManager.Instance.buildingManager.constructureSample;
        foreach (BuildingData buildingData in saveForm.buildingDatas){
            // 데이터로부터 프리셋과 위치를 읽어온다
            BuildingPreset buildingPreset = buildingData.buildingPreset;
            Vector3 location = new Vector3();
            location.x = buildingData.positionX;
            location.y = buildingData.positionY;
            location.z = buildingData.positionZ;

            // 건물을 생성하고 데이터로 초기화한다
            GameObject Built;
            if(buildingPreset.prefab != null){
                Built = Instantiate(buildingPreset.prefab,location,Quaternion.identity);
            }else{
                Built = Instantiate(buildingsPrefab,location,Quaternion.identity);
            }
            Built.transform.SetParent(buildingsParent.transform);
            BuildingObject buildingObject = Built.GetComponent<BuildingObject>();
            
            buildingObject.Initialize(buildingData);

            AddRebindInfo<Restoration>(Built);
            AddRebindInfo<WorkPlace>(Built);

            WorkPlace workPlace = Built.GetComponent<WorkPlace>();
            if(workPlace != null){
                switch (workPlace._gameTab){
                    case GameManager.GameTab.MANUFACTURER:
                        for (int i = 0; i < 3; i++){
                            string ticketName = "building_"+buildingObject.buildingData.id+"_make_"+i;
                            AddRebindInfo(ticketName, manufacturerUI.ManufactureComplete);
                        }
                        break;
                    case GameManager.GameTab.LABORATORY:
                        for (int i = 0; i < 3; i++){
                            string ticketName = "building_"+buildingObject.buildingData.id+"_research_"+i;
                            AddRebindInfo(ticketName, manufacturerUI.ManufactureComplete);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        int lastItemID = 1;

        // 맵에 남아있는 아이템을 전부 없앤다
        foreach (Transform childTransform in GameManager.Instance.itemManager.itemPickupParent.transform){
            Destroy(childTransform.gameObject);
        }

        foreach (ItemPickupData data in saveForm.itemPickupDatas){
            // 데이터로부터 위치를 읽어온다
            Vector3 location = new Vector3();
            location = data.position;
            // 아이템 데이터로 픽업아이템을 생성한다
            GameObject itemObject = Instantiate(itemPickupPrefab,location,Quaternion.identity);
            ItemPickup itemPickup = itemObject.GetComponent<ItemPickup>();
            itemPickup.itemPickupData = data;
            itemPickup.IconSpriteUpdate();
            itemObject.transform.SetParent(GameManager.Instance.itemManager.itemPickupParent.transform);

            AddRebindInfo<ItemPickup>(itemObject);
            if(itemPickup.itemPickupData.ID > lastItemID){
                lastItemID = itemPickup.itemPickupData.ID;
            }
        }
        GameManager.Instance.itemManager.lastID = lastItemID;

        // 맵에 남아있는 사람을 전부 없앤다. 학살의 시간이다!
        GameObject[] peopleObjects = GameObject.FindGameObjectsWithTag("Person");
        foreach (GameObject personObj in peopleObjects){
            Destroy(personObj);
        }
        GameObject normalPerson = GameManager.Instance.peopleManager.normalPerson;
        foreach (PersonData data in saveForm.peopleData){
            // 데이터로부터 위치를 읽어온다
            Vector3 location = new Vector3();
            location = data.position;
            // 데이터로 시민을 생성한다
            GameObject personObject = Instantiate(normalPerson,location,Quaternion.identity);
            PersonBehavior person = personObject.GetComponent<PersonBehavior>();
            person.personData = data;
            person.UpdateItemView();
            personObject.transform.SetParent(GameManager.Instance.peopleManager.theMotherOfWholePeople.transform);
            person.UpdateHatImage();
        }

        // 맵에 남아있는 동물을 전부 없앤다.
        GameObject[] animalObjects = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject animalObj in animalObjects){
            Destroy(animalObj);
        }

        foreach (AnimalData data in saveForm.animalsData){
            // 데이터로부터 위치를 읽어온다
            Vector3 location = data.position;
            // 데이터로 동물을 생성한다
            GameObject animalPrefab = GameManager.Instance.mobManager.GetPrefab(data.animalName);
            GameObject animalObject = Instantiate(animalPrefab,location,Quaternion.identity);
            AnimalBehavior animalBehavior = animalObject.GetComponent<AnimalBehavior>();
            animalBehavior.animalData = data;
            animalObject.transform.SetParent(GameManager.Instance.mobManager.theMotherOfNature.transform);
        }

        PlayerMovement playerMovement = GameManager.Instance.playerMovement;
        AddRebindInfo("Player_Stemina_Recharge", playerMovement.SteminaRecharge);

        // 타임 티켓 복구
        List<TimeEventQueueTicket> tickets = saveForm.timeEventQueueTickets;
        foreach (TimeEventQueueTicket ticket in tickets){
            if(rebindDictionary.ContainsKey(ticket._idString)){
                ticket._timeEvent = rebindDictionary[ticket._idString];
            }else{
                Debug.Log("ticket._idString - " + ticket._idString);
            }
        }
        // 진행과제 복구
        GameManager.Instance.achievementManager.ReloadSaveform(saveForm.achievements);

        GameManager.Instance.timeManager._waitingList = tickets;
    }

    void AddRebindInfo<T>(GameObject gameObject) where T : MonoBehaviour, ITiemEventRebindInfo{
        T objNeedsRebind = gameObject.GetComponent<T>();
        if(objNeedsRebind == null){
            return;
        }
        Dictionary<string, TimeManager.TimeEvent> rebindInfos = objNeedsRebind.GetDictionary();
        foreach (string key in rebindInfos.Keys){
            rebindDictionary[key] = rebindInfos[key];
        }
    }

    void AddRebindInfo(string key, TimeManager.TimeEvent timeEvent){
        rebindDictionary[key] = timeEvent;
    }
}
