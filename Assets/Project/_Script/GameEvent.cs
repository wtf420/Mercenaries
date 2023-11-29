using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : MonoBehaviour
{
    [SerializeField] protected string _ID;
    [SerializeField] public string ID => _ID;
    public UnityEvent events;
}

