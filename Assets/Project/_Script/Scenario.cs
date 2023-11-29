using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Scenario : MonoBehaviour
{
    [SerializeField]
    public List<GameEvent> gameEvents;

    [ExecuteInEditMode]
    void OnValidate()
    {
        gameEvents.Clear();
        gameEvents.AddRange(this.GetComponentsInChildren<GameEvent>());
    }

    public void InvokeGameEvent(int index)
    {
        gameEvents[index].events?.Invoke();
    }

    public void InvokeGameEvent(string id)
    {
        gameEvents.FirstOrDefault(x => x.ID == id).events?.Invoke();
    }

    void Start()
    {
        InvokeGameEvent("Start");
    }
}
