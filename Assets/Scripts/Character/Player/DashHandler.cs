using UnityEngine;

namespace Character.Player
{
    /// <summary>
    /// Handles the dash flag for the dash system
    /// </summary>
    public class DashHandler : MonoBehaviour
    {
        private DashSystem dashSystem;

        /// <summary>
        /// Grabs the BasicDash component at the start
        /// </summary>
        private void Start()
        {
            if (dashSystem == null)
            {
                dashSystem = GetComponentInParent<DashSystem>();
            }
        }

        /// <summary>
        /// Handles the collision with the wall
        /// </summary>
        /// <param name="other">The trigger we collide with</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Wall") && dashSystem.enabled)
            {
                Debug.Log("we runnin");
                StopCoroutine(dashSystem.Dash());
                dashSystem.DashTimeCounter = 0;
                dashSystem.CanDash = false;
            }
        }

        /// <summary>
        /// Handles the exit of the collision with the wall
        /// </summary>
        /// <param name="other">The trigger we collide with</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Wall") && dashSystem.enabled)
            {
                dashSystem.CanDash = true;
            }
        }
    }
}
