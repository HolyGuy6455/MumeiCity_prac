using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TentUI : MonoBehaviour{
    [SerializeField] List<Slider> sliders;
    [SerializeField] ItemSlot[] StorageSlots;
    HouseData houseData;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if(houseData != null){
            Debug.Log("sliders.Count : "+sliders.Count);
            // 이건 급하게 짜느라 이렇게 했다. slider.count를 나중에 housedata.personlist.count로 바꿀것
            for (int i = 0; i < sliders.Count; i++){
                int personID = houseData.personIDList[i];
                PersonCommonAI person = GameManager.Instance.peopleManager.FindPersonWithID(personID);
                Debug.Log("personID : "+personID);
                if(person != null){
                    sliders[i].value = ((float)person.personData.stamina)/100.0f;
                    Debug.Log(sliders[i].value);
                }
            }
        }
    }

    public void LoadHouseData(){
        Debug.Log(GameManager.Instance.interactingBuilding.mediocrityData is HouseData);
        houseData = GameManager.Instance.interactingBuilding.mediocrityData as HouseData;
        Debug.Log("houseData "+houseData);
    }

    public void LoadItemSlotData(){
        // ItemSlotData[] itemSlotData = GameManager.Instance.interactingBuilding.items;
        // for (int i = 0; i < itemSlotData.Length; i++){
        //     StorageSlots[i].data = itemSlotData[i];
        //     StorageSlots[i].UpdateUI();
        // }
    }
}
