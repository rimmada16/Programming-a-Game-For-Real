using System;
using System.Collections;
using System.Collections.Generic;
using Character.Player;
using UnityEngine;

public class EnableDash : Interactable
{
    [SerializeField] private PresentationPage presentationPage;
    
    protected override void interact(Collider other)
    {
        base.interact(other);
        other.GetComponent<DashSystem>().EnableSelf(true);

        GameStateManager.Instance.StartSlideshow(new []{presentationPage} );

    }
}