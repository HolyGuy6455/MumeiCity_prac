using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonStatusView : MonoBehaviour{
    [SerializeField] Animator animator;
    [SerializeField] Image staminaImage;
    [SerializeField] Image happinessImage;
    [SerializeField] Image personImage;
    public void UpdateUI(PersonData personData){
        staminaImage.fillAmount = ((float)personData.stamina)/1000.0f;
        happinessImage.fillAmount = ((float)personData.happiness)/100.0f;
        animator.SetBool("IsWorking",!personData.sleep);
    }

    public void SetVisible(bool visible){
        staminaImage.enabled = visible;
        happinessImage.enabled = visible;
        personImage.enabled = visible;
    }
}
