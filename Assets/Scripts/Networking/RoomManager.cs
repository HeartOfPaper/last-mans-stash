using System;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace LastMansStash.Networking
{
    /// <summary>
    /// Manages room creation, joining, and code generation.
    /// Handles 6-character alphanumeric room codes (A-Z, 0-9).
    /// </summary>
    public class RoomManager : MonoBehaviour
    {
        private const int ROOM_CODE_LENGTH = 6;
        private const int MAX_PLAYERS = 5;
        private const string CODE_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // Callbacks
        public event Action<string> OnRoomCreated;
        public event Action<string> OnRoomJoined;
        public event Action<string> OnRoomCreateFailed;
        public event Action<string> OnRoomJoinFailed;

        /// <summary>
        /// Create a new room with a unique 6-character alphanumeric code
        /// </summary>
        public void CreateRoom()
        {
            if (!PhotonNetwork.IsConnected)
            {
                Debug.LogError("[RoomManager] Cannot create room - not connected to Photon");
                OnRoomCreateFailed?.Invoke("Not connected to server");
                return;
            }

            string roomCode = GenerateRoomCode();
            Debug.Log($"[RoomManager] Creating room with code: {roomCode}");

            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = MAX_PLAYERS,
                IsVisible = true,
                IsOpen = true
            };

            PhotonNetwork.CreateRoom(roomCode, roomOptions);
        }

        /// <summary>
        /// Join an existing room by code
        /// </summary>
        public void JoinRoom(string roomCode)
        {
            if (!PhotonNetwork.IsConnected)
            {
                Debug.LogError("[RoomManager] Cannot join room - not connected to Photon");
                OnRoomJoinFailed?.Invoke("Not connected to server");
                return;
            }

            // Validate and format code
            roomCode = ValidateAndFormatCode(roomCode);
            
            if (string.IsNullOrEmpty(roomCode))
            {
                Debug.LogError("[RoomManager] Invalid room code format");
                OnRoomJoinFailed?.Invoke("Invalid room code format");
                return;
            }

            Debug.Log($"[RoomManager] Attempting to join room: {roomCode}");
            PhotonNetwork.JoinRoom(roomCode);
        }

        /// <summary>
        /// Join a random room or create one if none exist
        /// </summary>
        public void QuickMatch()
        {
            if (!PhotonNetwork.IsConnected)
            {
                Debug.LogError("[RoomManager] Cannot quick match - not connected to Photon");
                OnRoomJoinFailed?.Invoke("Not connected to server");
                return;
            }

            Debug.Log("[RoomManager] Attempting quick match...");
            PhotonNetwork.JoinRandomRoom();
        }

        /// <summary>
        /// Generate a unique 6-character alphanumeric room code
        /// Format: ABC123 (uppercase letters and numbers)
        /// </summary>
        private string GenerateRoomCode()
        {
            char[] code = new char[ROOM_CODE_LENGTH];
            System.Random random = new System.Random();

            for (int i = 0; i < ROOM_CODE_LENGTH; i++)
            {
                code[i] = CODE_CHARS[random.Next(CODE_CHARS.Length)];
            }

            return new string(code);
        }

        /// <summary>
        /// Validate and format room code
        /// - Must be 6 characters
        /// - Only A-Z and 0-9
        /// - Auto-capitalize lowercase letters
        /// </summary>
        public string ValidateAndFormatCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            // Remove whitespace
            code = code.Replace(" ", "").ToUpper();

            // Check length
            if (code.Length != ROOM_CODE_LENGTH)
            {
                Debug.LogWarning($"[RoomManager] Invalid code length: {code.Length} (expected {ROOM_CODE_LENGTH})");
                return null;
            }

            // Check characters are alphanumeric
            if (!code.All(c => CODE_CHARS.Contains(c)))
            {
                Debug.LogWarning("[RoomManager] Code contains invalid characters");
                return null;
            }

            return code;
        }

        /// <summary>
        /// Format code with spacing for display
        /// ABC123 -> ABC 123
        /// </summary>
        public string FormatCodeForDisplay(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length != ROOM_CODE_LENGTH)
                return code;

            return $"{code.Substring(0, 3)} {code.Substring(3, 3)}";
        }

        #region Photon Callbacks (called by NetworkManager events)

        /// <summary>
        /// Called when room is successfully created
        /// </summary>
        public void HandleRoomCreated()
        {
            string roomCode = PhotonNetwork.CurrentRoom?.Name ?? "";
            Debug.Log($"[RoomManager] Room created: {roomCode}");
            OnRoomCreated?.Invoke(roomCode);
        }

        /// <summary>
        /// Called when successfully joined a room
        /// </summary>
        public void HandleRoomJoined()
        {
            string roomCode = PhotonNetwork.CurrentRoom?.Name ?? "";
            Debug.Log($"[RoomManager] Joined room: {roomCode}");
            OnRoomJoined?.Invoke(roomCode);
        }

        /// <summary>
        /// Called when room creation fails
        /// </summary>
        public void HandleRoomCreateFailed(string error)
        {
            Debug.LogError($"[RoomManager] Room creation failed: {error}");
            OnRoomCreateFailed?.Invoke(error);
        }

        /// <summary>
        /// Called when joining room fails
        /// </summary>
        public void HandleRoomJoinFailed(string error)
        {
            Debug.LogError($"[RoomManager] Room join failed: {error}");
            OnRoomJoinFailed?.Invoke(error);
        }

        /// <summary>
        /// Called when quick match fails - create a new room instead
        /// </summary>
        public void HandleQuickMatchFailed()
        {
            Debug.Log("[RoomManager] No rooms available for quick match - creating new room");
            CreateRoom();
        }

        #endregion
    }
}
