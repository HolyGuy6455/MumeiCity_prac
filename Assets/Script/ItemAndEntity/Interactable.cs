using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactable : MonoBehaviour {
    [SerializeField] InteractType _interactType;
    [SerializeField] UnityEvent _interactEvent = new UnityEvent();
    public InteractType interactType{get{ return _interactType; }}

    public enum InteractType
    {
        ITEM,
        BUILDING,
        OPEN_CHEST
    }
    public void Interact(){
        if (!this.gameObject.activeSelf)
            return;

        _interactEvent.Invoke();
    }
}