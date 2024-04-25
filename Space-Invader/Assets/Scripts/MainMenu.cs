using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Start new game
    /// </summary>
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
}
