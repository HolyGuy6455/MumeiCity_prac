using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    public GameObject theMotherOfWholePeople;
    public GameObject normalPerson;
    public List<string> thinks;
    public int lastID = 1;
    [SerializeField] List<PersonCommonAI> people;
    List<BuildingObject> houseInfomation;
    public List<BuildingObject> _houseInfomation{get{return houseInfomation;}}
    private void Start() {
        List<PersonCommonAI> buildingList = GetWholePeopleList();
        foreach (PersonCommonAI person in buildingList){
            person.personData.id = lastID++;
        }
    }
    public int GetThinkCode(string think){
        return think.IndexOf(think);
    }

    public List<PersonCommonAI> GetWholePeopleList(){
        List<PersonCommonAI> result = new List<PersonCommonAI>();
        foreach (Transform childTransform in theMotherOfWholePeople.transform){
            PersonCommonAI person = childTransform.GetComponent<PersonCommonAI>();
            if(person != null){
                result.Add(person);
            }
        }
        people = result;
        return result;
    }

    public void ResetHouseInfomation(){
        List<BuildingObject> houseList = GameManager.Instance.buildingManager.wholeBuildingList();
        houseList = houseList.FindAll(buildingObject => buildingObject.buildingData.buildingPreset.name == "Tent");
        List<PersonCommonAI> people = GameManager.Instance.peopleManager.GetWholePeopleList();
        Debug.Log("집의 갯수는 "+ houseList.Count);

        foreach (PersonCommonAI person in people){
            Debug.Log("제 아이디는 "+person.personData.id);
            if(person.personData.homeID != 0){
                continue;
            }
            Debug.Log("저는 집이 없어요");
            foreach (BuildingObject house in houseList){
                if(!(house.buildingData.mediocrityData is HouseData)){
                    Debug.Log("여기엔 집의 정보가 없어요");
                    continue;
                }
                HouseData houseData = house.buildingData.mediocrityData as HouseData;
                for (int i = 0; i < houseData.personIDList.Length; i++){

                    if(houseData.personIDList[i] == 0){
                        Debug.Log("집을 찾았어요 : " + house.buildingData.id);
                        houseData.personIDList[i] = person.personData.id;
                        person.personData.homeID = house.buildingData.id;
                        break;
                    }
                }
            }
        }


        //     delegate(BuildingObject buildingObject){
        //         // BuildingData buildingData = buildingObject.buildingData;
        //         // if(buildingData.buildingPreset.name != "Tent"){
        //         //     return false;
        //         // }
        //         // HouseData houseData = buildingData.mediocrityData as HouseData;
        //         // for (int i = 0; i < houseData.personIDList.Length; i++){
        //         //     if(houseData.personIDList[i] == 0){
        //         //         return true;
        //         //     }
        //         // }
        //         // return false;
        //         return (buildingObject.buildingData.buildingPreset.name == "Tent");
        //     }
        // );
    }
}
