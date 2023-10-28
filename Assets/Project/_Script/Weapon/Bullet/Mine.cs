using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Mine : MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] float explosionRadius = 3f;
	[SerializeField] float damage = 40f;

	#endregion

	#region Methods
	public static Mine Create(Vector3 position, string tag)
	{
		Mine turret = Instantiate(Resources.Load<Mine>("_Prefabs/Bullet/Mine"), position, new Quaternion());
		turret.tag = tag;
		return turret;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (this.CompareTag(collider.tag))
		{
			return;
		}

		CalculateExplosionRange();

		Destroy(gameObject);
	}

	private void CalculateExplosionRange()
	{
		RaycastHit[] hits;

		hits = Physics.SphereCastAll(transform.position, explosionRadius, transform.forward, explosionRadius);
		if (hits.Length > 0)
		{
			foreach (var e in hits)
			{
				if (IsCollideWithCharacter(e.collider.GetComponent<Character>()))
					continue;

				if (IsCollideWithEnemy(e.collider.GetComponent<Enemy>()))
					continue;

				if (IsCollideWithPet(e.collider.GetComponent<Pet>()))
					continue;
			}
		}
	}

	private bool IsCollideWithCharacter(Character character)
	{
		if (!character)
		{
			return false;
		}

		character.TakenDamage(damage);

		return true;
	}

	private bool IsCollideWithEnemy(Enemy enemy)
	{
		if (!enemy)
		{
			return false;
		}

		enemy.TakenDamage(damage);

		return true;
	}

	private bool IsCollideWithPet(Pet pet)
	{
		if (!pet)
		{
			return false;
		}

		pet.TakenDamage(damage);

		return true;
	}
	#endregion
}

