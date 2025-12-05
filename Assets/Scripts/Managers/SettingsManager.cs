using UnityEngine;

namespace LastMansStash.Managers
{
    /// <summary>
    /// Manages game settings and PlayerPrefs persistence.
    /// Singleton pattern for global settings access.
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        // Singleton
        public static SettingsManager Instance { get; private set; }

        #region Audio Settings

        [Header("Audio Settings")]
        [SerializeField] [Range(0f, 100f)] private float masterVolume = 100f;
        [SerializeField] [Range(0f, 100f)] private float musicVolume = 80f;
        [SerializeField] [Range(0f, 100f)] private float sfxVolume = 100f;
        [SerializeField] private bool muteAll = false;

        public float MasterVolume => masterVolume;
        public float MusicVolume => musicVolume;
        public float SFXVolume => sfxVolume;
        public bool MuteAll => muteAll;

        #endregion

        #region Graphics Settings

        [Header("Graphics Settings")]
        [SerializeField] private int qualityLevel = 2; // 0=Low, 1=Medium, 2=High, 3=Ultra
        [SerializeField] private bool vSyncEnabled = true;
        [SerializeField] private bool fullscreen = true;
        [SerializeField] private int resolutionIndex = 0;

        public int QualityLevel => qualityLevel;
        public bool VSyncEnabled => vSyncEnabled;
        public bool Fullscreen => fullscreen;
        public int ResolutionIndex => resolutionIndex;

        #endregion

        #region Gameplay Settings

        [Header("Gameplay Settings")]
        [SerializeField] private string playerName = "Player";
        [SerializeField] private bool turnTimerEnabled = true;
        [SerializeField] private bool colorblindMode = false;

        public string PlayerName => playerName;
        public bool TurnTimerEnabled => turnTimerEnabled;
        public bool ColorblindMode => colorblindMode;

        #endregion

        #region Controls Settings

        [Header("Controls Settings")]
        [SerializeField] [Range(0.1f, 2.0f)] private float cameraSensitivity = 1.0f;
        [SerializeField] private bool invertYAxis = false;

        public float CameraSensitivity => cameraSensitivity;
        public bool InvertYAxis => invertYAxis;

        #endregion

        private void Awake()
        {
            // Singleton setup
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #region Audio Settings Methods

        public void SetMasterVolume(float value)
        {
            masterVolume = Mathf.Clamp(value, 0f, 100f);
            SaveSettings();
            ApplyAudioSettings();
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp(value, 0f, 100f);
            SaveSettings();
            ApplyAudioSettings();
        }

        public void SetSFXVolume(float value)
        {
            sfxVolume = Mathf.Clamp(value, 0f, 100f);
            SaveSettings();
            ApplyAudioSettings();
        }

        public void SetMuteAll(bool mute)
        {
            muteAll = mute;
            SaveSettings();
            ApplyAudioSettings();
        }

        private void ApplyAudioSettings()
        {
            // TODO: Implement audio mixer control in Phase 13
            Debug.Log($"[SettingsManager] Audio: Master={masterVolume}, Music={musicVolume}, SFX={sfxVolume}, Mute={muteAll}");
        }

        #endregion

        #region Graphics Settings Methods

        public void SetQualityLevel(int level)
        {
            qualityLevel = Mathf.Clamp(level, 0, 3);
            SaveSettings();
            ApplyGraphicsSettings();
        }

        public void SetVSync(bool enabled)
        {
            vSyncEnabled = enabled;
            SaveSettings();
            ApplyGraphicsSettings();
        }

        public void SetFullscreen(bool enabled)
        {
            fullscreen = enabled;
            SaveSettings();
            ApplyGraphicsSettings();
        }

        public void SetResolution(int index)
        {
            resolutionIndex = index;
            SaveSettings();
            ApplyGraphicsSettings();
        }

        private void ApplyGraphicsSettings()
        {
            // Apply quality level
            QualitySettings.SetQualityLevel(qualityLevel);

            // Apply VSync
            QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;

            // Apply fullscreen
            Screen.fullScreen = fullscreen;

            // Apply resolution
            if (resolutionIndex >= 0 && resolutionIndex < Screen.resolutions.Length)
            {
                Resolution resolution = Screen.resolutions[resolutionIndex];
                Screen.SetResolution(resolution.width, resolution.height, fullscreen);
            }

            Debug.Log($"[SettingsManager] Graphics: Quality={qualityLevel}, VSync={vSyncEnabled}, Fullscreen={fullscreen}");
        }

        #endregion

        #region Gameplay Settings Methods

        public void SetPlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("[SettingsManager] Player name cannot be empty");
                return;
            }

            // Limit to 16 characters
            playerName = name.Substring(0, Mathf.Min(name.Length, 16));
            SaveSettings();
            
            // Update Photon nickname
            if (Photon.Pun.PhotonNetwork.IsConnected)
            {
                Photon.Pun.PhotonNetwork.NickName = playerName;
            }

            Debug.Log($"[SettingsManager] Player name set to: {playerName}");
        }

        public void SetTurnTimer(bool enabled)
        {
            turnTimerEnabled = enabled;
            SaveSettings();
        }

        public void SetColorblindMode(bool enabled)
        {
            colorblindMode = enabled;
            SaveSettings();
            Debug.Log($"[SettingsManager] Colorblind mode: {enabled}");
        }

        #endregion

        #region Controls Settings Methods

        public void SetCameraSensitivity(float sensitivity)
        {
            cameraSensitivity = Mathf.Clamp(sensitivity, 0.1f, 2.0f);
            SaveSettings();
        }

        public void SetInvertYAxis(bool invert)
        {
            invertYAxis = invert;
            SaveSettings();
        }

        #endregion

        #region PlayerPrefs Persistence

        private void LoadSettings()
        {
            // Audio
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 100f);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 80f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 100f);
            muteAll = PlayerPrefs.GetInt("MuteAll", 0) == 1;

            // Graphics
            qualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
            vSyncEnabled = PlayerPrefs.GetInt("VSync", 1) == 1;
            fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", Screen.resolutions.Length - 1);

            // Gameplay
            playerName = PlayerPrefs.GetString("PlayerName", "Player");
            turnTimerEnabled = PlayerPrefs.GetInt("TurnTimer", 1) == 1;
            colorblindMode = PlayerPrefs.GetInt("ColorblindMode", 0) == 1;

            // Controls
            cameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity", 1.0f);
            invertYAxis = PlayerPrefs.GetInt("InvertYAxis", 0) == 1;

            Debug.Log("[SettingsManager] Settings loaded from PlayerPrefs");
            
            // Apply settings
            ApplyGraphicsSettings();
            ApplyAudioSettings();
            
            // Set Photon nickname
            if (Photon.Pun.PhotonNetwork.IsConnected)
            {
                Photon.Pun.PhotonNetwork.NickName = playerName;
            }
        }

        private void SaveSettings()
        {
            // Audio
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.SetInt("MuteAll", muteAll ? 1 : 0);

            // Graphics
            PlayerPrefs.SetInt("QualityLevel", qualityLevel);
            PlayerPrefs.SetInt("VSync", vSyncEnabled ? 1 : 0);
            PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);

            // Gameplay
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.SetInt("TurnTimer", turnTimerEnabled ? 1 : 0);
            PlayerPrefs.SetInt("ColorblindMode", colorblindMode ? 1 : 0);

            // Controls
            PlayerPrefs.SetFloat("CameraSensitivity", cameraSensitivity);
            PlayerPrefs.SetInt("InvertYAxis", invertYAxis ? 1 : 0);

            PlayerPrefs.Save();
        }

        public void ResetToDefaults()
        {
            // Audio
            masterVolume = 100f;
            musicVolume = 80f;
            sfxVolume = 100f;
            muteAll = false;

            // Graphics
            qualityLevel = 2;
            vSyncEnabled = true;
            fullscreen = true;
            resolutionIndex = Screen.resolutions.Length - 1;

            // Gameplay
            playerName = "Player";
            turnTimerEnabled = true;
            colorblindMode = false;

            // Controls
            cameraSensitivity = 1.0f;
            invertYAxis = false;

            SaveSettings();
            ApplyGraphicsSettings();
            ApplyAudioSettings();

            Debug.Log("[SettingsManager] Settings reset to defaults");
        }

        #endregion
    }
}
