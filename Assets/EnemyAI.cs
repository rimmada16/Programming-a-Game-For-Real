using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private AIBehaviour currentBehaviour;

    private bool alerted;
    private bool lastAlerted;
    
    // Start is called before the first frame update
    void Start()
    {
        SetBehaviour(new AIIdle(), false);
    }

    // Update is called once per frame
    void Update()
    {
        //check that alert state has changed
        if (alerted != lastAlerted)
        {
            //changed to be alert
            if (alerted )
            {
                SetBehaviour(new AIAlerted());
            }
            //changed to not be alert
            else
            {
                SetBehaviour(new AIIdle());
            }
        }
    }
    
    

    private void SetBehaviour(AIBehaviour newBehaviour, bool doExit = true)
    {
        if (doExit)
        {
            
            currentBehaviour.ExitBehaviour();
        }
        currentBehaviour = newBehaviour;
        currentBehaviour.EnterBehaviour();
    }
}
