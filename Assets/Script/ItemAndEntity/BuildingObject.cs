using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 인게임에서 보이는 건물의 GameObject
 */
public class BuildingObject : MonoBehaviour
{
    public BuildingData buildingData;
    public SpriteRenderer spriteRenderer;
    public GameObject spriteObject;
    public GameObject shadowObject;

    private void Start() {
        Vector3 shadowPostion = new Vector3();
        shadowPostion.x = this.transform.position.x;
        shadowPostion.y = (this.transform.position.y+this.transform.position.z);
        shadowObject.transform.position = shadowPostion;

        buildingData.positionX = ((int)Mathf.Round(this.transform.position.x));
        buildingData.positionY = ((int)Mathf.Round(this.transform.position.y));
        buildingData.positionZ = ((int)Mathf.Round(this.transform.position.z));

        if(this.buildingData != null){
            Initialize(this.buildingData);
        }
    }

    private void Update() {
        if(buildingData.buildingPreset.workplace && buildingData.workerID == 0){
            List<PersonBehavior> people = GameManager.Instance.peopleManager.GetWholePeopleList();
            people = people.FindAll(person=> (person != null)&&((person as PersonBehavior).personData.workplaceID == 0) );
            if(people.Count > 0){
                people[0].personData.workplaceID = this.buildingData.id;
                this.buildingData.workerID = people[0].personData.id;
            }
        }
    }

    public void Initialize(BuildingData buildingData){
        BuildingPreset buildingPreset = buildingData.buildingPreset;
        this.buildingData = buildingData;
        Debug.Log(buildingData);
        this.transform.localScale = buildingPreset.scale;
        spriteRenderer.sprite = buildingPreset.sprite;
        if( !buildingData.buildingPreset.interactable ){
            Debug.Log("buildingData "+this.GetComponentInChildren<Interactable>());
            this.GetComponentInChildren<Interactable>().gameObject.SetActive(false);
        }
    }

    public void ShowWindow(){
        GameManager.Instance.interactingBuilding = this.buildingData;
        GameManager.GameTab gameTab = GameManager.GameTab.NORMAL;
        BuildingManager buildingManager = GameManager.Instance.buildingManager;

        // switch문은 들어가는 인자가 반드시 상수여야한단다.....
        if(buildingData.code == buildingManager.presetDictionary["Forester"].code){
            gameTab = GameManager.GameTab.FORESTER;
        }else if(buildingData.code == buildingManager.presetDictionary["Tent"].code){
            gameTab = GameManager.GameTab.TENT;
        }else if(buildingData.code == buildingManager.presetDictionary["FoodStorage"].code){
            gameTab = GameManager.GameTab.TENT;
        }else if(buildingData.code == buildingManager.presetDictionary["Mine"].code){
            gameTab = GameManager.GameTab.TENT;
        }

        GameManager.Instance.ChangeGameTab(gameTab);
    }

}
