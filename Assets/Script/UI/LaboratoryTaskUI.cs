using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LaboratoryTaskUI : MonoBehaviour{
    public Text taskTitle;
    public Image guideIamge;
    public ResourceView resourceView1;
    public ResourceView resourceView2;
    public ResourceView resourceView3;
    public Button submitButton;
    public Button cancleButton;
    public void ValueChanged(bool value){
        Debug.Log(value);
    }
    
}
