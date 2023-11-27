using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableKunai : Interactable
{
    [SerializeField] private PresentationPage presentationPage;

    protected override void interact(Collider other)
    {
        base.interact(other);
        other.GetComponent<PlayerProjectile>().EnableSelf(true);
        GameStateManager.Instance.StartSlideshow(new []{presentationPage} );
    }
}