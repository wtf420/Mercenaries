using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Scenario : MonoBehaviour
{
    private static Scenario instance;
    public static Scenario Instance
    {
        get => instance;
        private set => instance = value;
    }

    [SerializeField]
    public List<GameEvent> gameEvents;
    public UnityEvent OnStartEvent, OnWinEvent, OnLoseEvent;

    void Awake()
    {
        Destroy(instance);
        instance = this;

        UnityAction a = () =>
        {
            InvokeGameEvent("End");
        };

        OnWinEvent?.AddListener(a);
        OnLoseEvent?.AddListener(a);
        UnityAction b = () =>
        {
            InvokeGameEvent("Start");
        };
        OnStartEvent?.AddListener(b);
    }

    void Start()
    {
        OnStartEvent?.Invoke();
    }

    public void InvokeGameEvent(int index)
    {
        GameEvent e = gameEvents[index];
        if (e != null)
            StartCoroutine(e.Invoke());
    }

    public void InvokeGameEvent(string id)
    {
        GameEvent e = gameEvents.FirstOrDefault(x => x.ID == id);
        if (e != null)
            StartCoroutine(e.Invoke());
    }

    public GameEvent GetGameEvent(string id)
    {
        return gameEvents.FirstOrDefault(x => x.ID == id);
    }
}
