using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGateManager : Singleton<EnemyGateManager>
{

    [SerializeField] private Text flavourText;
    [SerializeField] private Canvas canvas;

    [SerializeField] private EnemyGateController[] enemyGates;

    private static bool[] gatesOpened;
    private static bool boolArraySetup = false;

    private void Start()
    {
        if (!boolArraySetup)
        {
            gatesOpened = new bool[enemyGates.Length];
            boolArraySetup = true;
        }


        for (int i = 0; i < enemyGates.Length; i++)
        {
            if (gatesOpened[i])
            {
                enemyGates[i].SetEnoughKills();
            }
            enemyGates[i].CheckEnoughKills(true);
        }
    }


    public void GateOpenText()
    {
        var instantiatedText = Instantiate(flavourText, canvas.transform);
        instantiatedText.text = "A gate has opened somewhere...";
        Destroy(instantiatedText, 3f);
    }
    

    public void EnemyDiedAt(int gate)
    {
        if (gate < 0 || gate >= enemyGates.Length)
        {
            return;
        }

        enemyGates[gate].EnemyKilledAtHere();

    }

    public void StoreGateData()
    {
        
        for (int i = 0; i < enemyGates.Length; i++)
        {
            gatesOpened[i] = enemyGates[i].HasOpened();
        }
    }

    public static void ResetGateData()
    {
        boolArraySetup = false;
    }
}
