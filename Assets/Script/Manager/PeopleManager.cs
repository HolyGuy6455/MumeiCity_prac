using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    public GameObject theMotherOfWholePeople;
    public GameObject normalPerson;
    public List<string> thinks;
    public int lastID = 1;
    [SerializeField] List<PersonBehavior> people;
    List<BuildingObject> houseInfomation;
    public List<BuildingObject> _houseInfomation{get{return houseInfomation;}}
    private void Start() {
        List<PersonBehavior> buildingList = GetWholePeopleList();
        foreach (PersonBehavior person in buildingList){
            person.personData.id = lastID++;
        }
    }
    public int GetThinkCode(string think){
        return think.IndexOf(think);
    }

    public List<PersonBehavior> GetWholePeopleList(){
        List<PersonBehavior> result = new List<PersonBehavior>();
        foreach (Transform childTransform in theMotherOfWholePeople.transform){
            PersonBehavior person = childTransform.GetComponent<PersonBehavior>();
            if(person != null){
                result.Add(person);
            }
        }
        people = result;
        return result;
    }

    public PersonBehavior FindPersonWithID(int id){
        foreach (PersonBehavior person in people){
            if(person.personData.id == id){
                return person;
            }
        }
        return null;
    }

    public void ResetHouseInfomation(){
        List<BuildingObject> houseList = GameManager.Instance.buildingManager.wholeBuildingList();
        houseList = houseList.FindAll(buildingObject => buildingObject.buildingData.buildingPreset.name == "Tent");
        List<PersonBehavior> people = GameManager.Instance.peopleManager.GetWholePeopleList();
        Debug.Log("집의 갯수는 "+ houseList.Count);

        foreach (PersonBehavior person in people){
            Debug.Log("제 아이디는 "+person.personData.id);
            if(person.personData.homeID != 0){
                BuildingObject house =  GameManager.Instance.buildingManager.FindBuildingObjectWithID(person.personData.homeID);
                // continue;
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
