using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Default,
    Bullet,
    Explosive
}

public class Damage
{
    public float value;
    public Vector3 Origin;
    public DamageType damageType;

    public Damage(float v, Vector3 o, DamageType t)
    {
        value = v;
        Origin = o;
        damageType = t;
    }
}

public interface IDamageable
{
    public bool IsDead { get; }
    public float AttackPriority { get; }
    public virtual void TakenDamage(float damage) { }
    public bool IsInPatrolScope { get; set; }
}
