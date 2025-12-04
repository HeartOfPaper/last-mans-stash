using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LastMansStash.UI.MainMenu
{
    /// <summary>
    /// Handles Main Menu UI elements and visibility.
    /// Wire up UI references in Unity Inspector.
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Main Menu Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject loadingPanel;

        [Header("Buttons")]
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button joinRoomButton;
        [SerializeField] private Button quickMatchButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField roomCodeInput;

        [Header("Loading UI")]
        [SerializeField] private TextMeshProUGUI loadingText;

        [Header("Error Display")]
        [SerializeField] private GameObject errorPanel;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button errorCloseButton;
        [SerializeField] private TextMeshProUGUI errorCloseButtonText;

        private Managers.MainMenuManager manager;
        private bool isConnectionError = false;

        private void Awake()
        {
            manager = FindObjectOfType<Managers.MainMenuManager>();
            
            // Setup room code input validation
            if (roomCodeInput != null)
            {
                roomCodeInput.characterLimit = 6;
                roomCodeInput.onValueChanged.AddListener(OnRoomCodeChanged);
            }

            // Setup button callbacks
            if (createRoomButton != null)
                createRoomButton.onClick.AddListener(() => manager?.OnCreateRoomClicked());
            
            if (joinRoomButton != null)
                joinRoomButton.onClick.AddListener(() => manager?.OnJoinRoomClicked(roomCodeInput?.text));
            
            if (quickMatchButton != null)
                quickMatchButton.onClick.AddListener(() => manager?.OnQuickMatchClicked());
            
            if (settingsButton != null)
                settingsButton.onClick.AddListener(() => manager?.OnSettingsClicked());
            
            if (quitButton != null)
                quitButton.onClick.AddListener(() => manager?.OnQuitClicked());

            if (errorCloseButton != null)
                errorCloseButton.onClick.AddListener(OnErrorButtonClicked);
        }

        private void Start()
        {
            // Show main menu, hide others
            ShowMainMenu();
        }

        /// <summary>
        /// Validate and auto-format room code input
        /// Only allow A-Z and 0-9, auto-capitalize
        /// </summary>
        private void OnRoomCodeChanged(string value)
        {
            if (roomCodeInput == null) return;

            // Auto-capitalize and filter valid characters
            string filtered = "";
            foreach (char c in value.ToUpper())
            {
                if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
                {
                    filtered += c;
                }
            }

            // Update input field if changed
            if (filtered != value)
            {
                roomCodeInput.text = filtered;
            }

            // Enable/disable join button based on valid length
            if (joinRoomButton != null)
            {
                joinRoomButton.interactable = filtered.Length == 6;
            }
        }

        public void ShowMainMenu()
        {
            SetActivePanel(mainMenuPanel);
        }

        public void ShowSettings()
        {
            SetActivePanel(settingsPanel);
        }

        public void ShowLoading(string message = "Loading...")
        {
            if (loadingText != null)
            {
                loadingText.text = message;
            }
            SetActivePanel(loadingPanel);
        }

        public void HideLoading()
        {
            ShowMainMenu();
        }

    public void ShowError(string message)
        {
            if (errorText != null)
            {
                errorText.text = message;
            }
            
            // Detect if this is a connection error
            isConnectionError = message.Contains("Not connected") || 
                               message.Contains("server") || 
                               message.Contains("connection");
            
            // Change button text based on error type
            if (errorCloseButtonText != null)
            {
                errorCloseButtonText.text = isConnectionError ? "RETRY" : "OK";
            }
            
            if (errorPanel != null)
            {
                errorPanel.SetActive(true);
            }
        }

        public void HideError()
        {
            if (errorPanel != null)
            {
                errorPanel.SetActive(false);
            }
            isConnectionError = false;
        }

        private void OnErrorButtonClicked()
        {
            if (isConnectionError)
            {
                // Retry connection
                OnRetryConnection();
            }
            else
            {
                // Just close error and return to main menu
                HideError();
                ShowMainMenu();
            }
        }

        private void OnRetryConnection()
        {
            Debug.Log("[MainMenuUI] Retrying connection - loading Bootstrap scene...");
            
            HideError();
            
            // Just reload Bootstrap scene - it handles all connection logic
            UnityEngine.SceneManagement.SceneManager.LoadScene("Bootstrap");
        }

        private void SetActivePanel(GameObject panel)
        {
            // Hide all panels
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (loadingPanel != null) loadingPanel.SetActive(false);

            // Show requested panel
            if (panel != null) panel.SetActive(true);
        }
    }
}
