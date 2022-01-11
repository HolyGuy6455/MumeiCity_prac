using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonInfoUI : MonoBehaviour
{
    [SerializeField] RectTransform UIRectTransform;
    [SerializeField] new Camera camera;
    [SerializeField] Animator animator;
    [SerializeField] Image staminaImage;
    [SerializeField] Image happinessImage;
    public bool visible{
        get{
            return animator.GetBool("isVisible");
        }
        set{
            animator.SetBool("isVisible",value);
        }
    }

    public void MoveUIPositionFromTransform(Transform targetTransform){
        Vector2 ViewportPosition=camera.WorldToScreenPoint(targetTransform.position);
        UIRectTransform.position = new Vector3(ViewportPosition.x,ViewportPosition.y,0);
    }

    public void UpdateIcon(PersonData personData){
        staminaImage.fillAmount = ((float)personData.stamina)/1000.0f;
        happinessImage.fillAmount = ((float)personData.happiness)/100.0f;
        // actingImage.sprite = _interactTypeIconsDictionary[interactType];
    }
}
