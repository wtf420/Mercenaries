using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float force;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Character c = collider.gameObject.GetComponent<Character>();
            c.AddForce(Vector3.up * force);
        }
    }
}
