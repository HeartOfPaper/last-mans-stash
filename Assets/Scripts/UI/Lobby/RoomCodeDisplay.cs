using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LastMansStash.UI.Lobby
{
    /// <summary>
    /// Displays the 6-character room code with formatting.
    /// Auto-formats as "ABC 123" for readability.
    /// </summary>
    public class RoomCodeDisplay : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI codeText;
        [SerializeField] private Button copyButton;

        [Header("Display Settings")]
        [SerializeField] private bool useSpacing = true; // Format as "ABC 123" instead of "ABC123"

        private string currentRoomCode = "";

        private void Awake()
        {
            // Setup copy button
            if (copyButton != null)
            {
                copyButton.onClick.AddListener(CopyToClipboard);
            }
        }

        /// <summary>
        /// Set and display the room code
        /// </summary>
        public void SetRoomCode(string roomCode)
        {
            currentRoomCode = roomCode;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (codeText == null) return;

            if (string.IsNullOrEmpty(currentRoomCode))
            {
                codeText.text = "------";
                return;
            }

            // Format code for display
            string displayCode = currentRoomCode;
            
            if (useSpacing && currentRoomCode.Length == 6)
            {
                // Format as "ABC 123"
                displayCode = $"{currentRoomCode.Substring(0, 3)} {currentRoomCode.Substring(3, 3)}";
            }

            codeText.text = displayCode;
        }

        /// <summary>
        /// Copy room code to clipboard
        /// </summary>
        private void CopyToClipboard()
        {
            if (string.IsNullOrEmpty(currentRoomCode))
            {
                Debug.LogWarning("[RoomCodeDisplay] No room code to copy");
                return;
            }

            GUIUtility.systemCopyBuffer = currentRoomCode;
            Debug.Log($"[RoomCodeDisplay] Copied room code to clipboard: {currentRoomCode}");

            // TODO: Show "Copied!" feedback in Phase 11
        }
    }
}
