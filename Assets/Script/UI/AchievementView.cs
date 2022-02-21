using UnityEngine;
using UnityEngine.UI;

public class AchievementView : MonoBehaviour{
    [SerializeField] string AchievementName;
    [SerializeField] Slider progressSlider;
    [SerializeField] Text text;

    public void UpdateUI(){
        Achievement achievement = GameManager.Instance.achievementManager.GetAchievementInfo(AchievementName);
        float trialNumber = (float)achievement.trialNumber;
        float goalNumber = (float)achievement.goalNumber;
        progressSlider.value = trialNumber/goalNumber;
        text.text = trialNumber + "/" + goalNumber;
    }
}
