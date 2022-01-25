using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PauseUI : MonoBehaviour{

    public void SaveTheGame(){
        GameManager.Instance.saveLoadManager.SaveTheGame();
    }

    public void LoadTheGame(){
        GameManager.Instance.saveLoadManager.LoadTheGame();
    }
}
