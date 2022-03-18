using UnityEngine;

public class HouseUI : CommonTaskUI{
    [SerializeField] PersonStatusView[] personStatusViews;
    [SerializeField] GameObject PersonStatusViewParent;
    [SerializeField] HouseFunction houseFunction;
    // Start is called before the first frame update
    protected override void Start(){
        base.Start();
        personStatusViews = PersonStatusViewParent.GetComponentsInChildren<PersonStatusView>();
    }

    // Update is called once per frame
    void Update(){
        if(GameManager.Instance.presentGameTab != GameManager.GameTab.HOUSE){
            return;
        }
        if(houseFunction == null){
            return;
        }
        for (int i = 0; i < personStatusViews.Length; i++){
            if(i >= houseFunction.personIDList.Length){
                personStatusViews[i].SetVisible(false);
                personStatusViews[i].SetVisibleBG(false);
                continue;
            }
            int personID = houseFunction.personIDList[i];
            PersonBehavior person = PeopleManager.FindPersonWithID(personID);
            if(person != null){
                personStatusViews[i].SetVisible(true);
                personStatusViews[i].UpdateUI(person.personData);
            }
            personStatusViews[i].SetVisibleBG(true);
        }
        UpdateUI();
    }

    public override void UpdateUI(){
        base.UpdateUI();
        houseFunction = GameManager.Instance.interactingBuilding.buildingData.facilityFunction as HouseFunction;
        // Debug.Log("houseData "+houseData);
    }
}
