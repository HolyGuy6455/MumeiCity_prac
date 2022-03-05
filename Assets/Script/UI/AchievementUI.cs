using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour{
    [SerializeField] GameObject MumeiEra;
    [SerializeField] GameObject FaunaEra;
    [SerializeField] GameObject KroniiEra;
    [SerializeField] GameObject SanaEra;
    [SerializeField] GameObject BaelzOrdeal;
    [SerializeField] RectTransform eraTranstorm;
    [SerializeField] Text eraTitle;
    GameObject selectedObject;
    [SerializeField] string selected;
    public void SelectEra(string eraName){

        MumeiEra.SetActive(false);
        FaunaEra.SetActive(false);
        KroniiEra.SetActive(false);
        SanaEra.SetActive(false);
        BaelzOrdeal.SetActive(false);
        
        switch (eraName){
            case "Mumei":
                MumeiEra.SetActive(true);
                selectedObject = MumeiEra;
                selected = "- Mumei's Era -";
                break;
            case "Fauna":
                FaunaEra.SetActive(true);
                selectedObject = FaunaEra;
                selected = "- Fauna's Era -";
                break;
            case "Kronii":
                KroniiEra.SetActive(true);
                selectedObject = KroniiEra;
                selected = "- Kronii's Era -";
                break;
            case "Sana":
                SanaEra.SetActive(true);
                selectedObject = SanaEra;
                selected = "- Sana's Era -";
                break;
            case "Baelz":
                BaelzOrdeal.SetActive(true);
                selectedObject = BaelzOrdeal;
                selected = "- Baelz's Ordeal -";
                break;
            default:
                break;
        }
        UpdateUI();
    }

    public void UpdateUI(){
        if(selectedObject == null){
            selectedObject = MumeiEra;
        }
        AchievementView[] achievementViews = selectedObject.GetComponentsInChildren<AchievementView>();
        foreach (AchievementView achievementView in achievementViews){
            achievementView.UpdateUI();
        }
        RectTransform selectedRectTransform = selectedObject.GetComponent<RectTransform>();
        eraTranstorm.sizeDelta = selectedRectTransform.sizeDelta;
        eraTranstorm.transform.localPosition = new Vector3(0,-eraTranstorm.sizeDelta.y/4);
        eraTitle.text = selected;
    }
}
