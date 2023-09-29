using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent", fileName = "New GameEvent")]
public class GameEventSO : ScriptableObject
{
    private List<GameEventSOListener> listeners = new List<GameEventSOListener>();

    public void AddListener(GameEventSOListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventSOListener listener)
    {
        listeners.Remove(listener);
    }

    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }
}