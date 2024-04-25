using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public TextMeshProUGUI LevelText;
    public Player player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        LevelText.text = $"{player.level}";
    }

    /// <summary>
    /// Continue game
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    /// <summary>
    /// Stop game
    /// </summary>
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    /// <summary>
    /// Save data and load menu
    /// </summary>
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Save data and quit game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    //public void NewLevel()
    //{
    //    player.level += 1;
    //}
}
