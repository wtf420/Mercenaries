using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] bool isFollowPlayer = true;

	public GameObject Player;
	GameObject Camera;

	public Vector3 angle, offSet;
	Quaternion OriginalRotation;
	public float distance = 1f, speed, RotationSpeed;

	public bool LookAtPlayer;
	Vector3 position, playerLastPosition;

	#endregion

	#region Methods

	public void Initialize(GameObject player)
	{
		//offSet = player.position - transform.position;
		Player = player;
		Camera = this.gameObject;
	}

	public void UpdateCamera()
	{
		if (Input.GetKeyDown(KeyCode.C))
			isFollowPlayer = !isFollowPlayer;

		if (isFollowPlayer)
		{
			position = Vector3.Lerp(position, Player.transform.position, speed * Time.deltaTime);
			position += offSet;

			//calculate camera postion with angles and distance
			Vector3 rotation = Quaternion.Euler(angle) * Vector3.up;
			Ray ray = new Ray(position, distance * rotation.normalized);
			Camera.transform.position = ray.GetPoint(distance);

			//look at player
			if (LookAtPlayer)
			{
				Quaternion LookRotation = Quaternion.LookRotation(Player.transform.position - transform.position, Vector3.up);
				Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, LookRotation, RotationSpeed * Time.deltaTime);
			}
			else
			{
				Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, OriginalRotation, RotationSpeed * Time.deltaTime);
			}

			playerLastPosition = Player.transform.position;
		}
	}

	#endregion
}