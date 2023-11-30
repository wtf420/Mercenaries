using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : MonoBehaviour
{
    [SerializeField] protected string _ID;
    [SerializeField] public string ID => _ID;
    public UnityEvent Events;
    public UnityEvent onEventsStart;
    public UnityEvent onEventsComplete;

    private IEnumerator WaitUntilEvent(UnityEvent unityEvent)
    {
        var trigger = false;
        Action action = () => trigger = true;
        unityEvent.AddListener(action.Invoke);
        yield return new WaitUntil(() => trigger);
        unityEvent.RemoveListener(action.Invoke);
    }

    public IEnumerator Invoke()
    {
        bool isDone = false;
        UnityAction action = () => isDone = true;
        onEventsStart.AddListener(action);
        Events.AddListener(action);
        onEventsComplete.AddListener(action);

        onEventsStart.Invoke();
        yield return new WaitUntil(() => isDone);

        isDone = false;
        Events.Invoke();
        yield return new WaitUntil(() => isDone);

        isDone = false;
        onEventsComplete.Invoke();
        yield return new WaitUntil(() => isDone);
    }
}

