using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class GameGenerator : MonoBehaviour
{
    public GameObject[] start_levels;
    public GameObject[] levels;
    public GameObject[] boss_levels;
    public List<int> selected_levels = new List<int>();
    public List<GameObject> existing_levels = new List<GameObject>();
    public List<GameObject> open_levels = new List<GameObject>();
    public GameObject corridor_h;
    public GameObject corridor_v;
    public int padding = 2;
    public bool generateAgain = false;

    private void create_attached_level(Transform[] entries, List<Transform> disabledEntries, bool isBossLevel = false)
    {
        Vector2 newLevelPosition = new Vector2(0, 0);
        
        foreach (Transform entry in entries)
        {
            if (disabledEntries.Contains(entry))
            {
                continue;
            }
                
            entry.gameObject.SetActive(false);
            // расчет положения следующей комнаты
            if (entry.name == "Up")
            {
                Vector2 corridorCoords = new Vector2(entry.position.x, entry.position.y + padding);
                GameObject corridor = Instantiate(corridor_v, corridorCoords, Quaternion.identity);
                newLevelPosition = corridorCoords + new Vector2(0, 2);
                string newEntryDirection = "Down";
                if (isBossLevel)
                {
                    GameObject boss_level = Instantiate(boss_levels[0], newLevelPosition, Quaternion.identity);
                    Transform e = boss_level.transform.Find("Entry");
                    boss_level.transform.position = newLevelPosition - (Vector2)e.position;
                    boss_level.transform.position += (Vector3)newLevelPosition - e.position;
                    return;
                }
                create_next_level(newLevelPosition, newEntryDirection, true);
            }

            if (entry.name == "Down")
            {
                Vector2 corridorCoords = new Vector2(entry.position.x, entry.position.y - padding);
                GameObject corridor = Instantiate(corridor_v, corridorCoords, Quaternion.identity);
                newLevelPosition = corridorCoords + new Vector2(0, -2);
                string newEntryDirection = "Up";
                if (isBossLevel)
                {
                    GameObject boss_level = Instantiate(boss_levels[3], newLevelPosition, Quaternion.identity);
                    Transform e = boss_level.transform.Find("Entry");
                    boss_level.transform.position = newLevelPosition - (Vector2)e.position;
                    boss_level.transform.position += (Vector3)newLevelPosition - e.position;
                    return;
                }
                create_next_level(newLevelPosition, newEntryDirection, true);
            }

            if (entry.name == "Left")
            {
                Vector2 corridorCoords = new Vector2(entry.position.x - padding, entry.position.y);
                GameObject corridor = Instantiate(corridor_h, corridorCoords, Quaternion.identity);
                newLevelPosition = corridorCoords + new Vector2(-2, 0);
                string newEntryDirection = "Right";
                if (isBossLevel)
                {
                    GameObject boss_level = Instantiate(boss_levels[2], newLevelPosition, Quaternion.identity);
                    Transform e = boss_level.transform.Find("Entry");
                    boss_level.transform.position = newLevelPosition - (Vector2)e.position;
                    boss_level.transform.position += (Vector3)newLevelPosition - e.position;
                    return;
                }
                create_next_level(newLevelPosition, newEntryDirection, true);
            }

            if (entry.name == "Right")
            {
                Vector2 corridorCoords = new Vector2(entry.position.x + padding, entry.position.y);
                GameObject corridor = Instantiate(corridor_h, corridorCoords, Quaternion.identity);
                newLevelPosition = corridorCoords + new Vector2(2, 0);
                string newEntryDirection = "Left";
                if (isBossLevel)
                {
                    GameObject boss_level = Instantiate(boss_levels[1], newLevelPosition, Quaternion.identity);
                    Transform e = boss_level.transform.Find("Entry");
                    boss_level.transform.position = newLevelPosition - (Vector2)e.position;
                    boss_level.transform.position += (Vector3)newLevelPosition - e.position;
                    return;
                }
                create_next_level(newLevelPosition, newEntryDirection, true);
            }
        }
    }

    private void create_next_level(Vector2 level_position, string entryDirection, bool attachable = false)
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        if (selected_levels.Count == 9)
        {
            return;
        }
        
        int level_num = UnityEngine.Random.Range(0, levels.Length);
        while (selected_levels.Contains(level_num))
        {
            level_num = UnityEngine.Random.Range(0, levels.Length);
        };
        GameObject level = Instantiate(levels[level_num], level_position, Quaternion.identity);
        GameObject entryGroup = level.transform.Find("Entries").gameObject;
        Transform[] entries = entryGroup.GetComponentsInChildren<Transform>();
        bool correctEntryInRoom = false;
        Transform correctEntry = null ;
        foreach (Transform entry in entries)
        {
            if (entry.name == entryDirection)
            {
                correctEntryInRoom = true;
                correctEntry = entry;
                correctEntry.gameObject.SetActive(false);
                break;
            }
        }
        if (!correctEntryInRoom)
        {
            //Destroy(level);
            //create_next_level(level_position, entryDirection, attachable, true);
            generateAgain = true;
            return;
        }
        level.transform.position = level_position - (Vector2)correctEntry.position;
        level.transform.position += (Vector3)level_position - correctEntry.position;  
                
        selected_levels.Add(level_num);
        existing_levels.Add(level);
        if (attachable)
        {
            open_levels.Add(level);
            return;
        }
        List<Transform> disabledEntries = new List<Transform>();
        disabledEntries.Add(correctEntry);
        create_attached_level(entries, disabledEntries);
        return;
    }

    void Start()
    {
        selected_levels = new List<int>();
        open_levels = new List<GameObject>();
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        Vector2 initialPosition = new Vector2(0, 0);
        int start_level_num = UnityEngine.Random.Range(0, start_levels.Length);
        GameObject start_level = Instantiate(start_levels[start_level_num], initialPosition, Quaternion.identity);
        Transform entry = start_level.transform.Find("Entry");
        if (start_levels[start_level_num].name == "lvl_start_u")
        {
            //createBridge("Up");
            Vector2 corridorCoords = new Vector2(entry.position.x, entry.position.y + padding);
            GameObject corridor = Instantiate(corridor_v, corridorCoords, Quaternion.identity);
            Vector2 newEntryPosition = corridorCoords + new Vector2(0, 2);
            string newEntryDirection = "Down";
            create_next_level(newEntryPosition, newEntryDirection);
        }
        if (start_levels[start_level_num].name == "lvl_start_d")
        {
            Vector2 corridorCoords = new Vector2(entry.position.x, entry.position.y - padding);
            GameObject corridor = Instantiate(corridor_v, corridorCoords, Quaternion.identity);
            Vector2 newEntryPosition = corridorCoords + new Vector2(0, -2);
            string newEntryDirection = "Up";
            create_next_level(newEntryPosition, newEntryDirection);
        }
        if (start_levels[start_level_num].name == "lvl_start_l")
        {
            Vector2 corridorCoords = new Vector2(entry.position.x - padding, entry.position.y);
            GameObject corridor = Instantiate(corridor_h, corridorCoords, Quaternion.identity);
            Vector2 newEntryPosition = corridorCoords + new Vector2(-2, 0);
            string newEntryDirection = "Right";
            create_next_level(newEntryPosition, newEntryDirection);
        }
        if (start_levels[start_level_num].name == "lvl_start_r")
        {
            Vector2 corridorCoords = new Vector2(entry.position.x + padding, entry.position.y);
            GameObject corridor = Instantiate(corridor_h, corridorCoords, Quaternion.identity);
            Vector2 newEntryPosition = corridorCoords + new Vector2(2, 0);
            string newEntryDirection = "Left";
            create_next_level(newEntryPosition, newEntryDirection);
        }
        while (selected_levels.Count < 9 && !generateAgain)
        {
            GameObject level = open_levels[0];
            GameObject entryGroup = level.transform.Find("Entries").gameObject;
            Transform[] entries = entryGroup.GetComponentsInChildren<Transform>();
            List<Transform> disabledEntries = new List<Transform>();
            foreach (Transform e in entries)
            {
                if (e.gameObject.activeSelf == false)
                {
                    disabledEntries.Add(e);
                }
            }
            if (disabledEntries.Count() == entries.Length)
                open_levels.RemoveAt(0);
            create_attached_level(entries, disabledEntries);
        }
        if (selected_levels.Count == 9 && !generateAgain)
        {
            GameObject level = open_levels[0];
            GameObject entryGroup = level.transform.Find("Entries").gameObject;
            Transform[] entries = entryGroup.GetComponentsInChildren<Transform>();
            List<Transform> disabledEntries = new List<Transform>();
            while (true)
            {
                level = open_levels[0];
                entryGroup = level.transform.Find("Entries").gameObject;
                entries = entryGroup.GetComponentsInChildren<Transform>();
                disabledEntries = new List<Transform>();
                foreach (Transform e in entries)
                {
                    if (e.gameObject.activeSelf == false)
                    {
                        disabledEntries.Add(e);
                    }
                }
                if (disabledEntries.Count() == entries.Length)
                    open_levels.RemoveAt(0);
                else
                    break;
            }
            
            create_attached_level(entries, disabledEntries, true);

        }
        
        if (generateAgain)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
