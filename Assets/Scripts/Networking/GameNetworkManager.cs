using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace LastMansStash.Networking
{
    /// <summary>
    /// Manages network state and Photon callbacks.
    /// Singleton pattern for global network access.
    /// </summary>
    public class GameNetworkManager : MonoBehaviourPunCallbacks
    {
        // Singleton
        public static GameNetworkManager Instance { get; private set; }

        [Header("Network State")]
        [SerializeField] private bool isConnected = false;
        [SerializeField] private bool isInRoom = false;
        [SerializeField] private string currentRoomCode = "";

        // Public getters
        public bool IsConnected => isConnected;
        public bool IsInRoom => isInRoom;
        public string CurrentRoomCode => currentRoomCode;
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;

        private RoomManager roomManager;

        private void Awake()
        {
            // Singleton setup
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            // Update connection state
            UpdateConnectionState();
            
            // Find RoomManager
            roomManager = FindObjectOfType<RoomManager>();
        }

        /// <summary>
        /// Update connection state based on Photon status
        /// </summary>
        private void UpdateConnectionState()
        {
            isConnected = PhotonNetwork.IsConnected;
            isInRoom = PhotonNetwork.InRoom;
            
            if (isInRoom && PhotonNetwork.CurrentRoom != null)
            {
                currentRoomCode = PhotonNetwork.CurrentRoom.Name;
            }
            else
            {
                currentRoomCode = "";
            }
        }

        #region Photon Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("[NetworkManager] Connected to Photon Master Server");
            isConnected = true;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarning($"[NetworkManager] Disconnected from Photon: {cause}");
            isConnected = false;
            isInRoom = false;
            currentRoomCode = "";
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"[NetworkManager] Joined room: {PhotonNetwork.CurrentRoom.Name}");
            isInRoom = true;
            currentRoomCode = PhotonNetwork.CurrentRoom.Name;
            
            Debug.Log($"[NetworkManager] Players in room: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
        }

        public override void OnLeftRoom()
        {
            Debug.Log("[NetworkManager] Left room");
            isInRoom = false;
            currentRoomCode = "";
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log($"[NetworkManager] Player joined: {newPlayer.NickName}");
            Debug.Log($"[NetworkManager] Room now has {PhotonNetwork.CurrentRoom.PlayerCount} players");
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log($"[NetworkManager] Player left: {otherPlayer.NickName}");
            Debug.Log($"[NetworkManager] Room now has {PhotonNetwork.CurrentRoom.PlayerCount} players");
        }

        public override void OnCreatedRoom()
        {
            Debug.Log($"[GameNetworkManager] Room created successfully: {PhotonNetwork.CurrentRoom.Name}");
            roomManager?.HandleRoomCreated();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"[GameNetworkManager] Failed to create room: {message} (Code: {returnCode})");
            roomManager?.HandleRoomCreateFailed(message);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"[GameNetworkManager] Failed to join room: {message} (Code: {returnCode})");
            roomManager?.HandleRoomJoinFailed(message);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"[GameNetworkManager] No random room available: {message}");
            // This is expected when no rooms exist - RoomManager will create one
            roomManager?.HandleQuickMatchFailed();
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            Debug.Log($"[NetworkManager] New master client: {newMasterClient.NickName}");
        }

        #endregion

        /// <summary>
        /// Get list of all players in current room
        /// </summary>
        public List<Photon.Realtime.Player> GetPlayersInRoom()
        {
            if (!isInRoom || PhotonNetwork.CurrentRoom == null)
            {
                return new List<Photon.Realtime.Player>();
            }

            return new List<Photon.Realtime.Player>(PhotonNetwork.CurrentRoom.Players.Values);
        }

        /// <summary>
        /// Leave current room
        /// </summary>
        public void LeaveRoom()
        {
            if (isInRoom)
            {
                Debug.Log("[NetworkManager] Leaving room...");
                PhotonNetwork.LeaveRoom();
            }
        }
    }
}
