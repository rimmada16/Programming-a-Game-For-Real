using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGateManager : MonoBehaviour
{
    public static EnemyGateManager Instance;

    // This needs to be stored in the Checkpoint system
    private int _enemiesKilled;

    [SerializeField] private Text flavourText;
    [SerializeField] private Canvas canvas;

    // Copy paste for each gate
    [Header("Health Gate One")]
    [SerializeField] private GameObject enemyGateOne;
    [SerializeField] private int enemyGateOneKillAmountRequired;
    [SerializeField] private TMP_Text enemyGateOneText;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(_enemiesKilled);
        }

        // Gate one
        if (enemyGateOne.activeSelf)
        {
            GateOne();
        }
    }

    private void GateOne()
    {
        if (_enemiesKilled < enemyGateOneKillAmountRequired)
        {
            enemyGateOneText.SetText("Something blocks the way... <br>" + _enemiesKilled + "/" + enemyGateOneKillAmountRequired + "<br>Kills");
        }

        if (_enemiesKilled >= enemyGateOneKillAmountRequired)
        {
            enemyGateOne.SetActive(false);
            FlavourText();
        }
    }

    private void FlavourText()
    {
        var instantiatedText = Instantiate(flavourText, canvas.transform);
        instantiatedText.text = "A gate has opened somewhere...";
        Destroy(instantiatedText, 3f);
    }
    
    public void OnEnemyKilled()
    {
        _enemiesKilled++;
    }
}
