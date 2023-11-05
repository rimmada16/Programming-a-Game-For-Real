using UnityEngine;
using UnityEngine.UI;

public class TimeTrial : MonoBehaviour
{
    [SerializeField] private GameObject startGate;
    [SerializeField] private GameObject endGate;
    [SerializeField] private GameObject endDoor;
    
    // If the player enters the section then decides to leave
    [SerializeField] private GameObject entranceEndGate;

    [SerializeField] private float timer;
    private float _originalTimerValue;
    
    private bool _startGateTriggered = false;
    
    public Text timerText;
    [SerializeField] private GameObject timerTextUI;
    
    // Update is called once per frame
    void Start()
    {
        _originalTimerValue = timer;
    }
    
    void Update()
    {
        // UI Stuff
        float currentTimerValue = timer;
        timerText.text = "Escape: " + currentTimerValue.ToString("F2");
        
        // Timer stuff
        if (timer > 0 && _startGateTriggered)
        {
            timer -= Time.deltaTime;
            
            // Few seconds of sorrow
            if (timer <= 3f)
            {
                endDoor.SetActive(true); 
            }
            
            // Death
            if (timer <= 0)
            {
                // death stuff
                Debug.Log("The player died");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Starts the timer when the player interacts with the startGate trigger collider
        if (other.gameObject == startGate)
        {
            Debug.Log("Time trial has started");
            endDoor.SetActive(false);
            timerTextUI.SetActive(true);
            _startGateTriggered = true;
        }

        // Stops the timer when the player interacts with the endGate trigger collider
        if (other.gameObject == endGate)
        {
            Debug.Log("Time trial has ended");
            _startGateTriggered = false;
            timerTextUI.SetActive(false);
        }

        // For when the player enters the section then leaves
        if (other.gameObject == entranceEndGate)
        {
            //Debug.Log(entranceEndGate + " activated");
            
            timer = _originalTimerValue; 
            _startGateTriggered = false;
            timerTextUI.SetActive(false);
            
            //Debug.Log(_startGateTriggered);
        }
    }
}