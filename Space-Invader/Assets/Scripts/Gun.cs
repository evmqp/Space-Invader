using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public TextMeshProUGUI bulletCountText;
    private int bulletCount = 100;

    public float bulletForce;
    public float waitTime;

    float timer = 0.0f;


    private void Start()
    {
        bulletCountText.text = $"{bulletCount}";
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

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}