using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagSlot : MonoBehaviour{
    [SerializeField] Image TagBG;
    [SerializeField] Text TagText;
    string itemTag;
    public void UpdateUI(string itemTag){
        this.itemTag = itemTag;
        TagText.text = itemTag;
    }
}
