using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour{
    [SerializeField] Text resourceName;
    [SerializeField] Image resourceImage;
    [SerializeField] Text resourceAmount;
    [SerializeField] ItemData itemData;


    public void UpdateResource(NecessaryResource buildingMaterial){
        if(buildingMaterial == null){
            this.itemData = null;
            resourceName.text = "";
            resourceImage.sprite = GameManager.Instance.emptySprite;
            resourceAmount.text = "";
        }else{
            this.itemData = ItemData.Instant(buildingMaterial.itemDataName);
            resourceName.text = itemData.itemName;
            resourceImage.sprite = itemData.itemSprite;

            int resourceAmountValue = buildingMaterial.amount;
            int havingAmountValue = GameManager.Instance.inventory.GetItemAmount(itemData.itemName);
            string resourceAmountString = havingAmountValue.ToString() + "/" + resourceAmountValue.ToString();
            Color textColor = (resourceAmountValue>havingAmountValue)? Color.red : Color.white;

            resourceAmount.text = resourceAmountString;
            resourceAmount.color = textColor;
        }
    }
}
