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
    public bool actionCompleted; /*!< Is the action performed by the computer completed? */

    private void Start()
    {
        ComputerMonitor.SetActive(false);
        //ComputerText.enabled = false;
    }

    private void Update()
    {
        if (computerIsOn && Input.GetKeyDown(KeyCode.Q) && !actionCompleted)
        {
            Destroy(door);
            ComputerMonitor.SetActive(false);
            actionCompleted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !actionCompleted)
        {
            ComputerMonitor.SetActive(true);
            computerIsOn = true;
            //ComputerText.enabled = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ComputerMonitor.SetActive(false);
            computerIsOn = false;
            //ComputerText.enabled = false;
        }
    }


}
