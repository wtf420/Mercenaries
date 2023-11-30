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
    public UnityAction OnWinEvent, OnLoseEvent;

    void Awake()
    {
        Destroy(instance);
        instance = this;
    }

    void Start()
    {
        UnityAction a = () =>
        {
            Debug.Log("1");
        };
        UnityAction b = () =>
        {
            Debug.Log("2");
        };
        UnityAction c = () =>
        {
            Debug.Log("3");
        };
        gameEvents[0].onEventsStart.AddListener(a);
        gameEvents[0].Events.AddListener(b);
        gameEvents[0].onEventsComplete.AddListener(c);
        InvokeGameEvent("Start");
    }

    [ExecuteInEditMode]
    void OnValidate()
    {
        gameEvents.Clear();
        gameEvents.AddRange(this.GetComponentsInChildren<GameEvent>());
    }

    public void InvokeGameEvent(int index)
    {
        StartCoroutine(gameEvents[index].Invoke());
    }

    public void InvokeGameEvent(string id)
    {
        StartCoroutine(gameEvents.FirstOrDefault(x => x.ID == id).Invoke());
        
    }
}
