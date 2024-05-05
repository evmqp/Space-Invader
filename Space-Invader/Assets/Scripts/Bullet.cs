using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class for bullets
/// </summary>
public class Bullet : MonoBehaviour
{
    public GameObject hitEffect; /*!< The effect of bullets exploding upon collision with objects */
    public LayerMask whatIsSolid; /*!< Determines what is an obstacle */
    public float distance; /*!< The maximum distance a bullet flying */
    public int damage; /*!< The damage a bullet does to an object */

    public void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            if (!hitInfo.collider.CompareTag("Entry"))
            {
                GameObject effect = Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(effect, 1f);
                DestroyBullet();
            } 
        }
    }

    /// <summary>
    /// Destroys bullet
    /// </summary>
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}