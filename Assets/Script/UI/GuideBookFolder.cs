using UnityEngine;
using UnityEngine.UI;

public class GuideBookFolder : MonoBehaviour{
    [SerializeField] Image openImage;
    [SerializeField] Image closeImage;
    [SerializeField] GameObject childObject;
    [SerializeField] RectTransform rect;
    bool isActivated;

    private void Start() {
        Activate(false);
    }

    public void Activate(bool value){
        isActivated = value;
        childObject.SetActive(isActivated);
        if(isActivated){
            openImage.enabled = true;
            closeImage.enabled = false;
        }else{
            openImage.enabled = false;
            closeImage.enabled = true;
        }
        Resize();
    }
    public void OnPointerClick(){
        Activate(!isActivated);
    }

    [ContextMenu("Resize")]
    private void Resize(){
        Vector2 newSize = new Vector2();
        newSize.x = 100;
        newSize.y = 30;
        if(isActivated){
            foreach (RectTransform transform in childObject.transform){
                newSize.y += transform.sizeDelta.y;
                Debug.Log(transform.sizeDelta.y);
            }
        }
        rect.sizeDelta = newSize;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform.parent);
        GuideBookFolder parentFolder = this.transform.parent.parent.GetComponent<GuideBookFolder>();
        if(parentFolder != null){
            parentFolder.Resize();
        }
    }
}
