using System.Collections.Generic;
using UnityEngine;

public class WorkPlace : MonoBehaviour{
    [SerializeField] GameManager.GameTab gameTab;
    [SerializeField] BuildingObject buildingObject;
    [SerializeField] TimeEventQueueTicket hiringEvent;
    [SerializeField] bool hiringPerson;
    [SerializeField] int jobID;
    public List<TaskInfo> taskInfos;
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
            people = people.FindAll(person => (person != null)&&((person as PersonBehavior).personData.workplaceID == 0) );
            if(people.Count > 0){
                people[0].personData.workplaceID = this.buildingObject.buildingData.id;
                this.buildingObject.buildingData.workerID = people[0].personData.id;
                people[0].personData.jobID = jobID;
                people[0].UpdateHatImage();
            }
        }
        buildingObject.buildingData.mediocrityData.SaveMediocrityData();
        return false;
    }

    public void ShowWindow(){
        GameManager.Instance.interactingBuilding = this.buildingObject;
        GameManager.Instance.ChangeGameTab(gameTab);
    }
}