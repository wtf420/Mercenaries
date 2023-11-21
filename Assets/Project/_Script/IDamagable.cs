using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public bool IsDead { get; }
    public float AttackPriority { get; }
    public virtual void TakenDamage(float damage) {}
}
