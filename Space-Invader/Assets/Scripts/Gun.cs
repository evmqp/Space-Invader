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

    public float bulletForce = 20f;
    public float waitencyTime = 0.125f;

    float timer = 0.0f;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            if (timer >= waitencyTime && bulletCount > 0)
            {
                Shoot();
                bulletCount--;
                bulletCountText.text = $"{bulletCount}";
                timer = 0;
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}