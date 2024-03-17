using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public LayerMask whatIsSolid;
    public float distance;
    public int damage;

    public void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        GameObject effect = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(effect, 1f);
        Destroy(gameObject);
    }
}