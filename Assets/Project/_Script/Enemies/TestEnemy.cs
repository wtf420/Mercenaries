using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TestEnemy : Enemy, IDamageable
{
    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        UpdateEnemy();
    }
}

