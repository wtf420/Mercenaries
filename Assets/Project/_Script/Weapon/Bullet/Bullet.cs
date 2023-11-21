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

		if (IsCollideWithPlayer(collider))
		{
			return;
		}

		if (IsCollideWithPet(collider))
		{
			return;
		}

		if (IsCollideWithWall(collider))
		{
			return;
		}
	}

	private bool IsCollideWithEnemy(Collider collider)
	{
		Enemy enemy = collider.GetComponent<Enemy>();
		if(enemy)
		{
			enemy.TakenDamage(dame, enemy.transform.position - transform.position, 10f);
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

	private bool IsCollideWithPet(Collider collider)
	{
		Pet pet = collider.GetComponent<Pet>();
		if (pet)
		{
			pet.TakenDamage(dame);
			return true;
		}

		return false;
	}

	private bool IsCollideWithWall(Collider collider)
	{
		BulletproofWall wall = collider.GetComponent<BulletproofWall>();
		if (wall)
		{
			wall.TakenDamage(dame);
			return true;
		}

		return false;
	}

	#endregion
}

