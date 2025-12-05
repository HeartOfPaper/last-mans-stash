using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace LastMansStash.Networking
{
    /// <summary>
    /// Manages player ready states and countdown timer using Photon Custom Properties.
    /// </summary>
    public class ReadySystem : MonoBehaviourPunCallbacks
    {
        // Minimum players to start - set by LobbyManager
        private int _minPlayersToStart = 4;
        
        public int minPlayersToStart
        {
            get { return _minPlayersToStart; }
            set { _minPlayersToStart = value; }
        }
        
        private const string READY_PROPERTY = "isReady";
        private const string COUNTDOWN_PROPERTY = "countdownStartTime";
        private const float COUNTDOWN_DURATION = 5f;

        // Events
        public System.Action<int, bool> OnPlayerReadyChanged; // (actorNumber, isReady)
        public System.Action<float> OnCountdownTick; // (remainingTime)
        public System.Action OnCountdownCancelled;
        public System.Action OnCountdownComplete;

        private bool isCountdownActive = false;
        private float countdownStartTime = 0f;
        private float lastToggleTime = 0f;
        private const float TOGGLE_COOLDOWN = 0.5f; // Half-second cooldown for security

        private void Update()
        {
            if (isCountdownActive)
            {
                UpdateCountdown();
            }
        }

        /// <summary>
        /// Toggle local player's ready state
        /// </summary>
        public void ToggleReady()
        {
            if (!PhotonNetwork.InRoom) return;

            // Rate limiting to prevent spam
            if (Time.time - lastToggleTime < TOGGLE_COOLDOWN)
            {
                Debug.LogWarning("[ReadySystem] Please wait before toggling ready again");
                return;
            }
            lastToggleTime = Time.time;

            bool currentReady = GetPlayerReady(PhotonNetwork.LocalPlayer);
            SetPlayerReady(!currentReady);
        }

        /// <summary>
        /// Set local player's ready state
        /// </summary>
        public void SetPlayerReady(bool ready)
        {
            if (!PhotonNetwork.InRoom) return;

            Hashtable props = new Hashtable();
            props[READY_PROPERTY] = ready;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            Debug.Log($"[ReadySystem] Local player ready: {ready}");

            // Check if all players ready
            if (PhotonNetwork.IsMasterClient)
            {
                CheckAllPlayersReady();
            }
        }

        /// <summary>
        /// Get player's ready state
        /// </summary>
        public bool GetPlayerReady(Photon.Realtime.Player player)
        {
            if (player.CustomProperties.ContainsKey(READY_PROPERTY))
            {
                return (bool)player.CustomProperties[READY_PROPERTY];
            }
            return false;
        }

        /// <summary>
        /// Check if all players are ready (Master Client only)
        /// </summary>
        private void CheckAllPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (PhotonNetwork.CurrentRoom == null) return; // Null safety
            
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            
            // Need minimum players (4) or all 5 if room is full
            if (playerCount < _minPlayersToStart)
            {
                // Not enough players - cancel countdown if active
                if (isCountdownActive)
                {
                    CancelCountdown();
                }
                return;
            }

            // Check if ALL players are ready (4 out of 4, or 5 out of 5)
            bool allReady = true;
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (!GetPlayerReady(player))
                {
                    allReady = false;
                    break;
                }
            }

            if (allReady && !isCountdownActive)
            {
                // Start countdown
                StartCountdown();
            }
            else if (!allReady && isCountdownActive)
            {
                // Cancel countdown
                CancelCountdown();
            }
        }

        /// <summary>
        /// Start countdown (Master Client only)
        /// </summary>
        private void StartCountdown()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            Debug.Log("[ReadySystem] Starting countdown - all players ready!");

            Hashtable roomProps = new Hashtable();
            roomProps[COUNTDOWN_PROPERTY] = (float)PhotonNetwork.Time;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
        }

        /// <summary>
        /// Cancel countdown (Master Client only)
        /// </summary>
        private void CancelCountdown()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            Debug.Log("[ReadySystem] Countdown cancelled - not all players ready");

            Hashtable roomProps = new Hashtable();
            roomProps[COUNTDOWN_PROPERTY] = -1f;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
        }

        /// <summary>
        /// Update countdown timer
        /// </summary>
        private void UpdateCountdown()
        {
            float elapsed = (float)PhotonNetwork.Time - countdownStartTime;
            float remaining = COUNTDOWN_DURATION - elapsed;

            if (remaining > 0)
            {
                OnCountdownTick?.Invoke(remaining);
            }
            else
            {
                // Countdown complete - stop it immediately to prevent restart
                isCountdownActive = false;
                
                // Master Client clears the countdown property and unreadies all players
                if (PhotonNetwork.IsMasterClient)
                {
                    Hashtable roomProps = new Hashtable();
                    roomProps[COUNTDOWN_PROPERTY] = -1f;
                    PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
                    
                    // Unready all players for next game
                    UnreadyAllPlayers();
                }
                
                OnCountdownComplete?.Invoke();
                Debug.Log("[ReadySystem] Countdown complete - starting game!");
            }
        }

        /// <summary>
        /// Unready all players (Master Client only)
        /// </summary>
        private void UnreadyAllPlayers()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            Debug.Log("[ReadySystem] Unreadying all players");

            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                Hashtable props = new Hashtable();
                props[READY_PROPERTY] = false;
                player.SetCustomProperties(props);
            }
        }

        #region Photon Callbacks

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(READY_PROPERTY))
            {
                bool isReady = (bool)changedProps[READY_PROPERTY];
                Debug.Log($"[ReadySystem] Player {targetPlayer.NickName} ready: {isReady}");
                
                OnPlayerReadyChanged?.Invoke(targetPlayer.ActorNumber, isReady);

                // Master client checks if all ready
                if (PhotonNetwork.IsMasterClient)
                {
                    CheckAllPlayersReady();
                }
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(COUNTDOWN_PROPERTY))
            {
                float startTime = (float)propertiesThatChanged[COUNTDOWN_PROPERTY];
                
                if (startTime > 0)
                {
                    // Countdown started
                    countdownStartTime = startTime;
                    isCountdownActive = true;
                    Debug.Log($"[ReadySystem] Countdown started at {startTime}");
                }
                else
                {
                    // Countdown cancelled
                    isCountdownActive = false;
                    OnCountdownCancelled?.Invoke();
                    Debug.Log("[ReadySystem] Countdown cancelled");
                }
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            // New player not ready by default
            if (PhotonNetwork.IsMasterClient)
            {
                CheckAllPlayersReady();
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            // Recheck ready states
            if (PhotonNetwork.IsMasterClient)
            {
                CheckAllPlayersReady();
            }
        }

        #endregion
    }
}
