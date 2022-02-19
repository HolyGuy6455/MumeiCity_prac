using System.Collections.Generic;
using UnityEngine;

public class Restoration : MonoBehaviour{
    [SerializeField] TimeEventQueueTicket restoreEvent;
    [SerializeField] int restorationTerm;
    [SerializeField] Hittable hittable;

    private void Start() {
        BuildingObject buildingObject = this.GetComponent<BuildingObject>();
        hittable = this.GetComponent<Hittable>();
        string ticketName = "building_"+buildingObject.buildingData.id+"_Restore";
        restoreEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(restorationTerm, ticketName, Restore);
    }

    public bool Restore(string ticketName){
        if(hittable == null){
            return false;
        }
        hittable.Restore(1);
        return false;
    }
}