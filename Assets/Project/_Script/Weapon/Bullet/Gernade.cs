using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gernade : MonoBehaviour
{
    public float collisiontimer, explosionTimer, explosionRadius, damage;
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>());
        StartCoroutine(ignoreCollision());
    }

    IEnumerator ignoreCollision()
    {
        yield return new WaitForSeconds(collisiontimer);
        Physics.IgnoreCollision(this.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
    }

    void Update()
    {
        if (explosionTimer > 0)
        {
            explosionTimer -= Time.deltaTime;
        } else
        {
            Explode();
        }
    }

    void Explode()
    {
        RaycastHit[] info = Physics.SphereCastAll(this.transform.position, explosionRadius, Vector3.up, explosionRadius);
        foreach (RaycastHit hit in info)
        {
            if (hit.collider.gameObject.GetComponent<Enemy>())
            {
                hit.collider.gameObject.GetComponent<Enemy>().TakenDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}

