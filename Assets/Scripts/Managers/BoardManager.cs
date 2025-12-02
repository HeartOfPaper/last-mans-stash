using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LastMansStash.Board;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Managers
{
    /// <summary>
    /// Manages the game board - finds prebuilt tiles in the scene and orders them spatially.
    /// Uses spatial algorithm to arrange tiles clockwise from Start tile.
    /// </summary>
    public class BoardManager : MonoBehaviour
    {
        [Header("Board Configuration")]
        [SerializeField] private bool autoFindTiles = true;
        [SerializeField] private bool showDebugGizmos = true;

        [Header("Board State (Read-Only)")]
        [SerializeField] private List<Tile> boardTiles = new List<Tile>();
        [SerializeField] private Vector3 boardCenter;
        [SerializeField] private int totalTileCount = 0;

        // Singleton
        public static BoardManager Instance { get; private set; }

        // Public access to board
        public List<Tile> BoardTiles => boardTiles;
        public int TileCount => totalTileCount;
        public Vector3 BoardCenter => boardCenter;

        private void Awake()
        {
            // Singleton setup
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (autoFindTiles)
            {
                FindAndOrderTiles();
            }
        }

        /// <summary>
        /// Find all tiles in scene with TileIdentifier and order them spatially
        /// </summary>
        public void FindAndOrderTiles()
        {
            Debug.Log("[BoardManager] Finding tiles in scene...");

            // Find all TileIdentifier components
            TileIdentifier[] tileIdentifiers = FindObjectsOfType<TileIdentifier>();

            if (tileIdentifiers.Length == 0)
            {
                Debug.LogWarning("[BoardManager] No tiles found in scene! Make sure tile GameObjects have TileIdentifier component.");
                return;
            }

            Debug.Log($"[BoardManager] Found {tileIdentifiers.Length} tiles");

            // Calculate board center (average position of all tiles)
            boardCenter = CalculateBoardCenter(tileIdentifiers);

            // Find Start tile
            TileIdentifier startTile = System.Array.Find(tileIdentifiers, t => t.TileType == TileType.Start);
            if (startTile == null)
            {
                Debug.LogError("[BoardManager] No Start tile found! Board must have exactly one Start tile.");
                return;
            }

            // Order tiles spatially (clockwise from Start)
            List<TileIdentifier> orderedIdentifiers = OrderTilesClockwise(tileIdentifiers, startTile, boardCenter);

            // Convert to Tile data objects
            boardTiles.Clear();
            for (int i = 0; i < orderedIdentifiers.Count; i++)
            {
                TileIdentifier identifier = orderedIdentifiers[i];
                Tile tile = new Tile(identifier.TileType, i, identifier.gameObject);
                boardTiles.Add(tile);
            }

            totalTileCount = boardTiles.Count;

            Debug.Log($"[BoardManager] Board initialized with {totalTileCount} tiles");
            Debug.Log($"[BoardManager] Board center: {boardCenter}");
            Debug.Log($"[BoardManager] Tile order: {string.Join(" → ", boardTiles.Select(t => t.TileType))}");
        }

        /// <summary>
        /// Calculate the center point of all tiles
        /// </summary>
        private Vector3 CalculateBoardCenter(TileIdentifier[] tiles)
        {
            Vector3 sum = Vector3.zero;
            foreach (TileIdentifier tile in tiles)
            {
                sum += tile.transform.position;
            }
            return sum / tiles.Length;
        }

        /// <summary>
        /// Order tiles clockwise from the Start tile around the board center
        /// </summary>
        private List<TileIdentifier> OrderTilesClockwise(TileIdentifier[] tiles, TileIdentifier startTile, Vector3 center)
        {
            // Calculate angle from center for each tile
            Dictionary<TileIdentifier, float> tileAngles = new Dictionary<TileIdentifier, float>();

            foreach (TileIdentifier tile in tiles)
            {
                Vector3 directionFromCenter = tile.transform.position - center;
                // Calculate angle in degrees (0-360)
                float angle = Mathf.Atan2(directionFromCenter.z, directionFromCenter.x) * Mathf.Rad2Deg;
                if (angle < 0) angle += 360f;
                tileAngles[tile] = angle;
            }

            // Get Start tile's angle
            float startAngle = tileAngles[startTile];

            // Normalize all angles relative to Start tile (Start tile becomes 0°)
            foreach (TileIdentifier tile in tiles)
            {
                float relativeAngle = tileAngles[tile] - startAngle;
                if (relativeAngle < 0) relativeAngle += 360f;
                tileAngles[tile] = relativeAngle;
            }

            // Sort by angle (clockwise)
            List<TileIdentifier> sorted = tiles.OrderBy(t => tileAngles[t]).ToList();

            return sorted;
        }

        /// <summary>
        /// Get tile at specific index (wraps around board)
        /// </summary>
        public Tile GetTileAtIndex(int index)
        {
            if (boardTiles.Count == 0) return null;
            
            // Wrap around board
            int wrappedIndex = index % boardTiles.Count;
            if (wrappedIndex < 0) wrappedIndex += boardTiles.Count;
            
            return boardTiles[wrappedIndex];
        }

        /// <summary>
        /// Get tile by type (first occurrence)
        /// </summary>
        public Tile GetTileByType(TileType type)
        {
            return boardTiles.Find(t => t.TileType == type);
        }

        /// <summary>
        /// Get all tiles of specific type
        /// </summary>
        public List<Tile> GetTilesByType(TileType type)
        {
            return boardTiles.FindAll(t => t.TileType == type);
        }

        /// <summary>
        /// Rebuild the board (useful for testing or dynamic boards)
        /// </summary>
        [ContextMenu("Rebuild Board")]
        public void RebuildBoard()
        {
            FindAndOrderTiles();
        }

        /// <summary>
        /// Visual debugging - draw gizmos for board layout
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!showDebugGizmos || boardTiles == null || boardTiles.Count == 0) return;

            // Draw board center
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(boardCenter, 0.5f);

            // Draw lines connecting tiles in order
            Gizmos.color = Color.cyan;
            for (int i = 0; i < boardTiles.Count; i++)
            {
                Tile currentTile = boardTiles[i];
                Tile nextTile = boardTiles[(i + 1) % boardTiles.Count];

                if (currentTile.VisualObject != null && nextTile.VisualObject != null)
                {
                    Vector3 currentPos = currentTile.VisualObject.transform.position;
                    Vector3 nextPos = nextTile.VisualObject.transform.position;
                    
                    Gizmos.DrawLine(currentPos, nextPos);
                    
                    // Draw arrow direction
                    Vector3 direction = (nextPos - currentPos).normalized;
                    Vector3 arrowPoint = currentPos + direction * Vector3.Distance(currentPos, nextPos) * 0.5f;
                    Gizmos.DrawSphere(arrowPoint, 0.2f);
                }
            }

            // Draw Start tile specially
            Tile startTile = boardTiles.Find(t => t.TileType == TileType.Start);
            if (startTile != null && startTile.VisualObject != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(startTile.VisualObject.transform.position, 0.7f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!showDebugGizmos || boardTiles == null || boardTiles.Count == 0) return;

            // Draw index numbers on each tile
            for (int i = 0; i < boardTiles.Count; i++)
            {
                Tile tile = boardTiles[i];
                if (tile.VisualObject != null)
                {
#if UNITY_EDITOR
                    UnityEditor.Handles.Label(
                        tile.VisualObject.transform.position + Vector3.up * 1.5f,
                        $"[{i}] {tile.TileType}",
                        new GUIStyle()
                        {
                            normal = new GUIStyleState() { textColor = Color.white },
                            fontSize = 14,
                            fontStyle = FontStyle.Bold
                        }
                    );
#endif
                }
            }
        }
    }
}
