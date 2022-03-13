using System.Collections.Generic;
using UnityEngine;

public class Restoration : MonoBehaviour, ITiemEventRebindInfo{
    [SerializeField] TimeEventQueueTicket restoreEvent;
    [SerializeField] int restorationTerm;
    [SerializeField] Hittable hittable;
    [SerializeField] GameObject grownPrefab;

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
        if(grownPrefab != null && hittable.HP >= hittable.HPMax){
            Vector3 location = this.transform.position;
            Destroy(this.gameObject);
            GameObject Built = Instantiate(grownPrefab,location,Quaternion.identity);
        }
        return false;
    }

    public Dictionary<string, TimeManager.TimeEvent> GetDictionary(){
        Dictionary<string, TimeManager.TimeEvent> result = new Dictionary<string, TimeManager.TimeEvent>();

        BuildingObject buildingObject = this.GetComponent<BuildingObject>();
        string ticketName = "building_"+buildingObject.buildingData.id+"_Restore";
        result[ticketName] = Restore;

        return result;
    }
}