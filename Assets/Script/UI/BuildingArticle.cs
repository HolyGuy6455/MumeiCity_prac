using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BuildingArticle : MonoBehaviour{
    [SerializeField] Text buildingName;
    [SerializeField] Image buildingImage;
    [SerializeField] BuildingPreset buildingPreset;
    [SerializeField] BuildingUI buildingUI;

    public void UpdatePreset(BuildingPreset buildingPreset){
        if(buildingPreset == null){
            this.buildingPreset = null;
            return;
        }
        this.buildingPreset = buildingPreset;
        buildingName.text = buildingPreset.name;
        buildingImage.sprite = buildingPreset.sprite;
    }

    public void ArticleSelect(){
        GameManager.Instance.buildingManager.ReadyToBuild(this.buildingPreset);
        buildingUI.UpdateInfomation();
    }
}
