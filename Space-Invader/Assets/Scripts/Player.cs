using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Class for player
/// </summary>
public class Player : MonoBehaviour
{
    public int level = 1; /*!< Current level */
    public int health = 3; /*!< Current health */
    public AudioClip musicWorld;
    public AudioClip musicFightStart;
    public AudioClip musicFight;
    public AudioClip bossStart;
    public AudioClip bossMusic;
    public AudioClip bossMusic2;
    public AudioClip bossMusic3;
    public AudioClip step;
    public AudioClip boxOpenClip;
    public int grenadeNum;
    public int grenadeNumInBox;
    public int ammo;
    public int shield;
    public float speed; /*!< Speed of player's moving */
    public GameObject playerInterface;
    public GameObject ShieldImage;
    public GameObject DiedImage;
    public GameObject SurvivedImage;
    public Camera cam; /*!< Camera following to player */
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Vector2 mousePos;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI grenadeText;
    private bool died;
    public GameObject grenadePrefab;
    public bool restartLevel;
    private GameObject entry;
    public int completedLevels;
    public float grenadeMoveSpeed;
    private Animator anim;
    public GameObject flashlight;

    private void Start()
    {
        playerInterface.SetActive(true);
        ammoText.text = ammo.ToString();
        grenadeText.text = grenadeNum.ToString();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Q) && grenadeNum > 0)
            ThrowGrenade();

        if (Input.GetKeyDown(KeyCode.F))
            flashlight.SetActive(!flashlight.activeSelf);
        if (moveVelocity != Vector2.zero)
        {
            anim.SetBool("isRunning", true);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = PlayerPrefs.GetFloat("volume");
            audioSource.clip = step;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        cam.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (shield == 0 && !died)
            {
                health--;
                if (health == 0)
                {
                    died = true;
                    StartCoroutine(ShowFinalMessage(false));
                }
                else
                {
                    entry.GetComponent<AddRoom>().RestartLevel();
                }   
            }
            else
            {
                shield = 0;
                ShieldImage.SetActive(false);
                Destroy(collision.gameObject);
            }
                
        }
    }

    public void EndGame()
    {
        StartCoroutine(ShowFinalMessage(true));
    }

    private IEnumerator ShowFinalMessage(bool success)
    {
        AudioSource audioSource = cam.GetComponent<AudioSource>();
        audioSource.Stop();
        GameObject endMessage;
        if (success)
        {
            endMessage = SurvivedImage;
            yield return new WaitForSeconds(10f);
        }  
        else endMessage = DiedImage;
        endMessage.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }
        

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss bullet"))
        {
            Destroy(other);
            if (shield == 0 && !died)
            {
                if (health == 0)
                {
                    died = true;
                    StartCoroutine(ShowFinalMessage(false));
                }
                else
                {
                    health--;
                    entry.GetComponent<BossLevel>().RestartLevel();
                }
            }
            else
            {
                shield = 0;
                ShieldImage.SetActive(false);
            }
        }
        if (other.CompareTag("Entry"))
        {
            entry = other.gameObject;
        }

        if (other.CompareTag("Box ammo"))
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = PlayerPrefs.GetFloat("volume");
            audioSource.PlayOneShot(boxOpenClip);
            ammo += 1;
            grenadeNum += grenadeNumInBox;
            ammoText.text = ammo.ToString();
            grenadeText.text = grenadeNum.ToString();
            other.gameObject.tag = "Box";
        }
        else if (other.CompareTag("Box shield"))
        {
            if (shield == 0)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.volume = PlayerPrefs.GetFloat("volume");
                audioSource.PlayOneShot(boxOpenClip);
                shield = 1;
                ShieldImage.SetActive(true);
                other.gameObject.tag = "Box";
            }   
        }
    }

    public void SetAmmo(int new_ammo)
    {
        ammo = new_ammo;
        ammoText.text = ammo.ToString();
    }

    public void ThrowGrenade()
    {
        anim.SetBool("isThrowingGrenade", false);
        anim.Play("PlayerThrowGrenade");

        grenadeNum--;
        grenadeText.text = grenadeNum.ToString();
        Vector3 playerPosition = transform.position;
        Quaternion playerRotation = transform.rotation;
        Vector3 spawnPosition = playerPosition + transform.up * 1f;
        Vector3 targetPosition = playerPosition + transform.up * grenadeMoveSpeed;

        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, playerRotation);
        Rigidbody2D grenade_rb = grenade.GetComponent<Rigidbody2D>();
        Vector3 relativePos = targetPosition - grenade.transform.position;
        grenade_rb.AddForce(relativePos);
    }

    public void PlayShootAnimation()
    {
        anim.SetBool("isShooting", false);
        anim.Play("PlayerShoot");

    }

    public void PlayWorld()
    {
        AudioSource audioSource = cam.GetComponent<AudioSource>();
        audioSource.clip = musicWorld;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayFight()
    {
        StartCoroutine(PlayFightSequence());
    }

    public void PlayBossStart()
    {
        AudioSource audioSource = cam.GetComponent<AudioSource>();
        audioSource.clip = bossStart;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayBossMusic()
    {
        AudioSource audioSource = cam.GetComponent<AudioSource>();
        audioSource.clip = bossMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayBoss2()
    {
        AudioSource audioSource = cam.GetComponent<AudioSource>();
        audioSource.clip = bossMusic2;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayBoss3()
    {
        AudioSource audioSource = cam.GetComponent<AudioSource>();
        audioSource.clip = bossMusic3;
        audioSource.loop = true;
        audioSource.Play();
    }

    private IEnumerator PlayFightSequence()
    {
        AudioSource audioSource = cam.GetComponent<AudioSource>();
        audioSource.clip = musicFightStart;
        audioSource.loop = false;
        audioSource.Play();
        yield return new WaitUntil(() => !audioSource.isPlaying);
        audioSource.clip = musicFight;
        audioSource.loop = true;
        audioSource.Play();
    }
}
