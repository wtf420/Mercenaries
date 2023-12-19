using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Gernade : MonoBehaviour
{
    [SerializeField] protected float explosionTimer, _explosionRadius, _damage;
    [SerializeField] protected bool _damageScaleWithDistance;

    public virtual void Update()
    {
        if (explosionTimer > 0)
        {
            explosionTimer -= Time.deltaTime;
        } else
        {
            Explode();
        }
    }

    protected virtual void Explode()
    {
        int layermask = LayerMask.GetMask("Damageables");
        RaycastHit[] info = Physics.SphereCastAll(this.transform.position, _explosionRadius, Vector3.up, 0, layermask);
        foreach (RaycastHit hit in info)
        {
            if (this.tag == hit.transform.tag)
            {
                continue;
            }
            
            //check if theres a wall between
            bool c = false;
            Vector3 hitlocation = (hit.point == Vector3.zero) ? hit.transform.position : hit.point;
            RaycastHit[] info2 = Physics.RaycastAll(this.transform.position, hitlocation - this.transform.position, Vector3.Distance(this.transform.position, hit.transform.position));
            foreach (RaycastHit hit2 in info2)
            {
                //theres an object blocking
                if (hit2.collider.gameObject.GetComponent<BulletproofWall>() || (hit2.collider.gameObject.GetComponent<IDamageable>() == null))
                {
                    c = true;
                }
            }
            if (c) continue;

            if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
            {
                float distance = hit.distance;
                float Damage = _damageScaleWithDistance ? _damage * (1 / (distance / _explosionRadius)) : _damage;
                if (hit.collider.gameObject.GetComponent<Enemy>())
                {
                    hit.collider.gameObject.GetComponent<Enemy>().TakenDamage(Damage, hit.point - hit.transform.position, 10f);
                }
                else
                {
                    hit.collider.gameObject.GetComponent<IDamageable>().TakenDamage(Damage);
                }
            }
            //Debug.DrawLine(this.transform.position, hitlocation, Color.green, 5f);
        }
        StopAllCoroutines();
        Destroy(gameObject);
    }

    [ExecuteInEditMode]
    protected virtual void OnDrawGizmos()
    {
        if (this.tag == "Player")
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, _explosionRadius);
    }
}

