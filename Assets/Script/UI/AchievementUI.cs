using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUI : MonoBehaviour{
    [SerializeField] GameObject MumeiEra;
    [SerializeField] GameObject FaunaEra;
    [SerializeField] GameObject KroniiEra;
    [SerializeField] GameObject SanaEra;
    [SerializeField] GameObject BaelzOrdeal;
    GameObject selectedObject;
    [SerializeField] string selected;
    public void SelectEra(string eraName){
        selected = eraName;

        MumeiEra.SetActive(false);
        FaunaEra.SetActive(false);
        KroniiEra.SetActive(false);
        SanaEra.SetActive(false);
        BaelzOrdeal.SetActive(false);
        
        switch (eraName){
            case "Mumei":
                MumeiEra.SetActive(true);
                selectedObject = MumeiEra;
                break;
            case "Fauna":
                FaunaEra.SetActive(true);
                selectedObject = FaunaEra;
                break;
            case "Kronii":
                KroniiEra.SetActive(true);
                selectedObject = KroniiEra;
                break;
            case "Sana":
                SanaEra.SetActive(true);
                selectedObject = SanaEra;
                break;
            case "Baelz":
                BaelzOrdeal.SetActive(true);
                selectedObject = BaelzOrdeal;
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
    }
}
