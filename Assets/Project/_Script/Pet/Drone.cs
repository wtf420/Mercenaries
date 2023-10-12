using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drone : MonoBehaviour
{
	public float speed, rotationSpeed, minDistance = 1f, damage, interval;
	public GameObject target;
	Vector3 targetPosition, lookPosition;

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
		{
			targetPosition = target.transform.position;
			lookPosition = targetPosition;
			lookPosition.y = 0;
		}

		var q = Quaternion.LookRotation(lookPosition - transform.position);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);

		if (Vector3.Distance(targetPosition, this.transform.position) > minDistance)
		{
			this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, speed * Time.deltaTime);
		} else if (target != null && target.GetComponent<Enemy>())
		{
			StartCoroutine(Attack());
		}
	}

	IEnumerator Attack()
	{
		target.GetComponent<Enemy>().TakenDamage(50f);
		yield return new WaitForSeconds(interval);
	}

	public void SetTarget(GameObject Target)
	{
		target = Target;
	}
}

