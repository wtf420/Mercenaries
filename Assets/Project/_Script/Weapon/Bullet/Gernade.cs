using System.Collections;
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
        RaycastHit[] info = Physics.SphereCastAll(this.transform.position, _explosionRadius, Vector3.up, _explosionRadius);
        foreach (RaycastHit hit in info)
        {
            if (hit.collider.gameObject.GetComponent<Enemy>())
            {
                if (_damageScaleWithDistance)
                {
                    float distance = Vector3.Distance(this.transform.position, hit.point);
                    hit.collider.gameObject.GetComponent<Enemy>().TakenDamage(_damage * (distance / _explosionRadius));
                }
                else
                    hit.collider.gameObject.GetComponent<Enemy>().TakenDamage(_damage);
            }
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

