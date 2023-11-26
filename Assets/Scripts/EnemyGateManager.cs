using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGateManager : MonoBehaviour
{
    public static EnemyGateManager Instance;

    // This needs to be stored in the Checkpoint system
    private int _enemiesKilled;

    // Copy paste for each gate
    [Header("Health Gate One")]
    [SerializeField] private GameObject enemyGateOne;
    [SerializeField] private int enemyGateOneKillAmountRequired;
    
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(_enemiesKilled);
        }
        
        if (_enemiesKilled >= enemyGateOneKillAmountRequired)
        {
            enemyGateOne.SetActive(false);
            // May add a text fade that says something like
            // "A gate has opened somewhere..."
        }
    }

    public void OnEnemyKilled()
    {
        _enemiesKilled++;
    }
    
}
