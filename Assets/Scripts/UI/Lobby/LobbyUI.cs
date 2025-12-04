using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

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

        [Header("Buttons")]
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button leaveRoomButton;

        [Header("Loading")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI loadingText;

        private Managers.LobbyManager manager;
        private List<GameObject> playerSlots = new List<GameObject>();
        private const int MAX_PLAYERS = 5;

        private void Awake()
        {
            manager = FindObjectOfType<Managers.LobbyManager>();

            // Setup button callbacks
            if (startGameButton != null)
                startGameButton.onClick.AddListener(() => manager?.OnStartGameClicked());
            
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

            // Create slots for each player
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                GameObject slot = CreatePlayerSlot(i);
                
                if (i < players.Count)
                {
                    // Occupied slot
                    Photon.Realtime.Player player = players[i];
                    SetPlayerSlot(slot, player.NickName, player.IsMasterClient);
                }
                else
                {
                    // Empty slot
                    SetPlayerSlot(slot, "Waiting for player...", false, true);
                }

                playerSlots.Add(slot);
            }

            // Update player count
            if (playerCountText != null)
            {
                playerCountText.text = $"{players.Count}/{MAX_PLAYERS} Players";
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

        private void SetPlayerSlot(GameObject slot, string playerName, bool isHost, bool isEmpty = false)
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
                    nameText.text = $"{playerName}{hostTag}";
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
        /// Show/hide Start Game button (host only)
        /// </summary>
        public void SetStartButtonVisible(bool visible)
        {
            if (startGameButton != null)
            {
                startGameButton.gameObject.SetActive(visible);
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
