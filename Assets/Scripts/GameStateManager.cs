using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameStateManager : Singleton<GameStateManager>
{
    public bool isPaused;
    public GameObject pauseMenu;
    
    
    public bool isDead;
    public GameObject deathMenu;

    //static to access between different scenes
    public static bool isHardcore;
    public GameObject hardModeIndicator;

    
    // Start is called before the first frame update
    void Start()
    {
        SetPause(false);
        SetDeathMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isDead == false)
            {
                SetPause(!isPaused);
            }
            
        }
    }

    public void SetPause(bool willPause)
    {
        //code for game being pause
        if (willPause)
        {
            pauseMenu.SetActive(true);
            
            TryShowHardmodeIndicator(true);
            
            isPaused = true;
            
            menuTimeStopAndCursorShow(true);
            
        }
        //code for game resuming
        else
        {
            pauseMenu.SetActive(false);
            
            TryShowHardmodeIndicator(false);
            
            isPaused = false;
            menuTimeStopAndCursorShow(false);
            
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

    public void SetDeathMenu(bool willDeath)
    {
        if (willDeath)
        {
            pauseMenu.SetActive(false);
            
            
            deathMenu.SetActive(true);
            
            TryShowHardmodeIndicator(true);
            isDead = true;
            isPaused = true;
            
            menuTimeStopAndCursorShow(true);
            
        }
        else
        {
            deathMenu.SetActive(false);
            
            TryShowHardmodeIndicator(false);
            isDead = false;
            
            SetPause(false);
        }
    }

    public void SetHardcore(bool newHardcore)
    {
        isHardcore = newHardcore;
    }

    public void TryShowHardmodeIndicator(bool active)
    {
        hardModeIndicator.SetActive(isHardcore && active);
    }
    
    public void ToggleHardcore()
    {
        isHardcore = !isHardcore;
    }
    
    public void ReturnToCheckpoint()
    {
        //code to find a checkpoint and return to it if we do checkpoints
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
    public void ExitToTitle()
    {
        SceneManager.LoadScene(sceneBuildIndex:0);
    }

    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            SetPause(true);
        }
    }

    public void menuTimeStopAndCursorShow(bool menuMode)
    {
        if (menuMode)
        {
            
            Time.timeScale = 0;
            
            Cursor.lockState = CursorLockMode.None;
            // Locks the cursor upon script start
            // Documentation used: https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
            // This line was added for the Discord Git test
        }
        else
        {
            
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}


