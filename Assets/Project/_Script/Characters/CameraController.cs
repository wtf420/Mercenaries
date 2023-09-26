using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    GameObject Camera;

    public Vector3 angle;
    Quaternion OriginalRotation;
    public float distance, speed, RotationSpeed;

    public bool LookAtPlayer;
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
    void Update()
    { 
        position = Vector3.Lerp(position, Player.transform.position, speed * Time.deltaTime);
        Vector3 rotation = Quaternion.Euler(angle) * Vector3.up;
        Ray ray = new Ray(position, distance * rotation);
        Camera.transform.position = ray.GetPoint(distance);

        if (LookAtPlayer)
        {
            Quaternion LookRotation = Quaternion.LookRotation((Player.transform.position - transform.position).normalized);
            Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, LookRotation, RotationSpeed * Time.deltaTime);
        } else
        {
            Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, OriginalRotation, RotationSpeed * Time.deltaTime);
        }
    }
}
