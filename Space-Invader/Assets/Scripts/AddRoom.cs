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
    public int enemiesFromOneSpawner;
    private GameObject enemy;
    private GameObject[] enemySpawners; /*!<location on the map where enemies spawn */

    //[HideInInspector] public List<GameObject> enemies;
    public List<GameObject> enemies;

    private bool spawned;


    private void Start()
    {
        enemySpawners = GameObject.FindGameObjectsWithTag("Bug egg");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !spawned)
        {
            spawned = true;
            foreach (GameObject spawner in enemySpawners)
            {
                if (spawner.transform.parent.name == gameObject.transform.parent.parent.name)
                {
                    for (int i = 0; i < enemiesFromOneSpawner; i++)
                    {
                        enemy = Instantiate(enemyObject, spawner.transform.position, Quaternion.identity);
                        enemies.Add(enemy);
                    }
                    
                    spawner.SetActive(false);
                }
            }
        }
        StartCoroutine(CheckEnemies());
    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemies.Count == 0);
        LevelComplete();
    }

    /// <summary>
    /// Events when level completed
    /// </summary>
    public void LevelComplete()
    {
        Debug.Log("Level Completed!");
    }
}
