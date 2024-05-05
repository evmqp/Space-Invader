using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

/// <summary>
/// Class for main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;

    /// <summary>
    /// Start new game
    /// </summary>
    /// 
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            QuitGame();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Save player data and quit game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        StartCoroutine(PressAnyKey());
    }

    private IEnumerator PressAnyKey()
    {
        yield return new WaitUntil(() => 
        Input.anyKey 
        && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2) 
        && !Input.GetKey(KeyCode.Escape));
        PlayGame();
    }
}
