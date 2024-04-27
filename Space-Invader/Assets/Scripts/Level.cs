using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.name == "Tilemap")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {

    }
}
