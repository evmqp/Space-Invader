using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionEffect;
    public int radius;
    public AudioClip bang;
    private void Start()
    {
        Explode();
    }

    public void Explode()
    {
        StartCoroutine(ExplodeWithTimer(1f));
    }

    public IEnumerator ExplodeWithTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("volume");
        audioSource.clip = bang;
        audioSource.Play();
        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            if (overlappedColliders[i].tag == "Enemy")
            {
                overlappedColliders[i].gameObject.GetComponent<Enemy>().Die();
            }
            if (overlappedColliders[i].tag == "Boss")
            {
                overlappedColliders[i].gameObject.GetComponent<Boss>().TakeDamage(3);
            }
        }
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        effect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        AnimationClip anim = effect.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip;
        Destroy(effect, anim.length) ;
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    } 
}
