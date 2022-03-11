using UnityEngine;
using UnityEngine.UI;

public class GuideBookDescription : MonoBehaviour{
    [SerializeField] string infoTitle;
    [SerializeField] Sprite infoImage;
    [SerializeField] [TextArea] string infoDescription;

    [ContextMenu("Manual Initialize")]
    public void OnPointerClick(){
        GuideBookUI guideBookUI = GameManager.Instance.guideBookUI;
        guideBookUI.guideTitle.text = infoTitle;
        guideBookUI.guideDescriptionText.text = infoDescription;
        guideBookUI.guideImage.sprite = infoImage;
    }
}
