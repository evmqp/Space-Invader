using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes an enemy
/// </summary>
public class Enemy : MonoBehaviour
{

    public int health; /*!< Current health */
    public GameObject deathEffectPrefab; /*!< Effect playing just after health of an enemy become 0 */
    private GameObject deathEffect;
    private Transform player; /*!< Location of the player */
    public float speed; /*!< Enemie's speed */

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
        //Vector3 direction = player.position - transform.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 targetPosition = player.position;
        Vector2 direction = (targetPosition - rb.position).normalized;
        rb.velocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90f));
    }

    /// <summary>
    /// Take damage to enemy by player
    /// </summary>
    /// <param name="damage">How much damage will be taken</param>
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}