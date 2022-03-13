using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using FMODUnity;

public class Option : MonoBehaviour{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;

    public void SetResolution(string resolution){
        switch (resolution){
            case "640_360":
                Screen.SetResolution(640, 360, false);
                break;
            case "1024_576":
                Screen.SetResolution(1024, 576, false);
                break;
            case "1600_900":
                Screen.SetResolution(1600, 900, false);
                break;
            case "1920_1080":
                Screen.SetResolution(1920, 1080, false);
                break;
            default:
                break;
        }
    }

    public void RefreshVolume(){
        FMOD.Studio.System fmodSystem = FMODUnity.RuntimeManager.StudioSystem;
        fmodSystem.setParameterByName("BGM_Volume",BGMSlider.value);
        fmodSystem.setParameterByName("SFX_Volume",SFXSlider.value);
    }
}