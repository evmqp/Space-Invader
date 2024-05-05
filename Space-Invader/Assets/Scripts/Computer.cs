using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Specifies the actions that occur when working on the computer
/// </summary>
public class Computer : MonoBehaviour
{
    public TextMeshProUGUI ComputerText; /*!< Text on computer display */
    public GameObject ComputerMonitor; /*!< Computer display */
    public GameObject door; /*!< A Door between rooms */
    public bool computerIsOn; /*!< Is a computer switched on? */
    public bool actionCompleted = false; /*!< Is the action performed by the computer completed? */
    public bool isStartRoom = false;
    private GameObject gameMenu;

    private void Start()
    {
        ComputerMonitor.SetActive(false);
        foreach (Transform text in ComputerMonitor.transform)
        {
            if (text.name != ComputerText.name)
            {
                text.gameObject.SetActive(false);
            }
        }
        ComputerText.enabled = false;
        gameMenu = GameObject.Find("GameMenu");
    }

    private void Update()
    {
        if (computerIsOn && !actionCompleted)
            if ((isStartRoom && Input.GetKeyDown(KeyCode.Escape)) || (!isStartRoom && Input.GetKeyDown(KeyCode.E)))
            {
                Destroy(door);
                ComputerMonitor.SetActive(false);
                ComputerText.enabled = false;
                actionCompleted = true;
            }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !actionCompleted)
        {
            ComputerMonitor.SetActive(true);
            computerIsOn = true;
            ComputerText.enabled = true;
            gameMenu.SetActive(false);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ComputerMonitor.SetActive(false);
            computerIsOn = false;
            ComputerText.enabled = false;
            gameMenu.SetActive(true);
        }
    }


}
