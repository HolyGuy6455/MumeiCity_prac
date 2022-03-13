using System.Collections.Generic;
using UnityEngine;

public class WorkPlace : MonoBehaviour, ITiemEventRebindInfo{
    [SerializeField] GameManager.GameTab gameTab;
    [SerializeField] BuildingObject buildingObject;
    [SerializeField] TimeEventQueueTicket hiringEvent;
    public bool hiringPerson;
    [SerializeField] int jobID;
    public List<TaskInfo> taskInfos;
    public GameManager.GameTab _gameTab{get{return gameTab;}}
    private void Start() {
        buildingObject = this.GetComponent<BuildingObject>();
        if(hiringPerson){
            string ticketName = "building_"+this.buildingObject.buildingData.id+"_hire";
            hiringEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1, ticketName, HirePerson);
        }
    }

    private bool HirePerson(string ticketName){
        if(hiringPerson && buildingObject.buildingData.workerID == 0){
            List<PersonBehavior> people = PeopleManager.GetWholePeopleList();
            people = people.FindAll(
                delegate(PersonBehavior person){
                    if(person == null){
                        return false;
                    }
                    if(person.personData.growth < 1.0f){
                        return false;
                    }
                    if(person.personData.workplaceID != 0){
                        return false;
                    }
                    return true;
                }
            );
            if(people.Count > 0){
                people[0].personData.workplaceID = this.buildingObject.buildingData.id;
                this.buildingObject.buildingData.workerID = people[0].personData.id;
                people[0].personData.jobID = jobID;
                people[0].UpdateHatImage();
            }
        }
        // buildingObject.buildingData.mediocrityData.SaveMediocrityData();
        return false;
    }

    public void ShowWindow(){
        GameManager.Instance.interactingBuilding = this.buildingObject;
        GameManager.Instance.ChangeGameTab(gameTab);
    }

    public void Interact(){
        GameManager.Instance.ChangeGameTab(gameTab);
    }

    public Dictionary<string, TimeManager.TimeEvent> GetDictionary(){
        Dictionary<string, TimeManager.TimeEvent> result = new Dictionary<string, TimeManager.TimeEvent>();

        BuildingObject buildingObject = this.GetComponent<BuildingObject>();
        string ticketName = "building_"+buildingObject.buildingData.id+"_hire";
        result[ticketName] = HirePerson;

        return result;
    }

    public void Dismiss(){
        PersonBehavior person = PeopleManager.FindPersonWithID(this.buildingObject.buildingData.workerID);
        person.personData.workplaceID = 0;
        person.personData.jobID = 0;
        person.UpdateHatImage();
    }

}