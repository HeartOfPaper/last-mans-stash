using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board
{
    /// <summary>
    /// Data class representing a single tile on the board.
    /// Holds tile properties and references.
    /// </summary>
    [System.Serializable]
    public class Tile
    {
        [Header("Tile Properties")]
        [SerializeField] private TileType tileType;
        [SerializeField] private int index; // Position in board array
        [SerializeField] private string tileName;

        [Header("References")]
        [SerializeField] private GameObject visualObject; // 3D model or sprite

        // Properties
        public TileType TileType => tileType;
        public int Index => index;
        public string TileName => tileName;
        public GameObject VisualObject => visualObject;

        // Constructor
        public Tile(TileType type, int tileIndex, GameObject visual = null)
        {
            tileType = type;
            index = tileIndex;
            visualObject = visual;
            tileName = type.ToString();
        }

        // Set visual object (for runtime assignment)
        // Internal to prevent unauthorized tile manipulation
        internal void SetVisualObject(GameObject visual)
        {
            visualObject = visual;
        }

        // Set index (for board ordering)
        // Internal to prevent unauthorized tile manipulation
        internal void SetIndex(int newIndex)
        {
            index = newIndex;
        }

        public override string ToString()
        {
            return $"Tile [{index}]: {tileType}";
        }
    }
}
