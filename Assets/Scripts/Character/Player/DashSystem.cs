using System.Collections;
using UnityEngine;

namespace Character.Player
{
    /// <summary>
    /// Handles the dash mechanic for the player
    /// </summary>
    public class DashSystem : MonoBehaviour
    {
        private PlayerController _playerController;
        private CharacterController _characterController;

        [SerializeField] [Range(20f, 50f)] private float dashSpeed = 30f;
        [SerializeField] [Range(1f, 10f)] private float dashCooldownMax = 3f;

        private float _dashCooldownCounter;
        [SerializeField] private float dashTimeMax = 0.3f; // Duration of the dash

        private float _dashTimeCounter = 1f;
        public float DashTimeCounter
        {
            get { return _dashTimeCounter; }
            set { _dashTimeCounter = value; }
        }

        private bool _canDash = true;
        public bool CanDash
        {
            get { return _canDash; }
            set { _canDash = value; }
        }

        [SerializeField] private ValueGrabber rechargeBarUI;
        [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;

        public delegate void GeneralHandler();
        public event GeneralHandler OnDash;

        /// <summary>
        /// Grabs the PlayerController and sets the dash cooldown UI bar to the max value
        /// </summary>
        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _characterController = GetComponent<CharacterController>();
            
            if (_playerController == null)
            {
                Debug.LogError("PlayerController is null in DashSystem.cs");
            }
            
            if (_characterController == null)
            {
                Debug.LogError("CharacterController is null in DashSystem.cs");
            }
            
            if (rechargeBarUI != null)
            {
                rechargeBarUI.SetInputMinMax(dashCooldownMax, 0);
            }
            else
            {
                Debug.LogError("ValueGrabber reference is missing.");
            }
        }

        /// <summary>
        /// Decrements the dash cooldown timer and checks for input to dash and if we can dash
        /// </summary>
        private void Update()
        {
            // Exit out if we are paused
            if (GameStateManager.Instance.isPaused)
            {
                return;
            }

            // Slowing decrementing the dash cooldown timer and updating the stamina bar
            if (_dashCooldownCounter > 0)
            {
                _dashCooldownCounter -= Time.deltaTime;
                rechargeBarUI.SetValue(_dashCooldownCounter);
            }

            if (Input.GetKey(dashKey) && _dashCooldownCounter <= 0 && _canDash)
            {
                // Sets the dash to a cooldown of 1s
                _dashCooldownCounter = dashCooldownMax;

                // Prevent moving while dashing
                _playerController.lockMovement = true;

                OnDash?.Invoke();
                StartCoroutine(Dash());
            }
        }

        /// <summary>
        /// Executes the dash mechanic via translation and locks movement during the dash
        /// </summary>
        /// <returns></returns>
        public IEnumerator Dash()
        {
            var startTime = Time.time;
            _dashTimeCounter = dashTimeMax;

            // Dash forward for a set amount of time whilst we can dash and are not paused
            while (Time.time < startTime + _dashTimeCounter && !GameStateManager.Instance.isPaused && _canDash)
            {
                transform.Translate(Vector3.forward * (dashSpeed * Time.deltaTime));
                yield return null;
            }

            // Re-enable movement after the dash has finished
            if (Time.time >= startTime + _dashTimeCounter)
            {
                _playerController.lockMovement = false;
            }
        }

        /// <summary>
        /// Enable the dash and its corresponding UI
        /// </summary>
        /// <param name="nowEnable"></param>
        public void EnableSelf(bool nowEnable)
        {
            if (!enabled == nowEnable)
            {
                GameStateManager.Instance.dashUi.SetActive(nowEnable);
                enabled = nowEnable;
            }
        }
    }
}
