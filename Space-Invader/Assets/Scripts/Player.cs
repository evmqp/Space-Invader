using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for player
/// </summary>
public class Player : MonoBehaviour
{
    public int level = 1; /*!< Current level */
    public int health = 3; /*!< Current health */
    public int ammo;
    public int shield;
    public float speed; /*!< Speed of player's moving */
    public GameObject playerInterface;
    public GameObject DiedImage;
    public Camera cam; /*!< Camera following to player */
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Vector2 mousePos;
    public TextMeshProUGUI ammoText;
    private bool died;
    public bool cheatMode;
    public GameObject grenade;
    public float distanceInFrontStart;
    public float distanceInFrontEnd;
    public float grenadeSpeed;
    private bool isThrowingGrenade;

    private void Awake()
    {
        level = 0;
        health = 0;
        
        //LoadPlayer();
    }

    private void Start()
    {
        playerInterface.SetActive(true);
        ammoText.text = ammo.ToString();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //if (Input.GetKeyDown(KeyCode.Q) && !isThrowingGrenade)
        //    ThrowGrenade();
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
            Debug.Log("Lost!");
            if (shield == 0 && !died && !cheatMode)
            {
                died = true;
                StartCoroutine(ShowFinalMessage());
            }
            else
            {
                shield = 0;
                Destroy(collision.gameObject);
            }
                
        }
    }

    private IEnumerator ShowFinalMessage()
    {
        DiedImage.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }
        

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box ammo"))
        {
            ammo += 1;
            ammoText.text = ammo.ToString();
            other.gameObject.tag = "Box";
        }
        else if (other.CompareTag("Box shield"))
        {
            if (shield == 0)
            {
                shield = 1;
                other.gameObject.tag = "Box";
            }   
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;
        health = data.health;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = -10;
        transform.position = position;
    }

    public void SetAmmo(int new_ammo)
    {
        ammo = new_ammo;
        ammoText.text = ammo.ToString();
    }

    public void ThrowGrenade()
    {
        Vector3 playerPosition = transform.position;
        Quaternion playerRotation = transform.rotation;
        Vector3 spawnPosition = playerPosition;
        //Vector3 targetPosition = playerPosition + playerRotation * Vector3.forward * distanceInFrontEnd;

        Instantiate(grenade, spawnPosition, playerRotation);

       // grenade.transform.position = Vector3.MoveTowards(grenade.transform.position, targetPosition, Time.deltaTime * grenadeSpeed);
    }
}
