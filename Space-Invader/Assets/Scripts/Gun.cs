using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Player gun
/// </summary>
public class Gun : MonoBehaviour
{
    public AudioClip shotSound;
    public AudioClip rechargeSound;
    public Transform firePoint;                 /*!< Bullet generation location */
    public GameObject bulletPrefab;
    public TextMeshProUGUI bulletCountText;     /*!<A text object showing the number of bullets remaining */
    private int bulletCount = 100;

    public float bulletForce;                   /*!< The force with which bullets start flying */
    public float waitTime;                      /*!< Delay between bullet generation */
    private bool isRecharging;
    public GameObject player;

    float timer = 0.0f;


    private void Start()
    {
        bulletCountText.text = $"{bulletCount}";
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && !isRecharging)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = PlayerPrefs.GetFloat("volume");
            audioSource.clip = shotSound;
            if (timer >= waitTime && bulletCount > 0)
            {
                if (!audioSource.isPlaying)
                    audioSource.Play();
                Shoot();
                bulletCount--;
                bulletCountText.text = $"{bulletCount}";
                timer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isRecharging)
        {
            int ammo = gameObject.transform.parent.GetComponent<Player>().ammo;
            if (ammo > 0)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.volume = PlayerPrefs.GetFloat("volume");
                audioSource.PlayOneShot(rechargeSound);
                gameObject.transform.parent.GetComponent<Player>().SetAmmo(ammo - 1);
                StartCoroutine(Recharge(100));
            }
        }
    }

    private IEnumerator Recharge(int newBulletCount)
    {
        isRecharging = true;
        while (bulletCount < newBulletCount)
        {
            bulletCount += 1;
            bulletCountText.text = $"{bulletCount}";
            yield return new WaitForSeconds(0.1f);
        }
        isRecharging = false;
        
    }

    /// <summary>
    /// Generating new bullet
    /// </summary>
    public void Shoot()
    {
        player.GetComponent<Player>().PlayShootAnimation();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}