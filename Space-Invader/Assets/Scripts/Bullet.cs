using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class for bullets
/// </summary>
public class Bullet : MonoBehaviour
{
    public GameObject hitEffect; /*!< The effect of bullets exploding upon collision with objects */
    public GameObject bossBulletEffect;
    public LayerMask whatIsSolid; /*!< Determines what is an obstacle */
    public float distance; /*!< The maximum distance a bullet flying */
    public int damage; /*!< The damage a bullet does to an object */
    public bool isBossBullet;
    public AudioClip ricochetSound;

    public void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid); 
        if (hitInfo.collider != null)
        {
            if (isBossBullet)
            {
                if (hitInfo.collider.name == "Tilemap")
                {
                    GameObject effect = Instantiate(bossBulletEffect, transform.position, transform.rotation);
                    Destroy(effect, 1f);
                    Destroy(gameObject);
                }
            }
            else
            {
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
                    Destroy(gameObject);
                }
                else if (hitInfo.collider.CompareTag("Boss"))
                {
                    hitInfo.collider.GetComponent<Boss>().TakeDamage(damage);
                    GameObject effect = Instantiate(hitEffect, transform.position, transform.rotation);
                    AnimationClip anim = effect.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
                    Destroy(effect, anim.length);
                    Destroy(gameObject);
                }
                if (!hitInfo.collider.CompareTag("Entry") && !hitInfo.collider.CompareTag("Enemy") && !hitInfo.collider.CompareTag("Boss"))
                {;
                    GameObject effect = Instantiate(hitEffect, transform.position, transform.rotation);
                    AudioSource audioSource = effect.GetComponent<AudioSource>();
                    audioSource.volume = PlayerPrefs.GetFloat("volume");
                    audioSource.clip = ricochetSound;
                    audioSource.Play();
                    AnimationClip anim = effect.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
                    Destroy(effect, anim.length);
                    Destroy(gameObject);
                }
                
            }
        }
    }
}