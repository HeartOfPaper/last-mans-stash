using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LastMansStash.UI
{
    /// <summary>
    /// Simple loading UI for the Bootstrap scene.
    /// Shows connection status and loading animation.
    /// </summary>
    public class BootstrapUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Image loadingSpinner;
        
        [Header("Animation")]
        [SerializeField] private float spinSpeed = 180f; // degrees per second

        private void Update()
        {
            // Rotate the loading spinner
            if (loadingSpinner != null)
            {
                loadingSpinner.transform.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Update the status text
        /// </summary>
        public void SetStatus(string status)
        {
            if (statusText != null)
            {
                statusText.text = status;
            }
        }
    }
}
