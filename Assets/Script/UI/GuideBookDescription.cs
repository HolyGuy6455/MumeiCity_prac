using UnityEngine;
using UnityEngine.UI;

public class GuideBookDescription : MonoBehaviour{
    [SerializeField] Sprite infoImage;
    [SerializeField] [TextArea] string infoDescription;
    [SerializeField] string infoTitle;

    [ContextMenu("Manual Initialize")]
    public void OnPointerClick(){
        GuideBookUI guideBookUI = GuideBookUI.Instance;
        guideBookUI.guideTitle.text = infoTitle;
        guideBookUI.guideDescriptionText.text = infoDescription;
        guideBookUI.guideImage.sprite = infoImage;
    }
}
