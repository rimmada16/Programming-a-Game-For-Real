using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyGateController : MonoBehaviour
{
    public int killsExpected;
    public int enemiesKilled;
    [SerializeField] private TMP_Text enemyGateOneText;


    public void EnemyKilledAtHere(bool skipGateText = false)
    {
        if (enemiesKilled >= killsExpected)
        {
            return;
        }

        enemiesKilled++;
        CheckEnoughKills(skipGateText);
        
    }

    public void CheckEnoughKills(bool skipGateText = false)
    {
        if (enemiesKilled >= killsExpected)
        {
            OpenGate();
            if (!skipGateText)
            {
                EnemyGateManager.Instance.DisplayGateOpenText();    
            }
        }
        else
        {
         
            enemyGateOneText.SetText("Something blocks the way... <br>" + enemiesKilled + "/" + killsExpected + "<br>Kills");   
        }

    }

    public void OpenGate()
    {
        gameObject.SetActive(false);
    }

    public void SetEnoughKills()
    {
        enemiesKilled = killsExpected;
        CheckEnoughKills(true);
    }

    public bool HasOpened()
    {
        return (enemiesKilled >= killsExpected);
    }


}
