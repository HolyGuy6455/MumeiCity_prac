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

    public void addTrial(string achievementName, int count){
        dictionary[achievementName].trialNumber += count;
        GameManager.Instance.taskUIBundle.LoadTaskUIData("Achievement");
    }

    public Achievement GetAchievementInfo(string name){
        return dictionary[name];
    }

}

[Serializable]
public class Achievement{
    public string name;
    public int trialNumber;
    public int goalNumber;
    public bool hidden;
}
