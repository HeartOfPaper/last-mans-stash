using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace LastMansStash.Networking
{
    /// <summary>
    /// Manages the Photon connection lifecycle.
    /// Handles connecting, disconnecting, and connection callbacks.
    /// </summary>
    public class PhotonConnector : MonoBehaviourPunCallbacks
    {
        public static PhotonConnector Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private string gameVersion = "1.0";

        public bool IsConnected => PhotonNetwork.IsConnected;
        public bool IsConnectedAndReady => PhotonNetwork.IsConnectedAndReady;

        private void Awake()
        {
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

            // Set Photon settings
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = gameVersion;
        }

        /// <summary>
        /// Connect to Photon using settings from PhotonServerSettings
        /// </summary>
        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Already connected to Photon");
                return;
            }

            Debug.Log("Connecting to Photon...");
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// Disconnect from Photon
        /// </summary>
        public void Disconnect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }

        #region Photon Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log($"<color=green>Connected to Photon Master Server (Region: {PhotonNetwork.CloudRegion})</color>");
            
            // Join the default lobby
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Photon Lobby");
            
            // Notify BootstrapManager that we're ready
            if (BootstrapManager.Instance != null)
            {
                BootstrapManager.Instance.OnPhotonReady();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarning($"Disconnected from Photon: {cause}");
        }

        public override void OnLeftLobby()
        {
            Debug.Log("Left Photon Lobby");
        }

        #endregion
    }
}
