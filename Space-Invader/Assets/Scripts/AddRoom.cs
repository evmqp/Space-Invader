using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Sets the room's environment and tracks the player's progress in the level
/// </summary>
public class AddRoom : MonoBehaviour
{
    public GameObject[] enemyTypes; /*!< Types of enemies */
    public Transform[] enemySpawners; /*!<location on the map where enemies spawn */
    public GameObject shield; /*!< Shield to use by player  */

    [HideInInspector] public List<GameObject> enemies;

    private bool spawned;


    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !spawned)
        {
            spawned = true;

            foreach (Transform spawner in enemySpawners)
            {
                GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity) as GameObject;
                enemy.transform.parent = transform;
                enemies.Add(enemy);
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
