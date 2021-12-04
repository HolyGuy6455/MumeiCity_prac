using UnityEngine;
using System;

public interface IEntityDestroyEvent {
    delegate void VoidEvent();
    event VoidEvent EntityDestroyEventHandler;
}