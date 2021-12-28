using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour{
    public delegate void TimeEvent();
    [SerializeField] int timeValue;
    [SerializeField] float ticPerSecond = 10.0f;
    [SerializeField] Animator daylightAnimator;
    [SerializeField] int lengthOfDay = 100;
    [SerializeField] int morning = 20;
    [SerializeField] int evening = 60;
    [SerializeField] int midnight = 80;
    [SerializeField] int timeInDay;
    [SerializeField] int elapsedDate;
    [SerializeField] List<TimeEventQueueTicket> waitingList = new List<TimeEventQueueTicket>();

    float blendValue;
    public float _blendValue{get{return blendValue;}}
    public int _timeInDay{get{return timeInDay;}}
    public int _timeValue{get{return timeValue;}set{timeValue=value;}}
    bool dayTime;
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
    public bool IsItDayTime(){return dayTime;}

    public TimeEventQueueTicket AddTimeEventQueueTicket(int delay, string idString, TimeManager.TimeEvent timeEvent){
        foreach (TimeEventQueueTicket queueTicket in waitingList){
            if(queueTicket._idString == idString){
                return null;
            }
        }

        TimeEventQueueTicket result = new TimeEventQueueTicket(this.timeValue + delay, idString, timeEvent);
        waitingList.Add(result);
        return result;
    }

    void Start(){
        StartCoroutine("CountTime", 1);
    }

    void Update(){
        elapsedDate = timeValue/lengthOfDay;
        timeInDay = timeValue%lengthOfDay;
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
        bool newDayTime = (blendValue<0.5);
        if(dayTime != newDayTime){
            dayTime = newDayTime;
            if(dayTime){
                // 출근시간
                GameManager.Instance.peopleManager.ResetHouseInfomation();
            }else{
                // 퇴근시간
            }
        }
        daylightAnimator.SetFloat("Blend",blendValue);

        List<TimeEventQueueTicket> discardList = new List<TimeEventQueueTicket>();

        foreach (TimeEventQueueTicket queueTicket in waitingList){
            if(queueTicket._timeValue <= this.timeValue){
                queueTicket._timeEvent.Invoke();
                discardList.Add(queueTicket);
            }
        }
        foreach (TimeEventQueueTicket discardQueueTicket in discardList){
            waitingList.Remove(discardQueueTicket);
        }
    }

    IEnumerator CountTime(float delayTime) {
      timeValue++;
      yield return new WaitForSeconds(delayTime);
      StartCoroutine("CountTime", 1/ticPerSecond);
   }
}

[Serializable]
public class TimeEventQueueTicket{
    [SerializeField] int timeValue;
    [SerializeField] string idString;
    TimeManager.TimeEvent timeEvent;
    public int _timeValue{get{return timeValue;}}
    public string _idString{get{return idString;}}
    public TimeManager.TimeEvent _timeEvent{get{return timeEvent;}}
    
    public TimeEventQueueTicket(int timeValue,string idString, TimeManager.TimeEvent timeEvent){
        this.timeValue = timeValue;
        this.idString = idString;
        this.timeEvent = timeEvent;
    }
    public bool isThisValid(){
        return GameManager.Instance.timeManager._timeValue < this.timeValue;
    }
}