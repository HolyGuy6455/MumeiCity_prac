using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonStatusView : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] Image staminaImage;
    [SerializeField] Image staminaBGImage;
    [SerializeField] Image happinessImage;
    [SerializeField] Image happinessBGImage;
    [SerializeField] Image backGroundImage;
    [SerializeField] Image personImage;
    [SerializeField] Image hatImage;
    [SerializeField] PersonData personData;
    public void UpdateUI(PersonData personData){
        this.personData = personData;
        staminaImage.fillAmount = ((float)personData.stamina)/1000.0f;
        happinessImage.fillAmount = ((float)personData.happiness)/100.0f;
        animator.SetBool("IsWorking",!personData.sleep);
        animator.SetFloat("Growth",personData.growth);
        hatImage.sprite = PeopleManager.Instance.GetJobInfo(personData.jobID).hatImage;
    }

    public void SetVisible(bool visible){
        staminaImage.enabled = visible;
        staminaBGImage.enabled = visible;
        happinessImage.enabled = visible;
        happinessBGImage.enabled = visible;
        backGroundImage.enabled = visible;
        personImage.enabled = visible;
        hatImage.enabled = visible;
    }

}
