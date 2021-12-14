using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactable : MonoBehaviour {
    [Serializable] public class InteractEvent : UnityEvent {}
    [SerializeField] InteractType _interactType;
    [SerializeField] private InteractEvent _interactEvent = new InteractEvent();
    public InteractType interactType{get{ return _interactType; }}

    public enum InteractType
    {
        ITEM,
        BUILDING
    }
    public void Interact(){
        if (!this.gameObject.activeSelf)
            return;

        _interactEvent.Invoke();
    }
}