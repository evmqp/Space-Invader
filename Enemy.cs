using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 10;
    public GameObject deathEffect;
    public Transform player;
    public float speed = 5.0f;

    private void Start()
    {

    }

    private void Update()
    {
        

    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
    }


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
