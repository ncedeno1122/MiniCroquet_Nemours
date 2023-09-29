using UnityEngine;
using UnityEngine.Events;

public class GameEventSOListener : MonoBehaviour
{
    public GameEventSO GameEvent;
    public UnityEvent TriggeredUnityEvent;

    private void OnEnable()
    {
        GameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        GameEvent.RemoveListener(this);
    }

    public void OnEventTriggered()
    {
        TriggeredUnityEvent?.Invoke();
    }
}