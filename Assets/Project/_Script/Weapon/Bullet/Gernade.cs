using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gernade : MonoBehaviour
{
    public float timer;
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>());
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        } else
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        }
    }
}

