using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverOnDeath : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        var HU = GetComponent<HealthUnit>();
        if (HU != null)
        {
            GetComponent<HealthUnit>().OnDeath += CauseGameOver;
        }
    }

    void CauseGameOver()
    {
        //player game over code
        
        GameStateManager.Instance.SetDeathMenu(true);
    }
}
