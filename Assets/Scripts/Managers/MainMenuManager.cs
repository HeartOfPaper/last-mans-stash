using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastMansStash.Managers
{
    /// <summary>
    /// Manages Main Menu scene logic and button callbacks.
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LastMansStash.Networking.RoomManager roomManager;
        [SerializeField] private LastMansStash.UI.MainMenu.MainMenuUI mainMenuUI;

        private void Awake()
        {
            // Get room manager
            if (roomManager == null)
            {
                roomManager = FindObjectOfType<LastMansStash.Networking.RoomManager>();
            }

            // Get UI
            if (mainMenuUI == null)
            {
                mainMenuUI = FindObjectOfType<LastMansStash.UI.MainMenu.MainMenuUI>();
            }
        }

        private void Start()
        {
            // Subscribe to room manager events
            if (roomManager != null)
            {
                roomManager.OnRoomCreated += HandleRoomCreated;
                roomManager.OnRoomJoined += HandleRoomJoined;
                roomManager.OnRoomCreateFailed += HandleRoomFailed;
                roomManager.OnRoomJoinFailed += HandleRoomFailed;
            }

            Debug.Log("[MainMenuManager] Main Menu initialized");
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (roomManager != null)
            {
                roomManager.OnRoomCreated -= HandleRoomCreated;
                roomManager.OnRoomJoined -= HandleRoomJoined;
                roomManager.OnRoomCreateFailed -= HandleRoomFailed;
                roomManager.OnRoomJoinFailed -= HandleRoomFailed;
            }
        }

        #region Button Callbacks

        public void OnCreateRoomClicked()
        {
            Debug.Log("[MainMenuManager] Create Room clicked");
            
            if (mainMenuUI != null)
            {
                mainMenuUI.ShowLoading("Creating room...");
            }

            roomManager?.CreateRoom();
        }

        public void OnJoinRoomClicked(string roomCode)
        {
            Debug.Log($"[MainMenuManager] Join Room clicked: {roomCode}");

            if (string.IsNullOrEmpty(roomCode))
            {
                if (mainMenuUI != null)
                {
                    mainMenuUI.ShowError("Please enter a room code");
                }
                return;
            }

            if (mainMenuUI != null)
            {
                mainMenuUI.ShowLoading("Joining room...");
            }

            roomManager?.JoinRoom(roomCode);
        }

        public void OnQuickMatchClicked()
        {
            Debug.Log("[MainMenuManager] Quick Match clicked");

            if (mainMenuUI != null)
            {
                mainMenuUI.ShowLoading("Finding room...");
            }

            roomManager?.QuickMatch();
        }

        public void OnSettingsClicked()
        {
            Debug.Log("[MainMenuManager] Settings clicked");
            
            if (mainMenuUI != null)
            {
                mainMenuUI.ShowSettings();
            }
        }

        public void OnQuitClicked()
        {
            Debug.Log("[MainMenuManager] Quit clicked");
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion

        #region Room Manager Event Handlers

        private void HandleRoomCreated(string roomCode)
        {
            Debug.Log($"[MainMenuManager] Room created: {roomCode}");
            TransitionToLobby();
        }

        private void HandleRoomJoined(string roomCode)
        {
            Debug.Log($"[MainMenuManager] Room joined: {roomCode}");
            TransitionToLobby();
        }

        private void HandleRoomFailed(string error)
        {
            Debug.LogError($"[MainMenuManager] Room operation failed: {error}");
            
            if (mainMenuUI != null)
            {
                mainMenuUI.HideLoading();
                mainMenuUI.ShowError(error);
            }
        }

        #endregion

        private void TransitionToLobby()
        {
            Debug.Log("[MainMenuManager] Transitioning to Lobby scene");
            SceneManager.LoadScene("Lobby");
        }
    }
}
