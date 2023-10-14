using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : Singleton<GameStateManager>
{
    public bool isPaused;
    public GameObject pauseMenu;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetPause(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause(!isPaused);
        }
    }

    public void SetPause(bool willPause)
    {

        //code for game being pause
        if (willPause)
        {
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true);
            }
            Time.timeScale = 0;
            isPaused = true;
            
            Cursor.lockState = CursorLockMode.None;
            // Locks the cursor upon script start
            // Documentation used: https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
            // This line was added for the Discord Git test
            
        }
        //code for game resuming
        else
        {
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
            Time.timeScale = 1;
            isPaused = false;
            
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ShowSettings(bool willOpen)
    {
        if (willOpen)
        {
            //opening settings code here
            
        }
        else
        {
            //closing settings code here
        }
    }

    public void RestartGame()
    {
        //code to restart game here
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            SetPause(true);
        }
    }
}


