using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public TextMeshProUGUI ComputerText;
    public GameObject ComputerMonitor;
    public GameObject door;
    public bool computerIsOn;
    public bool actionCompleted;

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
