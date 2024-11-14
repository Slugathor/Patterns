using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : GenericSingleton<MenuManager>
{

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        // if we're not in menu scene and press esc
        
    }
    public static void StartGame()
    {
        GameManager.gameState = GameState.PLAYING;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public static void ExitGame()
    {
        Debug.LogWarning("Closing the program.");
        Application.Quit();
    }
    public static void TogglePauseGame()
    {
        if (GameManager.gameState == GameState.PLAYING) // if playing, pause
        {
            GameManager.gameState = GameState.PAUSED;
            Time.timeScale = 0f;
        }
        else if(GameManager.gameState == GameState.PAUSED) // else continue
        {
            GameManager.gameState = GameState.PLAYING;
            Time.timeScale = 1f;
        }
    }
    public static void GoToMainMenu()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
        {
            GameManager.gameState = GameState.MENU;
            SceneManager.LoadScene(0);
        }
    }
}
