using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SuperintendentTaskUI : MonoBehaviour{
    public Text taskTitle;
    public Image guideIamge;
    public ResourceView resourceView1;
    public ResourceView resourceView2;
    public ResourceView resourceView3;
    public Toggle toggle;
    public TooltipArea tooltipArea;
    public void ChangeValue(bool value){
        toggle.isOn = value;
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
        // 이런 더러운 코드 쓰는건 별로 안좋아하지만.....
        resourceView1.UpdateResource((resources.Count > 0) ? resources[0] : null);
        resourceView2.UpdateResource((resources.Count > 1) ? resources[1] : null);
        resourceView3.UpdateResource((resources.Count > 2) ? resources[2] : null);
        tooltipArea.content = taskInfo.info;
    }
    
}
