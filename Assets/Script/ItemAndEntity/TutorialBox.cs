using UnityEngine;

public class TutorialBox : MonoBehaviour{
    [SerializeField] string content;
    [SerializeField] string guideBookTitle;
    private void OnTriggerEnter(Collider other) {
        if(other.tag != "Player"){
            return;
        }
        AchievementManager achievementManager = GameManager.Instance.achievementManager;
        if(!achievementManager.isDone(content)){
            GameManager.Instance.ChangeGameTab(GameManager.GameTab.GUIDE_BOOK);
        }
        achievementManager.AddTrial(content,1);
        Destroy(this.gameObject);
    }
}