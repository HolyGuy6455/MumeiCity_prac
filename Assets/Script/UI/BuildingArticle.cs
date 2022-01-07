using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingArticle : MonoBehaviour{
    [SerializeField] Text buildingName;
    [SerializeField] Image buildingImage;
    [SerializeField] BuildingPreset buildingPreset;
    [SerializeField] BuildingUI buildingUI;

    public void UpdatePreset(BuildingPreset buildingPreset){
        this.buildingPreset = buildingPreset;
        buildingName.text = buildingPreset.name;
        buildingImage.sprite = buildingPreset.sprite;
    }

    public void ArticleSelect(){
        GameManager.Instance.buildingManager.ReadyToBuild(this.buildingPreset);
        buildingUI.UpdateInfomation();
    }
}
