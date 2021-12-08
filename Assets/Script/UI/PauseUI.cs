using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PauseUI : MonoBehaviour{
    [TextArea] public string savefile;
    [SerializeField] string _fileName = "01.save";
    class SaveForm {
        public List<BuildingData> buildingDatas = new List<BuildingData>();
    }


    public void SaveTheGame(){
        SaveForm saveForm = new SaveForm();

        GameObject buildingsParent = GameManager.Instance.buildingManager.buildingsParent;
        foreach (Transform childTransform in buildingsParent.transform){
            BuildingData buildingData = childTransform.GetComponent<BuildingObject>().buildingData;
            saveForm.buildingDatas.Add(buildingData);
        }
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

        // 일단 있는 건물들을 전부 없앤다
        GameObject buildingsParent = GameManager.Instance.buildingManager.buildingsParent;
        foreach (Transform childTransform in buildingsParent.transform){
            Destroy(childTransform.gameObject);
        }
        // string에서 데이터를 읽어들임
        SaveForm saveForm = JsonUtility.FromJson<SaveForm>(savefile);
        GameManager.Instance.buildingManager.LoadBuilding(saveForm.buildingDatas);
    }
}
