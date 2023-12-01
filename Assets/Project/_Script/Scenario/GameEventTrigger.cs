using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    public GameEvent gameEvent;
    [SerializeField] protected bool OneTimeUseOnly = false;

    protected bool Used = false;

    public void TriggerGameEvent()
    {
        if (!Used || !OneTimeUseOnly)
        {
            if (gameEvent != null)
            {
                Debug.Log("Triggered");
                StartCoroutine(gameEvent.Invoke());
            }
            Used = true;
        }
    }
}
