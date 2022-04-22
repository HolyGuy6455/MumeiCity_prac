using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TimeManager : MonoBehaviour{
    public delegate bool TimeEvent(string args = "");
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
    [SerializeField] StudioEventEmitter studioEventEmitter;
    public List<TimeEventQueueTicket> _waitingList{
        get{
            return waitingList;
        }
        set{
            waitingList = value;
        }
    }

    float blendValue;
    public float _blendValue{get{return blendValue;}}
    public int _timeInDay{get{return timeInDay;}}
    public int _timeValue{get{return timeValue;}set{timeValue=value;}}
    public int _elapsedDate{get{return elapsedDate;}}
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
        TimeEventQueueTicket result = new TimeEventQueueTicket(this.timeValue, idString, delay, timeEvent);
        waitingList.Add(result);
        return result;
    }

    public TimeEventQueueTicket GetTicket(string idString){
        foreach (TimeEventQueueTicket queueTicket in waitingList){
            if(queueTicket._idString == idString){
                return queueTicket;
            }
        }
        return null;
    }

    public void RemoveTimeEventQueueTicket(string idString){
        TimeEventQueueTicket ticket = GetTicket(idString);
        waitingList.Remove(ticket);
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
        studioEventEmitter.SetParameter("DayOrNight",blendValue);

        List<TimeEventQueueTicket> invokeList = new List<TimeEventQueueTicket>();

        foreach (TimeEventQueueTicket queueTicket in waitingList){
            if(queueTicket._endTime <= this.timeValue){
                invokeList.Add(queueTicket);
            }
        }

        foreach (TimeEventQueueTicket ticket in invokeList){
            bool isDone = false;
            waitingList.Remove(ticket);
            if(ticket._timeEvent != null){
                isDone = ticket._timeEvent.Invoke(ticket._idString);
            }
            if(!isDone){
                AddTimeEventQueueTicket(ticket._delay,ticket._idString,ticket._timeEvent);
            }
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
                // GameManager.Instance.peopleManager.OfferJob();
                break;
            case TimeSlot.DAY:
                // GameManager.Instance.peopleManager.ResetHouseInfomation();
                break;
            case TimeSlot.EVENING:
                break;
            case TimeSlot.NIGHT:
                // GameManager.Instance.peopleManager.SurveyHappiness();
                GameManager.Instance.mobManager.Spawn();
                break;
            default:
                break;
       }
   }
}

[Serializable]
public class TimeEventQueueTicket{
    [SerializeField] int startTime;
    [SerializeField] int delay;
    [SerializeField] string idString;
    TimeManager.TimeEvent timeEvent;
    public int _startTime{get{return startTime;}}
    public int _endTime{get{return startTime+delay;}}
    public int _delay{get{return delay;}}

    public string _idString{get{return idString;}}
    public TimeManager.TimeEvent _timeEvent{
        get{
            return timeEvent;
        }
        set{
            timeEvent = value;
        }
    }
    
    public TimeEventQueueTicket(int startTime, string idString, int delay, TimeManager.TimeEvent timeEvent){
        this.startTime = startTime;
        this.idString = idString;
        this.timeEvent = timeEvent;
        this.delay = delay;
    }
    public bool isThisValid(){
        return GameManager.Instance.timeManager._timeValue <= this._endTime;
    }
}