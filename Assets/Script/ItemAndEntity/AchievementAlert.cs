using UnityEngine;

public class AchievementAlert : MonoBehaviour {
    public void Alert(string value){
        GameManager.Instance.achievementManager.AddTrial(value,1);
    }
}