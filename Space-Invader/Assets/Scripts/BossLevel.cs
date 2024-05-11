using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossLevel : MonoBehaviour
{
    GameObject boss;
    GameObject player;
    Camera mainCamera;
    private bool bossActivated;
    public GameObject door;
    private bool levelsCompleted;
    public float cameraSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        player = GameObject.FindGameObjectWithTag("Player");
        boss.SetActive(false);
    }

    private void Update()
    {
        if (player.GetComponent<Player>().completedLevels == 9)
        {
            door.SetActive(false);
            levelsCompleted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && !bossActivated && levelsCompleted)
        {
            bossActivated = true;
            boss.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            StartCoroutine(BossAppear());
            player.GetComponent<Player>().PlayBossStart();
            boss.GetComponent<Boss>().PlayFlight();
        }
    }

    private IEnumerator BossAppear()
    {
        mainCamera = Camera.main;
        mainCamera.GetComponent<cameraToPlayer>().freeze = true;
        while (mainCamera.transform.position.x != boss.transform.position.x && mainCamera.transform.position.y != boss.transform.position.y)
        {
            Vector3 targetPos = new Vector3(boss.transform.position.x, boss.transform.position.y, -10);
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPos, cameraSpeed * Time.deltaTime);
            yield return null;
        }
        boss.SetActive(true);
        boss.GetComponent<Boss>().ActivateBoss();
        GameObject gun = GameObject.Find("Gun");
        gun.SetActive(false);
        Debug.Log(boss.transform.localScale);
        while (boss.transform.localScale != Vector3.one)
        {
            boss.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        gun.SetActive(true);
        player.GetComponent<Player>().PlayBossMusic();
    }

    public void RestartLevel()
    {
        boss.SetActive(false);
        bossActivated = false;
        player.transform.position = transform.position;

    }
}
