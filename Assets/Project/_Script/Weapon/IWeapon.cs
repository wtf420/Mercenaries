using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeapon : MonoBehaviour
{
    public GameConfig.WEAPON Type { get; protected set; }
    public virtual void Initialize() {}
    public virtual void AttemptAttack() {}
    public virtual void AttemptReload() { }
}
