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
    TimeSlot lastTimeSlot = TimeSlot.NONE;
    bool dayTime;
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
        if(lastTimeSlot != _timeSlot){
            lastTimeSlot = _timeSlot;
            OnTimeSlotChanged();
        }
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
        dayTime = (blendValue<0.5);
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
      if(ticPerSecond == 0){
          // 실수로 0이 들어가면, 그냥 10초뒤에 실행되게 한다. 10초도 길어...
          StartCoroutine("CountTime", 10);
      }else{
          StartCoroutine("CountTime", 1/ticPerSecond);
      }
      
   }

    void OnTimeSlotChanged(){
        switch (_timeSlot){
            case TimeSlot.MORNING:
                GameManager.Instance.peopleManager.OfferJob();
                break;
            case TimeSlot.DAY:
                GameManager.Instance.peopleManager.ResetHouseInfomation();
                break;
            case TimeSlot.EVENING:
                break;
            case TimeSlot.NIGHT:
                GameManager.Instance.peopleManager.SurveyHappiness();
                break;
            default:
                break;
       }
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