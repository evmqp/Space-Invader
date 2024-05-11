using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public Sprite firstPhaseImage;
    public Sprite newPhaseImage;
    public GameObject gun;
    public GameObject deathEffect;
    public float bulletForce;
    public GameObject bossBullet;
    private GameObject player;
    public int initHealth;
    private int health;
    public AudioClip flight;
    public AudioClip shot;
    float waitTime = 3f;
    bool phase2;
    bool phase3;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 targetPosition = player.transform.position;
        Vector2 direction = (targetPosition - rb.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 45f+180f));
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
        if (((float) health / (float)initHealth * 100 <= 70.0) && ((float)health / (float)initHealth * 100 >= 30.0) && !phase2) 
        {
            phase2 = true;
            waitTime -= 1f;
            player.GetComponent<Player>().PlayBoss2();
            GetComponent<SpriteRenderer>().sprite = newPhaseImage;
        }
        if ((float)health / (float)initHealth * 100 <= 30.0 && !phase3)
        {
            phase3 = true;
            waitTime -= 1f;
            player.GetComponent<Player>().PlayBoss3();
        }


    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GameObject explodeBoss = Instantiate(deathEffect, transform.position, Quaternion.identity);
            explodeBoss.transform.localScale = new Vector3(5, 5, 5);
            Destroy(explodeBoss, 1f);
            gameObject.SetActive(false);
            player.GetComponent<Player>().EndGame();
        }
    }
    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2f);
        
        int newHealth = health;
        while (health > 0)
        {
            GameObject bullet = Instantiate(bossBullet, gun.transform.position, gun.transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(gun.transform.up * bulletForce, ForceMode2D.Impulse);
            PlayShot();
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void ActivateBoss()
    {
        health = initHealth;
        waitTime = 3f;
        GetComponent<SpriteRenderer>().sprite = firstPhaseImage;
        phase2 = false;
        phase3 = false;
        StartCoroutine(Shoot());
    }

    public void PlayFlight()
    {
        GetComponent<AudioSource>().PlayOneShot(flight);
    }

    private void PlayShot()
    {
        GetComponent<AudioSource>().PlayOneShot(shot);
    }    
}
