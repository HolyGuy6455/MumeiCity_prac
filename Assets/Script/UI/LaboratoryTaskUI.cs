using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LaboratoryTaskUI : MonoBehaviour{
    public Text taskTitle;
    public Image guideIamge;
    public ResourceView resourceView1;
    public ResourceView resourceView2;
    public ResourceView resourceView3;
    public Button researchButton;
    public Button cancleButton;
    public GameObject doneObject;
    public Slider processBars;

    public int requiredTime;
    public int dueDate;
    public int presentTime;
    public int remainingTime;
    public float remainingPercent;

    public void ChangeValue(float value){
        processBars.gameObject.SetActive( (value != 0) );
        processBars.value = value;
    }
    public void UpdateUI(TaskInfo taskInfo){
        if(taskInfo == null){
            this.gameObject.SetActive(false);
            return;
        }
        this.gameObject.SetActive(true);
        taskTitle.text = "- "+taskInfo.name+" -";
        guideIamge.sprite = taskInfo.guideSprite;
        List<NecessaryResource> resources = taskInfo.necessaryResources;
        resourceView1.UpdateResource((resources.Count > 0) ? resources[0] : null);
        resourceView2.UpdateResource((resources.Count > 1) ? resources[1] : null);
        resourceView3.UpdateResource((resources.Count > 2) ? resources[2] : null);

        researchButton.gameObject.SetActive(false);
        cancleButton.gameObject.SetActive(false);
        doneObject.SetActive(false);
        if(processBars.value < 0.01f){
            researchButton.gameObject.SetActive(true);
        }else if(processBars.value > 0.99f){
            doneObject.SetActive(true);
        }else{
            cancleButton.gameObject.SetActive(true);
        }
    }
    
}
