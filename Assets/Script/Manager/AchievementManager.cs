using UnityEngine;
using System;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {
    public List<Achievement> achievements;
    public List<AchievementInfo> achievementInfos;
    Dictionary<string,Achievement> achievementsDictionary;
    Dictionary<string,AchievementInfo> infoDictionary;

    private void Awake() {
        achievementsDictionary = new Dictionary<string, Achievement>();
        infoDictionary = new Dictionary<string, AchievementInfo>();
        
        foreach (Achievement achievement in achievements){
            achievementsDictionary[achievement.name] = achievement;
        }
        foreach (AchievementInfo achievementInfo in achievementInfos){
            infoDictionary[achievementInfo.name] = achievementInfo;
        }
    }

    public void AddTrial(string achievementName, int value){
        achievementsDictionary[achievementName].trialNumber += value;
        GameManager.Instance.taskUIBundle.LoadTaskUIData("Achievement");
    }

    public void SetTrial(string achievementName, int value){
        achievementsDictionary[achievementName].trialNumber = value;
        GameManager.Instance.taskUIBundle.LoadTaskUIData("Achievement");
    }

    public bool isDone(string achievementName){
        Achievement achievement = achievementsDictionary[achievementName];
        return achievement.trialNumber >= achievement.goalNumber;
    }

    public float GetPercentage(string achievementName){
        Achievement achievement = achievementsDictionary[achievementName];
        return ((float)achievement.trialNumber/(float)achievement.goalNumber);
    }

    public Achievement GetAchievementInfo(string name){
        return achievementsDictionary[name];
    }

    public List<Achievement> GenerateSaveForm(){
        return new List<Achievement>(achievementsDictionary.Values);
    }

    public void ReloadSaveform(List<Achievement> list){
        achievementsDictionary = new Dictionary<string, Achievement>();
        foreach (Achievement achievement in list){
            achievementsDictionary[achievement.name] = achievement;
        }
    }

}

[Serializable]
public class Achievement{
    public string name;
    public int trialNumber;
    public int goalNumber;
}


[Serializable]
public class AchievementInfo{
    public string name;
    public Sprite descriptionImage;
    public string descriptionString;
}
