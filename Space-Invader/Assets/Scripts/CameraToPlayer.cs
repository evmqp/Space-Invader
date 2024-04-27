using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using System;

/// <summary>
/// Camera following to player
/// </summary>
public class cameraToPlayer : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;
    private bool mapIsOpened = false;
    public float currentScrollDelta = 100f;
    public float speed;
    public float scrollSpeed;
    public float defaultScale = 5f;


    private void Start()
    {
        
    }
    void Update()
    {
        if (!mapIsOpened)
        {
            transform.position = player.position + offset;
            Camera.main.orthographicSize = defaultScale;
        }
           
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mapIsOpened = !mapIsOpened;
        }

        if (mapIsOpened)
        {
            currentScrollDelta -= Input.mouseScrollDelta.y * scrollSpeed;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }

            currentScrollDelta = Math.Max(currentScrollDelta, defaultScale);

            Camera.main.orthographicSize = currentScrollDelta;

        }
    }
}