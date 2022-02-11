using UnityEngine;
using UnityEngine.UI;

public class GuideBookDescription : MonoBehaviour{
    [SerializeField] Sprite infoImage;
    [SerializeField] [TextArea] string infoDescription;
    [SerializeField] string infoTitle;
    [SerializeField] GuideBookUI guideBookUI;

    public void OnPointerClick(){
        guideBookUI.guideTitle.text = infoTitle;
        guideBookUI.guideDescriptionText.text = infoDescription;
        guideBookUI.guideImage.sprite = infoImage;
    }
}
