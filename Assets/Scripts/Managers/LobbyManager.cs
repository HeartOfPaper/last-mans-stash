using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace LastMansStash.Managers
{
    /// <summary>
    /// Manages Lobby scene logic and player list synchronization.
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [Header("Dependencies")]
        [SerializeField] private LastMansStash.UI.Lobby.LobbyUI lobbyUI;

        [Header("Ready System Settings")]
        [Tooltip("Minimum players to start (2 for testing, 4 for production)")]
        [SerializeField] private int minPlayersToStart = 4;

        private Networking.GameNetworkManager networkManager;
        private Networking.ReadySystem readySystem;

        private void Awake()
        {
            networkManager = Networking.GameNetworkManager.Instance;
            
            if (lobbyUI == null)
            {
                lobbyUI = FindObjectOfType<LastMansStash.UI.Lobby.LobbyUI>();
            }

            // Get or create ReadySystem
            readySystem = GetComponent<Networking.ReadySystem>();
            if (readySystem == null)
            {
                readySystem = gameObject.AddComponent<Networking.ReadySystem>();
            }
            
            // Configure ReadySystem
            readySystem.minPlayersToStart = minPlayersToStart;

            // Subscribe to ready system events
            readySystem.OnPlayerReadyChanged += HandlePlayerReadyChanged;
            readySystem.OnCountdownTick += HandleCountdownTick;
            readySystem.OnCountdownCancelled += HandleCountdownCancelled;
            readySystem.OnCountdownComplete += HandleCountdownComplete;
        }

        private void OnDestroy()
        {
            // Unsubscribe from ready system events to prevent memory leaks
            if (readySystem != null)
            {
                readySystem.OnPlayerReadyChanged -= HandlePlayerReadyChanged;
                readySystem.OnCountdownTick -= HandleCountdownTick;
                readySystem.OnCountdownCancelled -= HandleCountdownCancelled;
                readySystem.OnCountdownComplete -= HandleCountdownComplete;
            }
        }

        private void Start()
        {
            // Wait for Photon to confirm we're in a room
            StartCoroutine(WaitForRoomJoin());
        }

        private System.Collections.IEnumerator WaitForRoomJoin()
        {
            // Wait up to 3 seconds for Photon to confirm room join
            float waitTime = 0f;
            while (!PhotonNetwork.InRoom && waitTime < 3f)
            {
                yield return new WaitForSeconds(0.1f);
                waitTime += 0.1f;
            }

            // Verify we're in a room
            if (!PhotonNetwork.InRoom)
            {
                Debug.LogError("[LobbyManager] Timed out waiting for room join! Returning to Main Menu");
                ReturnToMainMenu();
                yield break;
            }

            Debug.Log($"[LobbyManager] Lobby initialized - Room: {PhotonNetwork.CurrentRoom.Name}");
            
            // Configure UI with settings
            if (lobbyUI != null)
            {
                lobbyUI.SetMinPlayersToStart(minPlayersToStart);
            }
            
            // Update UI with current room state
            UpdateUI();
        }

        /// <summary>
        /// Update lobby UI with current player list and room info
        /// </summary>
        private void UpdateUI()
        {
            if (lobbyUI == null) return;

            // Update room code
            string roomCode = PhotonNetwork.CurrentRoom?.Name ?? "";
            lobbyUI.UpdateRoomCode(roomCode);

            // Get players directly from Photon (more reliable than network manager)
            var players = new System.Collections.Generic.List<Photon.Realtime.Player>();
            if (PhotonNetwork.CurrentRoom != null)
            {
                players.AddRange(PhotonNetwork.CurrentRoom.Players.Values);
            }
            
            Debug.Log($"[LobbyManager] Updating UI with {players.Count} players");
            lobbyUI.UpdatePlayerList(players);

            // Update ready button visibility (always show)
            lobbyUI.SetReadyButtonVisible(true);
            
            // Update ready button text based on local player state
            bool isLocalPlayerReady = readySystem?.GetPlayerReady(PhotonNetwork.LocalPlayer) ?? false;
            lobbyUI.UpdateReadyButton(isLocalPlayerReady);
        }

        #region Button Callbacks

        public void OnReadyToggled()
        {
            Debug.Log("[LobbyManager] Ready toggled");
            readySystem?.ToggleReady();
            
            // Update UI immediately
            UpdateUI();
        }

        public void OnLeaveRoomClicked()
        {
            Debug.Log("[LobbyManager] Leaving room...");
            
            if (lobbyUI != null)
            {
                lobbyUI.ShowLoading("Leaving room...");
            }

            // Start timeout coroutine in case callback doesn't fire
            StartCoroutine(LeaveRoomTimeout());

            // Leave room directly (more reliable than through network manager)
            PhotonNetwork.LeaveRoom();
        }

        private System.Collections.IEnumerator LeaveRoomTimeout()
        {
            // Wait 5 seconds for OnLeftRoom callback
            yield return new WaitForSeconds(5f);

            // If still in this scene, force return to main menu
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Lobby")
            {
                Debug.LogWarning("[LobbyManager] Leave room timed out - forcing return to MainMenu");
                ReturnToMainMenu();
            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log($"[LobbyManager] Player entered: {newPlayer.NickName}");
            UpdateUI();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log($"[LobbyManager] Player left: {otherPlayer.NickName}");
            UpdateUI();
        }

        public override void OnLeftRoom()
        {
            Debug.Log("[LobbyManager] OnLeftRoom callback fired!");
            ReturnToMainMenu();
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            Debug.Log($"[LobbyManager] New host: {newMasterClient.NickName}");
            UpdateUI();
        }

        #endregion

        #region Ready System Event Handlers

        private void HandlePlayerReadyChanged(int actorNumber, bool isReady)
        {
            Debug.Log($"[LobbyManager] Player {actorNumber} ready state: {isReady}");
            UpdateUI();
        }

        private void HandleCountdownTick(float remainingTime)
        {
            if (lobbyUI != null)
            {
                lobbyUI.UpdateCountdown(remainingTime);
            }
        }

        private void HandleCountdownCancelled()
        {
            Debug.Log("[LobbyManager] Countdown cancelled");
            if (lobbyUI != null)
            {
                lobbyUI.HideCountdown();
            }
        }

        private void HandleCountdownComplete()
        {
            Debug.Log("[LobbyManager] Countdown complete - starting game!");
            
            if (lobbyUI != null)
            {
                lobbyUI.HideCountdown();
            }

            // TODO: Load Draft scene (Phase 6)
            Debug.Log("[LobbyManager] Would load Draft scene here");
        }

        #endregion

        private void OnDestroy()
        {
            // Unsubscribe from ready system events
            if (readySystem != null)
            {
                readySystem.OnPlayerReadyChanged -= HandlePlayerReadyChanged;
                readySystem.OnCountdownTick -= HandleCountdownTick;
                readySystem.OnCountdownCancelled -= HandleCountdownCancelled;
                readySystem.OnCountdownComplete -= HandleCountdownComplete;
            }
        }

        private void ReturnToMainMenu()
        {
            Debug.Log("[LobbyManager] Returning to Main Menu");
            SceneManager.LoadScene("MainMenu");
        }
    }
}
