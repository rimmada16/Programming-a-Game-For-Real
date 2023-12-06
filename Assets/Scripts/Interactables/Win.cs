using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : Interactable
{
    [SerializeField] private PresentationPage presentationPage;

    private bool won;
    
    protected override void interact(Collider other)
    {
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }
        base.interact(other);
        //other.GetComponent<BasicDash>().EnableSelf(true);
        won = true;

        GameStateManager.Instance.StartSlideshow(new []{presentationPage} );

    }

    private void Update()
    {
        if (won)
        {
            if (GameStateManager.Instance.isPaused)
            {
                return;
            }

            GameStateManager.Instance.SetWinMenu(true);
        }
    }
}