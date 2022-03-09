using UnityEngine;
using UnityEngine.UI;

public class AchievementView : MonoBehaviour{
    [SerializeField] string AchievementName;
    [SerializeField] Slider progressSlider;
    [SerializeField] Text text;
    [SerializeField] Image fillImage;

    public void UpdateUI(){
        Achievement achievement = GameManager.Instance.achievementManager.GetAchievementInfo(AchievementName);
        float trialNumber = (float)achievement.trialNumber;
        float goalNumber = (float)achievement.goalNumber;
        progressSlider.value = trialNumber/goalNumber;
        if(progressSlider.value >= 1.0f){
            text.text = "Done";
            fillImage.color = new Color((255.0f/255.0f),(230/255.0f),(88/255.0f),(255/255.0f));
        }else{
            text.text = trialNumber + "/" + goalNumber;
        }
        
    }
}
