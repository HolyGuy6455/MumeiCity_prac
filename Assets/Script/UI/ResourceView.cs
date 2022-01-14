using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour{
    [SerializeField] Text resourceName;
    [SerializeField] Image resourceImage;
    [SerializeField] Text resourceAmount;
    [SerializeField] ItemPreset itemPreset;


    public void UpdateResource(BuildingResource buildingMaterial){
        if(buildingMaterial == null){
            this.itemPreset = null;
            resourceName.text = "";
            resourceImage.sprite = GameManager.Instance.emptySprite;
            resourceAmount.text = "";
        }else{
            this.itemPreset = ItemManager.GetItemPresetFromCode(ItemManager.GetCodeFromItemName(buildingMaterial.preset.name));
            resourceName.text = itemPreset.name;
            resourceImage.sprite = itemPreset.itemSprite;
            resourceAmount.text = buildingMaterial.amount.ToString();
        }
    }
}
