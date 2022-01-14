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
    [SerializeField] int staminaMax;
    [SerializeField] int staminaEnough;
    [SerializeField] int staminaHunger;
    [SerializeField] int[] happinessStep;
    public int _staminaMax{get{return staminaMax;}}
    public int _staminaEnough{get{return staminaEnough;}}
    public int _staminaHunger{get{return staminaHunger;}}
    public int[] _happinessStep{get{return happinessStep;}}
    private void Start() {
        List<PersonBehavior> buildingList = GetWholePeopleList();
        foreach (PersonBehavior person in buildingList){
            person.personData.id = lastID++;
        }
    }
    public int GetThinkCode(string think){
        return think.IndexOf(think);
    }

    public static List<PersonBehavior> GetWholePeopleList(){
        PeopleManager peopleManager = GameManager.Instance.peopleManager;
        List<PersonBehavior> result = new List<PersonBehavior>();
        foreach (Transform childTransform in peopleManager.theMotherOfWholePeople.transform){
            PersonBehavior person = childTransform.GetComponent<PersonBehavior>();
            if(person != null){
                result.Add(person);
            }
        }
        peopleManager.people = result;
        return result;
    }

    public static PersonBehavior FindPersonWithID(int id){
        PeopleManager peopleManager = GameManager.Instance.peopleManager;
        foreach (PersonBehavior person in peopleManager.people){
            if(person.personData.id == id){
                return person;
            }
        }
        return null;
    }

    // 매일 아침마다 단체로 이사를 한다
    public void ResetHouseInfomation(){
        List<BuildingObject> houseList = GameManager.Instance.buildingManager.wholeBuildingList();
        houseList = houseList.FindAll(buildingObject => buildingObject.buildingData.mediocrityData is HouseData);
        List<PersonBehavior> people = PeopleManager.GetWholePeopleList();
        // Debug.Log("집의 갯수는 "+ houseList.Count);

        foreach (PersonBehavior person in people){
            // Debug.Log("제 아이디는 "+person.personData.id);
            if(person.personData.homeID != 0){
                BuildingObject house =  GameManager.Instance.buildingManager.FindBuildingObjectWithID(person.personData.homeID);
                // Debug.Log("제 집은 여기예요 "+house);
                continue;
            }
            // Debug.Log("저는 집이 없어요");
            foreach (BuildingObject house in houseList){
                HouseData houseData = house.buildingData.mediocrityData as HouseData;
                for (int i = 0; i < houseData.personIDList.Length; i++){
                    if(houseData.personIDList[i] == 0){
                        // Debug.Log("집을 찾았어요 : " + house.buildingData.id);
                        houseData.personIDList[i] = person.personData.id;
                        person.personData.homeID = house.buildingData.id;
                        break;
                    }
                }
            }
        }
    }

    // 매일 아침마다 구인구직활동을 한다
    // 잘 작동 안하길래 일단 막아둠
    public void OfferJob(){
        // List<BuildingObject> buildingObjects = GameManager.Instance.buildingManager.wholeBuildingList();
        // foreach (BuildingObject buildingObject in buildingObjects){
            // buildingObject.HirePerson();
        // }
    }

    // 행복위원회입니다 댁은 행복하신지요
    // 행복수치를 조사한다. 사실 조사가 아니라 결산이지만
    public void SurveyHappiness(){
        // 일단 사람들 행복수치 측정
        List<PersonBehavior> people = PeopleManager.GetWholePeopleList();
        foreach (PersonBehavior person in people){
            if(person.personData.stamina > staminaEnough){
                person.personData.happiness += 10;
            }else if(person.personData.stamina < staminaHunger){
                person.personData.happiness -= 10;
            }
        }
        // 행복한 사람이 둘 이상 있는 집을 찾아 애를 낳게 한다
        List<BuildingObject> houseList = GameManager.Instance.buildingManager.wholeBuildingList();
        houseList = houseList.FindAll(buildingObject => buildingObject.buildingData.mediocrityData is HouseData);
        foreach (BuildingObject house in houseList){
            HouseData houseData = house.buildingData.mediocrityData as HouseData;
            if(houseData == null){
                continue;
            }
            PersonBehavior happyPerson_1st = null;
            foreach (int personID in houseData.personIDList){
                PersonBehavior person = FindPersonWithID(personID);
                if(person == null){
                    continue;
                }
                if(person.personData.happiness > 80){
                    if(happyPerson_1st == null){
                        happyPerson_1st = person;
                    }else{
                        happyPerson_1st.personData.happiness -= 50;
                        person.personData.happiness -= 50;
                        Vector3 location = house.gameObject.transform.position;

                        GameObject personObject = Instantiate(normalPerson,location,Quaternion.identity);
                        PersonBehavior newBorn = personObject.GetComponent<PersonBehavior>();
                        newBorn.personData.id = lastID++;
                        personObject.transform.SetParent(GameManager.Instance.peopleManager.theMotherOfWholePeople.transform);
                        happyPerson_1st = null;
                    }
                }
            }
        }
    }
}
