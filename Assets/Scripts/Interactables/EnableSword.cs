using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSword : Interactable
{
    [SerializeField] private PresentationPage presentationPage;
    
    protected override void interact(Collider other)
    {
        base.interact(other);
        //other.GetComponent<BasicDash>().EnableSelf(true);

        GameStateManager.Instance.StartSlideshow(new []{presentationPage} );

    }
}