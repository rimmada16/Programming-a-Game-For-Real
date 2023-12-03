using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameStateManager : Singleton<GameStateManager>
{
    public bool isPaused;
    public GameObject pauseMenu;
    
    
    public bool isDead;
    public GameObject deathMenu;

    public GameObject kunaiUi;
    public GameObject dashUi;
    
    
    public GameObject checkpointButton;
    public GameObject checkpointButton2;

    public bool isCutscene;
    public GameObject imagePage;
    public GameObject imagePageBackdrop;

    public PresentationPage[] presentationPageQueue;
    public int presentationPageNumber;
    
    public PresentationPage[] startCutscene;

    //static to access between different scenes
    public static bool isHardcore;
    public GameObject hardModeIndicator;

    public GameObject player;

    public static bool startAtCheckpoint;

    
    // Start is called before the first frame update
    void Start()
    {
        SetPause(false);
        SetDeathMenu(false);
        
        player = GameObject.FindWithTag("Player");

        if ( startAtCheckpoint)
        {
            if (!isHardcore)
            {
                CheckpointManager.Instance.StartAtCheckpoint(player);
            }
            startAtCheckpoint = false;
        }
        
        checkpointButton.SetActive(!isHardcore);
        checkpointButton2.SetActive(!isHardcore);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isDead && !isCutscene)
            {
                SetPause(!isPaused);
            }
            
        }
        
        // !! if continue key and last index, make it exit
        // !! if skip key, make it exit
        // !! if continue key and there is more, go to next page
        if (isCutscene && !(presentationPageNumber >= presentationPageQueue.Length))
        {
            PresentationUpdate();
            
        }
    }

    void PresentationUpdate()
    {
        if (Input.GetKeyDown(presentationPageQueue[presentationPageNumber].keyToContinue))
        {
            NextSlide();
            return;
        }
        if (Input.GetKeyDown(presentationPageQueue[presentationPageNumber].keyToSkipAll))
        {
            CloseSlide();
            return;
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

    public void StartSlideshow(PresentationPage[] newPres)
    {
        if (newPres.Length == 0)
        { return;
        }
        
        isCutscene = true;
        isPaused = true;
        imagePage.SetActive(true);

        menuTimeStopAndCursorShow(true,false);

        presentationPageQueue = newPres;
        
        
        if (presentationPageNumber >= presentationPageQueue.Length)
        {
            CloseSlide();
            return;
        }
        
        imagePage.GetComponent<UnityEngine.UI.Image>().sprite  = 
            presentationPageQueue[presentationPageNumber].currentImage;
        
        imagePageBackdrop.SetActive(presentationPageQueue[presentationPageNumber].useBackdrop );
    }

    public void NextSlide()
    {
        presentationPageNumber++;
        if (presentationPageNumber >= presentationPageQueue.Length)
        {
            CloseSlide();
            return;
        }
        
        imagePage.GetComponent<UnityEngine.UI.Image>().sprite = presentationPageQueue[presentationPageNumber].currentImage;
        imagePageBackdrop.SetActive(presentationPageQueue[presentationPageNumber].useBackdrop );
    }

    public void CloseSlide()
    {
        presentationPageQueue = null;
        presentationPageNumber = 0;
        isCutscene = false;
        
        isPaused = false;
        imagePage.SetActive(false);
        imagePageBackdrop.SetActive(false);

        menuTimeStopAndCursorShow(false);

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
        
        startAtCheckpoint = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public void menuTimeStopAndCursorShow(bool menuMode, bool doCursorLock = true)
    {
        if (menuMode)
        {
            
            Time.timeScale = 0;

            if (doCursorLock)
            {
                
                Cursor.lockState = CursorLockMode.None;
            }
            // Locks the cursor upon script start
            // Documentation used: https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
            // This line was added for the Discord Git test
        }
        else
        {
            
            Time.timeScale = 1;
            if (doCursorLock)
            {

                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}


