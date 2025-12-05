using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

namespace LastMansStash.UI.Lobby
{
    /// <summary>
    /// Handles Lobby UI elements - player list, room code, buttons.
    /// </summary>
    public class LobbyUI : MonoBehaviour
    {
        [Header("Room Code Display")]
        [SerializeField] private RoomCodeDisplay roomCodeDisplay;

        [Header("Player List")]
        [SerializeField] private Transform playerListContainer;
        [SerializeField] private GameObject playerSlotPrefab;
        [SerializeField] private TextMeshProUGUI playerCountText;
        [SerializeField] private TextMeshProUGUI readyStatusText; // "Starting when ready [X/Y]" or countdown

        [Header("Buttons")]
        [SerializeField] private Button readyButton;
        [SerializeField] private Button leaveRoomButton;

        [Header("Loading")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI loadingText;

        private Managers.LobbyManager manager;
        private List<GameObject> playerSlots = new List<GameObject>();
        private const int MAX_PLAYERS = 5;
        private int minPlayersToStart = 4; // Set by LobbyManager

        private void Awake()
        {
            manager = FindFirstObjectByType<Managers.LobbyManager>();

            // Setup button callbacks - Ready button instead of Start
            if (readyButton != null)
                readyButton.onClick.AddListener(() => manager?.OnReadyToggled());
            
            if (leaveRoomButton != null)
                leaveRoomButton.onClick.AddListener(() => manager?.OnLeaveRoomClicked());

            // Hide loading by default
            if (loadingPanel != null)
                loadingPanel.SetActive(false);
        }

        /// <summary>
        /// Update room code display
        /// </summary>
        public void UpdateRoomCode(string roomCode)
        {
            if (roomCodeDisplay != null)
            {
                roomCodeDisplay.SetRoomCode(roomCode);
            }
        }

        /// <summary>
        /// Set minimum players required to start (called by LobbyManager)
        /// </summary>
        public void SetMinPlayersToStart(int minPlayers)
        {
            minPlayersToStart = minPlayers;
        }

        /// <summary>
        /// Update player list with current players in room
        /// </summary>
        public void UpdatePlayerList(List<Photon.Realtime.Player> players)
        {
            Debug.Log($"[LobbyUI] UpdatePlayerList called with {players?.Count ?? 0} players");
            
            if (players == null)
            {
                Debug.LogError("[LobbyUI] Player list is null!");
                players = new List<Photon.Realtime.Player>();
            }
            
            // Clear existing slots
            ClearPlayerSlots();

            // Get ready system
            var readySystem = FindFirstObjectByType<LastMansStash.Networking.ReadySystem>();

            // Create slots for each player
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                GameObject slot = CreatePlayerSlot(i);
                
                if (i < players.Count)
                {
                    // Occupied slot
                    Photon.Realtime.Player player = players[i];
                    bool isReady = readySystem?.GetPlayerReady(player) ?? false;
                    SetPlayerSlot(slot, player.NickName, player.IsMasterClient, isReady);
                }
                else
                {
                    // Empty slot
                    SetPlayerSlot(slot, "Waiting for player...", false, false, true);
                }

                playerSlots.Add(slot);
            }

            // Update player count
            if (playerCountText != null)
            {
                playerCountText.text = $"{players.Count}/{MAX_PLAYERS} Players";
            }

            // Update readyStatusText
            if (readyStatusText != null)
            {
                // Check if we have minimum players
                if (players.Count < minPlayersToStart)
                {
                    // Not enough players
                    readyStatusText.text = $"<size=28><color=#888888>Waiting for players... </color></size>";
                }
                else
                {
                    // Enough players - show ready count
                    int readyCount = 0;
                    foreach (var player in players)
                    {
                        if (readySystem?.GetPlayerReady(player) ?? false)
                        {
                            readyCount++;
                        }
                    }
                    readyStatusText.text = $"<size=28>Starting when ready [{readyCount}/{players.Count}]</size>";
                }
            }
        }

        private GameObject CreatePlayerSlot(int index)
        {
            GameObject slot;
            
            if (playerSlotPrefab != null && playerListContainer != null)
            {
                slot = Instantiate(playerSlotPrefab, playerListContainer);
            }
            else
            {
                // Fallback if no prefab
                slot = new GameObject($"PlayerSlot_{index}");
                if (playerListContainer != null)
                    slot.transform.SetParent(playerListContainer);
            }

            return slot;
        }

        private void SetPlayerSlot(GameObject slot, string playerName, bool isHost, bool isReady, bool isEmpty = false)
        {
            // Find text component
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (nameText != null)
            {
                if (isEmpty)
                {
                    nameText.text = $"<color=#888888>{playerName}</color>";
                }
                else
                {
                    string hostTag = isHost ? " <color=#FFD700>[HOST]</color>" : "";
                    string readyIndicator = isReady ? " <color=#00FF00>[READY]</color>" : "";
                    nameText.text = $"{playerName}{hostTag}{readyIndicator}";
                }
            }

            // TODO: Add player avatar/icon in Phase 6
        }

        private void ClearPlayerSlots()
        {
            foreach (GameObject slot in playerSlots)
            {
                if (slot != null)
                    Destroy(slot);
            }
            playerSlots.Clear();
        }

        /// <summary>
        /// Show/hide ready button
        /// </summary>
        public void SetReadyButtonVisible(bool visible)
        {
            if (readyButton != null)
            {
                readyButton.gameObject.SetActive(visible);
            }
        }

        /// <summary>
        /// Update ready button text and color based on local player ready state
        /// </summary>
        public void UpdateReadyButton(bool isReady)
        {
            if (readyButton != null)
            {
                // Update text
                var buttonText = readyButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = isReady ? "UNREADY" : "READY";
                }

                // Update color using Image component
                var buttonImage = readyButton.GetComponent<Image>();
                if (buttonImage != null)
                {
                    if (isReady)
                    {
                        // Green when ready
                        buttonImage.color = new Color(0f, 0.7f, 0f, 1f);
                    }
                    else
                    {
                        // Red when not ready
                        buttonImage.color = new Color(0.8f, 0f, 0f, 1f);
                    }
                }

                // Also update ColorBlock for hover effects
                var colors = readyButton.colors;
                if (isReady)
                {
                    // Green colors
                    colors.normalColor = new Color(0f, 0.7f, 0f, 1f);
                    colors.highlightedColor = new Color(0f, 0.85f, 0f, 1f);
                    colors.pressedColor = new Color(0f, 0.5f, 0f, 1f);
                }
                else
                {
                    // Red colors
                    colors.normalColor = new Color(0.8f, 0f, 0f, 1f);
                    colors.highlightedColor = new Color(0.9f, 0.1f, 0.1f, 1f);
                    colors.pressedColor = new Color(0.6f, 0f, 0f, 1f);
                }
                readyButton.colors = colors;
            }
        }

        /// <summary>
        /// Show countdown timer
        /// </summary>
        public void UpdateCountdown(float remainingTime)
        {
            if (readyStatusText != null)
            {
                int seconds = Mathf.CeilToInt(remainingTime);
                readyStatusText.text = $"<color=#FFD700><size=48>Starting in {seconds}...</size></color>";
            }
        }

        /// <summary>
        /// Hide countdown and restore ready count
        /// </summary>
        public void HideCountdown()
        {
            if (readyStatusText != null)
            {
                // Restore ready count display
                var readySystem = FindFirstObjectByType<LastMansStash.Networking.ReadySystem>();
                int playerCount = PhotonNetwork.CurrentRoom?.PlayerCount ?? 0;
                int readyCount = 0;
                
                if (PhotonNetwork.CurrentRoom != null && readySystem != null)
                {
                    foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                    {
                        if (readySystem.GetPlayerReady(player))
                        {
                            readyCount++;
                        }
                    }
                }
                
                readyStatusText.text = $"<size=28>Starting when ready [{readyCount}/{playerCount}]</size>";
            }
        }

        public void ShowLoading(string message = "Loading...")
        {
            if (loadingText != null)
                loadingText.text = message;
            
            if (loadingPanel != null)
                loadingPanel.SetActive(true);
        }

        public void HideLoading()
        {
            if (loadingPanel != null)
                loadingPanel.SetActive(false);
        }
    }
}
