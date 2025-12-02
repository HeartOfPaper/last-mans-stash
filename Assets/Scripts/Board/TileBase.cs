using UnityEngine;
using LastMansStash.Player;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board
{
    /// <summary>
    /// Abstract base class for all tile types.
    /// Each tile can have effects when landed on or passed.
    /// </summary>
    public abstract class TileBase : MonoBehaviour
    {
        [Header("Tile Configuration")]
        [SerializeField] protected TileType tileType;
        [SerializeField] protected string tileName;
        [TextArea(2, 4)]
        [SerializeField] protected string description;

        // Properties
        public TileType TileType => tileType;
        public string TileName => tileName;
        public string Description => description;

        /// <summary>
        /// Called when a player lands exactly on this tile
        /// </summary>
        /// <param name="player">The player who landed</param>
        public abstract void OnLanded(PlayerData player);

        /// <summary>
        /// Called when a player passes this tile without landing
        /// </summary>
        /// <param name="player">The player who passed</param>
        public abstract void OnPassed(PlayerData player);

        /// <summary>
        /// Can this tile be landed on? (Some tiles might be disabled)
        /// </summary>
        public virtual bool CanLandOn()
        {
            return true;
        }

        /// <summary>
        /// Optional visual feedback when player is on this tile
        /// </summary>
        protected virtual void ShowHighlight()
        {
            // Override in derived classes for visual feedback
        }

        /// <summary>
        /// Hide highlight
        /// </summary>
        protected virtual void HideHighlight()
        {
            // Override in derived classes
        }
    }
}
