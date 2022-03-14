using UnityEngine;
using System;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {
    public List<Achievement> achievements;
    Dictionary<string,Achievement> dictionary;

    private void Awake() {
        dictionary = new Dictionary<string, Achievement>();
        foreach (Achievement achievement in achievements){
            dictionary[achievement.name] = achievement;
        }
    }

    public void AddTrial(string achievementName, int value){
        dictionary[achievementName].trialNumber += value;
        GameManager.Instance.taskUIBundle.LoadTaskUIData("Achievement");
    }

    public void SetTrial(string achievementName, int value){
        dictionary[achievementName].trialNumber = value;
        GameManager.Instance.taskUIBundle.LoadTaskUIData("Achievement");
    }

    public bool isDone(string achievementName){
        Achievement achievement = dictionary[achievementName];
        return achievement.trialNumber >= achievement.goalNumber;
    }

    public float GetPercentage(string achievementName){
        Achievement achievement = dictionary[achievementName];
        return ((float)achievement.trialNumber/(float)achievement.goalNumber);
    }

    public Achievement GetAchievementInfo(string name){
        return dictionary[name];
    }

    public List<Achievement> GenerateSaveForm(){
        return new List<Achievement>(dictionary.Values);
    }

    public void ReloadSaveform(List<Achievement> list){
        dictionary = new Dictionary<string, Achievement>();
        foreach (Achievement achievement in list){
            dictionary[achievement.name] = achievement;
        }
    }

}

[Serializable]
public class Achievement{
    public string name;
    public int trialNumber;
    public int goalNumber;
}
