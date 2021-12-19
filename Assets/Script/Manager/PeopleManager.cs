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
    private void Start() {
        List<PersonCommonAI> buildingList = wholePeopleList();
        foreach (PersonCommonAI person in buildingList){
            person.personData.id = lastID++;
        }
    }
    public int getThinkCode(string think){
        return think.IndexOf(think);
    }

    public List<PersonCommonAI> wholePeopleList(){
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
}
