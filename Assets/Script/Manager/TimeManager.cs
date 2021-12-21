using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour{
    [SerializeField] int timeValue;
    [SerializeField] float ticPerSecond = 10.0f;
    [SerializeField] Animator daylightAnimator;
    [SerializeField] int lengthOfDay = 100;
    [SerializeField] int morning = 20;
    [SerializeField] int evening = 60;
    [SerializeField] int midnight = 80;
    [SerializeField] int timeInDay;
    [SerializeField] int elapsedDate;
    float blendValue;
    public float _blendValue{get{return blendValue;}}
    public int _timeInDay{get{return timeInDay;}}
    public enum TimeSlot{
        NONE,
        MORNING,
        DAY,
        EVENING,
        NIGHT
    }
    public TimeSlot _timeSlot{
        get{
            if(timeInDay < morning){
                return TimeSlot.MORNING;
            }else if(timeInDay < evening){
                return TimeSlot.DAY;
            }else if(timeInDay < midnight){
                return TimeSlot.EVENING;
            }else{
                return TimeSlot.NIGHT;
            }
        }
    }

    void Start(){
        StartCoroutine("CountTime", 1);
    }

    void Update(){
        elapsedDate = timeValue/lengthOfDay;
        timeInDay = timeValue%lengthOfDay;
        blendValue = 0.0f;
        switch (_timeSlot){
            case TimeSlot.MORNING:
                blendValue = ((float)(morning-timeInDay))/((float)(morning));
                break;
            case TimeSlot.DAY:
                blendValue = 0.0f;
                break;
            case TimeSlot.EVENING:
                blendValue = ((float)(timeInDay - evening))/((float)(midnight-evening));
                break;
            case TimeSlot.NIGHT:
                blendValue = 1.0f;
                break;
            default:
                break;
        }
        daylightAnimator.SetFloat("Blend",blendValue);
    }

    IEnumerator CountTime(float delayTime) {
      timeValue++;
      yield return new WaitForSeconds(delayTime);
      StartCoroutine("CountTime", 1/ticPerSecond);
   }
}
