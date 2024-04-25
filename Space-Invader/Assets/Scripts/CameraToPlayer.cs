using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera following to player
/// </summary>
public class cameraToPlayer : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;

    void Update()
    {
        transform.position = player.position + offset;
    }
}