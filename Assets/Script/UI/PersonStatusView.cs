using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonStatusView : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] Image staminaImage;
    [SerializeField] Image happinessImage;
    [SerializeField] Image backGroundImage;
    [SerializeField] Image hatImage;
    [SerializeField] Text owlName;
    [SerializeField] PersonData personData;
    [SerializeField] CanvasGroup canvasGroup;
    public PersonData _personData{
        get{
            return personData;
        }
    }
    public void UpdateUI(PersonData personData){
        this.personData = personData;
        staminaImage.fillAmount = ((float)personData.stamina)/1000.0f;
        happinessImage.fillAmount = ((float)personData.happiness)/100.0f;
        animator.SetBool("IsWorking",!personData.sleep);
        animator.SetFloat("Growth",personData.growth);
        hatImage.sprite = PeopleManager.Instance.GetJobInfo(personData.jobID).hatImage;
        owlName.text = personData.name;
    }

    public void SetVisible(bool visible){
        canvasGroup.alpha = (visible)? 1:0;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    public void SetVisibleBG(bool visible){
        backGroundImage.enabled = visible;
    }

}
