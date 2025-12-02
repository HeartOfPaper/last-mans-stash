using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastMansStash
{
    /// <summary>
    /// Bootstrap manager - first scene that loads.
    /// Handles Photon connection and transitions to Main Menu.
    /// </summary>
    public class BootstrapManager : MonoBehaviour
    {
        public static BootstrapManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        [SerializeField] private float minimumLoadTime = 1.5f; // Minimum time to show loading screen

        [Header("UI")]
        [SerializeField] private UI.BootstrapUI bootstrapUI;

        private float loadStartTime;
        private bool photonReady = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            loadStartTime = Time.time;
        }

        private void Start()
        {
            Debug.Log("=== BOOTSTRAP STARTED ===");
            
            UpdateStatus("Initializing...");
            
            // Connect to Photon
            if (Networking.PhotonConnector.Instance != null)
            {
                UpdateStatus("Connecting to servers...");
                Networking.PhotonConnector.Instance.Connect();
            }
            else
            {
                Debug.LogError("PhotonConnector not found! Make sure it exists in the Bootstrap scene.");
                UpdateStatus("ERROR: PhotonConnector not found!");
            }
        }

        /// <summary>
        /// Called by PhotonConnector when connected and ready
        /// </summary>
        public void OnPhotonReady()
        {
            photonReady = true;
            Debug.Log("Photon is ready!");
            
            UpdateStatus("Connected! Loading...");
            
            // Check if minimum load time has passed
            float elapsedTime = Time.time - loadStartTime;
            if (elapsedTime >= minimumLoadTime)
            {
                LoadMainMenu();
            }
            else
            {
                // Wait for remaining time
                Invoke(nameof(LoadMainMenu), minimumLoadTime - elapsedTime);
            }
        }

        private void LoadMainMenu()
        {
            Debug.Log($"Loading {mainMenuSceneName} scene...");
            UpdateStatus("Loading main menu...");
            SceneManager.LoadScene(mainMenuSceneName);
        }

        private void UpdateStatus(string status)
        {
            if (bootstrapUI != null)
            {
                bootstrapUI.SetStatus(status);
            }
            Debug.Log($"[Bootstrap] {status}");
        }
    }
}
