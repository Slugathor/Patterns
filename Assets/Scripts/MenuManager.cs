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
        if(Input.GetKeyDown(KeyCode.Escape) 
            && SceneManager.GetActiveScene()!=SceneManager.GetSceneByBuildIndex(0)) 
        { 
            SceneManager.LoadScene(0); 
        }
    }
    public static void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public static void ExitGame()
    {
        Debug.LogWarning("Closing the program.");
        Application.Quit();
    }
}
