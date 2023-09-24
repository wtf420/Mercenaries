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
			//Debug.Log(range);
			previousPosition = transform.position;
			yield return null;
		}

		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (this.CompareTag(collider.tag))
		{
			return;
		}
		
		Destroy(gameObject);
		if (IsCollideWithEnemy(collider))
		{
			return;
		}

		if(IsCollideWithPlayer(collider))
		{
			return;
		}
	}

	private bool IsCollideWithEnemy(Collider collider)
	{
		Enemy enemy = collider.GetComponent<Enemy>();
		if(enemy)
		{
			enemy.TakenDamage(dame);
			return true;
		}

		return false;
	}

	private bool IsCollideWithPlayer(Collider collider)
	{
		Character character = collider.GetComponent<Character>();
		if (character)
		{
			character.TakenDamage(dame);
			return true;
		}

		return false;
	}

	#endregion
}

