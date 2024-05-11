using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Sets the room's environment and tracks the player's progress in the level
/// </summary>
public class AddRoom : MonoBehaviour
{
    public GameObject enemyObject; /*!< Enemy */
    private GameObject enemy;
    public int enemiesFromOneSpawner;
    private GameObject[] enemySpawners; /*!<location on the map where enemies spawn */

    public List<GameObject> enemies;

    private bool spawned;
    private bool spawnedFirst = true;
    private Vector2 initialPosition;
    private Transform player;
    public AudioClip enemyStepSound;

    private void Start()
    {
        enemySpawners = GameObject.FindGameObjectsWithTag("Bug egg");
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
    }

    public void RestartLevel()
    {
        player.position = initialPosition;
        player.GetComponent<Player>().PlayWorld();
        spawned = false;
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        foreach (GameObject spawner in enemySpawners)
        {
            spawner.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !spawned)
        {
            player.GetComponent<Player>().PlayFight();
            initialPosition = player.position;
            Debug.Log(initialPosition);
            spawned = true;
            foreach (GameObject spawner in enemySpawners)
            {
                if (spawner.transform.parent.name == gameObject.transform.parent.parent.name)
                {
                    StartCoroutine(CreateEnemiesFromSpawner(spawner));
                    spawner.SetActive(false);
                }
            }
            if (spawnedFirst)
            {
                StartCoroutine(CheckEnemies());
                spawnedFirst = false;
            }
            
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = enemyStepSound;
            audioSource.Play();
        }
    }

    IEnumerator CreateEnemiesFromSpawner(GameObject spawner)
    {
        for (int i = 0; i < enemiesFromOneSpawner; i++)
        {
            enemy = Instantiate(enemyObject, spawner.transform.position, Quaternion.identity);
            enemies.Add(enemy);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && spawned);
        LevelComplete();
    }

    /// <summary>
    /// Events when level completed
    /// </summary>
    public void LevelComplete()
    {
        Debug.Log("Level completed!");
        player.gameObject.GetComponent<Player>().completedLevels += 1;
        player.GetComponent<Player>().PlayWorld();
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }
}
