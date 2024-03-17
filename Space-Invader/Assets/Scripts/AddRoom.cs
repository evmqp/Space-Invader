using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners;
    public GameObject shield;

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

    public void LevelComplete()
    {
        Debug.Log("Level Completed!");
    }
}
