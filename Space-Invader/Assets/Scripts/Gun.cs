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

    float timer = 0.0f;


    private void Start()
    {
        //bulletCountText.text = $"{bulletCount}";
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            if (timer >= waitTime && bulletCount > 0)
            {
                Shoot();
                bulletCount--;
                bulletCountText.text = $"{bulletCount}";
                timer = 0;
            }
        }
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