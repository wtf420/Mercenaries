using System.Collections;
using UnityEngine;

public class Bullet: MonoBehaviour
{
	#region Fields & Properties
	float dame;
	float range;
	float speed;

	Vector3 direction;

	Vector3 previousPosition;
	#endregion

	#region Methods
	public void Initialize(float dame, float range, float speed, Vector3 direction)
	{
		this.dame = dame;
		this.range = range;
		this.speed = speed;
		this.direction = direction;

		previousPosition = transform.position;

		StartCoroutine(RangeRemaining());
	}

	IEnumerator RangeRemaining()
	{
		while(range > 0)
		{
			transform.position += direction * speed * Time.deltaTime;

			range -= Vector3.Distance(transform.position, previousPosition);
			Debug.Log(range);
			previousPosition = transform.position;
			yield return null;
		}

		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		
	}

	#endregion
}

