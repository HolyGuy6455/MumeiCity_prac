using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SuperintendentTaskUI : MonoBehaviour{
    public Text taskTitle;
    public Image guideIamge;
    public ResourceView resourceView1;
    public ResourceView resourceView2;
    public ResourceView resourceView3;
    public Toggle toggle;
    public void ValueChanged(bool value){
        Debug.Log(value);
    }
    
}
