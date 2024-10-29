using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the enemy gates in the game
/// </summary>
public class EnemyGateManager : Singleton<EnemyGateManager>
{
    [SerializeField] private Text flavourText;
    [SerializeField] private Canvas canvas;

    [SerializeField] private EnemyGateController[] enemyGates;
    
    private static GateData[] _gatesData;
    private static bool _boolArraySetup = false;

    /// <summary>
    /// Contains the data for the gates
    /// </summary>
    [Serializable]
    public struct GateData
    {
        public bool isOpened;

        public GateData(bool isOpened)
        {
            this.isOpened = isOpened;
        }
    }
    
    /// <summary>
    /// Handles the initial setup of the gates
    /// </summary>
    private void Start()
    {
        if (!_boolArraySetup)
        {
            _gatesData = new GateData[enemyGates.Length];
            _boolArraySetup = true;
        }
        
        for (int i = 0; i < enemyGates.Length; i++)
        {
            if (_gatesData[i].isOpened)
            {
                enemyGates[i].SetEnoughKills();
            }
            enemyGates[i].CheckEnoughKills(true);
        }
    }

    /// <summary>
    /// Handles the display of the gate open text
    /// </summary>
    public void DisplayGateOpenText()
    {
        var instantiatedText = Instantiate(flavourText, canvas.transform);
        instantiatedText.text = "A gate has opened somewhere...";
        Destroy(instantiatedText, 3f);
    }
    

    /// <summary>
    /// Registers the death of an enemy at a gate
    /// </summary>
    /// <param name="gate"></param>
    public void RegisterEnemyDeathAtGate(int gate)
    {
        if (gate < 0 || gate >= enemyGates.Length)
        {
            return;
        }

        enemyGates[gate].EnemyKilledAtHere();
    }

    /// <summary>
    /// Handles the storing of the gate data
    /// </summary>
    public void StoreGateData()
    {
        for (int i = 0; i < enemyGates.Length; i++)
        {
            _gatesData[i].isOpened = enemyGates[i].HasOpened();
        }
    }

    /// <summary>
    /// Resets the gate data
    /// </summary>
    public static void ResetGateData()
    {
        _boolArraySetup = false;
        for (int i = 0; i < _gatesData.Length; i++)
        {
            _gatesData[i] = new GateData(false);
        }
    }
}
