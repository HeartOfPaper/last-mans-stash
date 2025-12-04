using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LastMansStash.Managers;

namespace LastMansStash.UI.MainMenu
{
    /// <summary>
    /// Handles Settings UI elements and interactions.
    /// Organized into tabs: Audio, Graphics, Gameplay, Controls.
    /// </summary>
    public class SettingsUI : MonoBehaviour
    {
        [Header("Tab Buttons")]
        [SerializeField] private Button audioTabButton;
        [SerializeField] private Button graphicsTabButton;
        [SerializeField] private Button gameplayTabButton;
        [SerializeField] private Button controlsTabButton;

        [Header("Tab Panels")]
        [SerializeField] private GameObject audioPanel;
        [SerializeField] private GameObject graphicsPanel;
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject controlsPanel;

        [Header("Audio Controls")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Toggle muteAllToggle;

        [Header("Graphics Controls")]
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private Toggle vSyncToggle;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        [Header("Gameplay Controls")]
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private Toggle turnTimerToggle;
        [SerializeField] private Toggle colorblindModeToggle;

        [Header("Controls Settings")]
        [SerializeField] private Slider cameraSensitivitySlider;
        [SerializeField] private Toggle invertYAxisToggle;

        [Header("Action Buttons")]
        [SerializeField] private Button saveButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button backButton;

        private SettingsManager settingsManager;

        private void Awake()
        {
            settingsManager = SettingsManager.Instance;

            // Setup tab buttons
            if (audioTabButton != null)
                audioTabButton.onClick.AddListener(() => ShowTab(audioPanel));
            if (graphicsTabButton != null)
                graphicsTabButton.onClick.AddListener(() => ShowTab(graphicsPanel));
            if (gameplayTabButton != null)
                gameplayTabButton.onClick.AddListener(() => ShowTab(gameplayPanel));
            if (controlsTabButton != null)
                controlsTabButton.onClick.AddListener(() => ShowTab(controlsPanel));

            // Setup action buttons
            if (saveButton != null)
                saveButton.onClick.AddListener(OnSaveClicked);
            if (resetButton != null)
                resetButton.onClick.AddListener(OnResetClicked);
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClicked);

            // Setup change listeners
            SetupAudioListeners();
            SetupGraphicsListeners();
            SetupGameplayListeners();
            SetupControlsListeners();
        }

        private void OnEnable()
        {
            LoadCurrentSettings();
            ShowTab(audioPanel); // Default to Audio tab
        }

        #region Setup Listeners

        private void SetupAudioListeners()
        {
            if (masterVolumeSlider != null)
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            if (muteAllToggle != null)
                muteAllToggle.onValueChanged.AddListener(OnMuteAllChanged);
        }

        private void SetupGraphicsListeners()
        {
            if (qualityDropdown != null)
                qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
            if (vSyncToggle != null)
                vSyncToggle.onValueChanged.AddListener(OnVSyncChanged);
            if (fullscreenToggle != null)
                fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
            if (resolutionDropdown != null)
                resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }

        private void SetupGameplayListeners()
        {
            if (playerNameInput != null)
                playerNameInput.onEndEdit.AddListener(OnPlayerNameChanged);
            if (turnTimerToggle != null)
                turnTimerToggle.onValueChanged.AddListener(OnTurnTimerChanged);
            if (colorblindModeToggle != null)
                colorblindModeToggle.onValueChanged.AddListener(OnColorblindModeChanged);
        }

        private void SetupControlsListeners()
        {
            if (cameraSensitivitySlider != null)
                cameraSensitivitySlider.onValueChanged.AddListener(OnCameraSensitivityChanged);
            if (invertYAxisToggle != null)
                invertYAxisToggle.onValueChanged.AddListener(OnInvertYAxisChanged);
        }

        #endregion

        #region Load Current Settings

        private void LoadCurrentSettings()
        {
            if (settingsManager == null) return;

            // Audio
            if (masterVolumeSlider != null)
                masterVolumeSlider.value = settingsManager.MasterVolume;
            if (musicVolumeSlider != null)
                musicVolumeSlider.value = settingsManager.MusicVolume;
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.value = settingsManager.SFXVolume;
            if (muteAllToggle != null)
                muteAllToggle.isOn = settingsManager.MuteAll;

            // Graphics
            if (qualityDropdown != null)
                qualityDropdown.value = settingsManager.QualityLevel;
            if (vSyncToggle != null)
                vSyncToggle.isOn = settingsManager.VSyncEnabled;
            if (fullscreenToggle != null)
                fullscreenToggle.isOn = settingsManager.Fullscreen;
            if (resolutionDropdown != null)
                PopulateResolutions();

            // Gameplay
            if (playerNameInput != null)
                playerNameInput.text = settingsManager.PlayerName;
            if (turnTimerToggle != null)
                turnTimerToggle.isOn = settingsManager.TurnTimerEnabled;
            if (colorblindModeToggle != null)
                colorblindModeToggle.isOn = settingsManager.ColorblindMode;

            // Controls
            if (cameraSensitivitySlider != null)
                cameraSensitivitySlider.value = settingsManager.CameraSensitivity;
            if (invertYAxisToggle != null)
                invertYAxisToggle.isOn = settingsManager.InvertYAxis;
        }

        private void PopulateResolutions()
        {
            if (resolutionDropdown == null) return;

            resolutionDropdown.ClearOptions();
            var options = new System.Collections.Generic.List<string>();

            foreach (Resolution res in Screen.resolutions)
            {
                options.Add($"{res.width} x {res.height}");
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = settingsManager.ResolutionIndex;
        }

        #endregion

        #region Change Handlers

        private void OnMasterVolumeChanged(float value) => settingsManager?.SetMasterVolume(value);
        private void OnMusicVolumeChanged(float value) => settingsManager?.SetMusicVolume(value);
        private void OnSFXVolumeChanged(float value) => settingsManager?.SetSFXVolume(value);
        private void OnMuteAllChanged(bool value) => settingsManager?.SetMuteAll(value);

        private void OnQualityChanged(int value) => settingsManager?.SetQualityLevel(value);
        private void OnVSyncChanged(bool value) => settingsManager?.SetVSync(value);
        private void OnFullscreenChanged(bool value) => settingsManager?.SetFullscreen(value);
        private void OnResolutionChanged(int value) => settingsManager?.SetResolution(value);

        private void OnPlayerNameChanged(string value) => settingsManager?.SetPlayerName(value);
        private void OnTurnTimerChanged(bool value) => settingsManager?.SetTurnTimer(value);
        private void OnColorblindModeChanged(bool value) => settingsManager?.SetColorblindMode(value);

        private void OnCameraSensitivityChanged(float value) => settingsManager?.SetCameraSensitivity(value);
        private void OnInvertYAxisChanged(bool value) => settingsManager?.SetInvertYAxis(value);

        #endregion

        #region Button Handlers

        private void OnSaveClicked()
        {
            Debug.Log("[SettingsUI] Settings saved");
            // Settings are auto-saved on change, this is just for user feedback
            OnBackClicked();
        }

        private void OnResetClicked()
        {
            Debug.Log("[SettingsUI] Resetting to defaults");
            settingsManager?.ResetToDefaults();
            LoadCurrentSettings();
        }

        private void OnBackClicked()
        {
            // Return to main menu
            FindObjectOfType<MainMenuUI>()?.ShowMainMenu();
        }

        #endregion

        private void ShowTab(GameObject panel)
        {
            // Hide all tabs
            if (audioPanel != null) audioPanel.SetActive(false);
            if (graphicsPanel != null) graphicsPanel.SetActive(false);
            if (gameplayPanel != null) gameplayPanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(false);

            // Show selected tab
            if (panel != null) panel.SetActive(true);
        }
    }
}
