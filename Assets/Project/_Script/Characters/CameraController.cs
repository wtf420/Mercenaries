using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    GameObject Camera;

    public Vector3 angle, offSet;
    Quaternion OriginalRotation;
    public float distance, speed, RotationSpeed;

    public bool LookAtPlayer, MoveWithTarget;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        Player = GameObject.FindGameObjectWithTag("Player");
        OriginalRotation = Camera.transform.rotation;

        Vector3 rotation = Quaternion.Euler(angle) * Vector3.up;
        Ray ray = new Ray(Player.transform.position, distance * rotation);
        Camera.transform.position = ray.GetPoint(distance);

        position = Player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        if (MoveWithTarget)
        {
            //move position to player
            if (Vector3.Distance(this.transform.position, Player.transform.position) < speed * Time.deltaTime)
            {
                position = Player.transform.position;
            } else
            {
                position = Vector3.Lerp(position, Player.transform.position, speed * Time.deltaTime);
            }
            position += offSet;

            //calculate camera postion with angles and distance
            Vector3 rotation = Quaternion.Euler(angle) * Vector3.up;
            Ray ray = new Ray(position, distance * rotation);
            Camera.transform.position = ray.GetPoint(distance);

            //look at player
            if (LookAtPlayer)
            {
                Quaternion LookRotation = Quaternion.LookRotation((Player.transform.position - transform.position).normalized);
                Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, LookRotation, RotationSpeed * Time.deltaTime);
            }
            else
            {
                Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, OriginalRotation, RotationSpeed * Time.deltaTime);
            }
        }
    }
}
