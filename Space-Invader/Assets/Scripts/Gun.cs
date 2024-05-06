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
    public Transform firePoint;                 /*!< Bullet generation location */
    public GameObject bulletPrefab;
    public TextMeshProUGUI bulletCountText;     /*!<A text object showing the number of bullets remaining */
    private int bulletCount = 100;

    public float bulletForce;                   /*!< The force with which bullets start flying */
    public float waitTime;                      /*!< Delay between bullet generation */
    private bool isRecharging;

    float timer = 0.0f;


    private void Start()
    {
        bulletCountText.text = $"{bulletCount}";
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && !isRecharging)
        {
            if (timer >= waitTime && bulletCount > 0)
            {
                Shoot();
                bulletCount--;
                bulletCountText.text = $"{bulletCount}";
                timer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !isRecharging)
        {
            int ammo = gameObject.GetComponent<Player>().ammo;
            if (ammo > 0)
            {
                gameObject.GetComponent<Player>().SetAmmo(ammo - 1);
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}