using UnityEngine;

interface IDamageable
{
	void TakenDamage(float damage, Vector3? DamageDirection = null, float punch = 0.0f);
}
