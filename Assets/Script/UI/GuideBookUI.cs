using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuideBookUI : MonoBehaviour{
    public Text guideTitle;
    public Text guideDescriptionText;
    public Image guideImage;
    [SerializeField] GameObject scrollContent;
    [SerializeField] RectTransform scrollContentRect;
    public Dictionary<string,GuideBookDescription> descriptions;
    
    private static GuideBookUI singleton;
    public static GuideBookUI Instance{
        get{
            return singleton;
        }
    }

    private void Awake() {
        singleton = this;
    }

    public void AddDescription(string key, GuideBookDescription value){
        if(descriptions == null){
            descriptions = new Dictionary<string, GuideBookDescription>();
        }
        descriptions[key] = value;
        Debug.Log("descriptions " + key);
    }

    public void UpdateUI(){
        float x = scrollContent.transform.position.x;
        float y = scrollContent.transform.position.y;
        Vector2 newSize = new Vector2();
        newSize.x = 312;
        newSize.y = 0;
        foreach (RectTransform transform in scrollContent.transform){
            newSize.y += transform.sizeDelta.y;
        }
        if(newSize.y < 362){
            
            newSize.y = 362;
            // scrollContent.transform.position = new Vector3(x,0);
        }else{
            scrollContent.transform.position = new Vector3(x,y);
        }
        scrollContentRect.sizeDelta = newSize;
    }

    // [ContextMenu("Resize")]
    // public void scrap
}