using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drone : MonoBehaviour
{
	public float speed, rotationSpeed;
	public GameObject target;
	Vector3 targetPosition;

	public Drone(GameObject Target = null)
	{
		target = Target;
	}

	void Start()
	{

	}

	void Update()
	{
		if (target != null)
			targetPosition = target.transform.position;

		var q = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);

		this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, speed * Time.deltaTime);
	}
}

