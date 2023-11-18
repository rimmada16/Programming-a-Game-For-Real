using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : Singleton<TitleScreenManager>
{
    private bool onStartScreen;

    [SerializeField]
    private GameObject difficultyScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        CloseDifficultyScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
        
        if (onStartScreen && Input.anyKeyDown)
        {
            OpenDifficultyScreen();
        }
        
        
    }


    public void LaunchGame(bool isHardcore)
    {
        GameStateManager.isHardcore = isHardcore;
        SceneManager.LoadScene(sceneBuildIndex:1);
    }

    public void OpenDifficultyScreen()
    {
        difficultyScreen.SetActive(true);
        onStartScreen = false;
    }
    public void CloseDifficultyScreen()
    {
        difficultyScreen.SetActive(false);
        onStartScreen = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

