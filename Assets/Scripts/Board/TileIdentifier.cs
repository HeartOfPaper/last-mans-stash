using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board
{
    /// <summary>
    /// Component to identify and mark tiles in the Unity scene.
    /// Attach to tile GameObjects to mark them for BoardManager.
    /// </summary>
    public class TileIdentifier : MonoBehaviour
    {
        [Header("Tile Configuration")]
        [SerializeField] private TileType tileType;
        
        [Header("Visual Debugging")]
        [SerializeField] private Color gizmoColor = Color.yellow;
        [SerializeField] private float gizmoSize = 0.5f;

        public TileType TileType => tileType;

        private void OnDrawGizmos()
        {
            // Draw sphere to show tile position in Scene view
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, gizmoSize);

            // Draw label
#if UNITY_EDITOR
            UnityEditor.Handles.Label(
                transform.position + Vector3.up * (gizmoSize + 0.2f),
                tileType.ToString(),
                new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = gizmoColor },
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                }
            );
#endif
        }

        private void OnDrawGizmosSelected()
        {
            // Highlight selected tile
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, gizmoSize * 1.2f);
        }
    }
}
