using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TentUI : CommonTaskUI{
    [SerializeField] PersonStatusView[] personStatusViews;
    [SerializeField] GameObject PersonStatusViewParent;
    [SerializeField] HouseData houseData;
    // Start is called before the first frame update
    protected override void Start(){
        base.Start();
        personStatusViews = PersonStatusViewParent.GetComponentsInChildren<PersonStatusView>();
    }

    // Update is called once per frame
    void Update(){
        if(GameManager.Instance.presentGameTab != GameManager.GameTab.TENT){
            return;
        }
        if(houseData == null){
            return;
        }
        for (int i = 0; i < personStatusViews.Length; i++){
            if(i >= houseData.personIDList.Length){
                personStatusViews[i].SetVisible(false);
                continue;
            }
            int personID = houseData.personIDList[i];
            PersonBehavior person = PeopleManager.FindPersonWithID(personID);
            if(person != null){
                personStatusViews[i].SetVisible(true);
                personStatusViews[i].UpdateUI(person.personData);
            }else{
                // personStatusViews[i].SetVisible(false);
            }
        }
        UpdateUI();
    }

    public override void UpdateUI(){
        base.UpdateUI();
        houseData = GameManager.Instance.interactingBuilding.mediocrityData as HouseData;
        Debug.Log("houseData "+houseData);
    }
}
