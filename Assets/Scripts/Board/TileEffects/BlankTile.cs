using UnityEngine;
using LastMansStash.Player;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board.TileEffects
{
    /// <summary>
    /// Blank Tile - No effect when landed on or passed.
    /// </summary>
    public class BlankTile : TileBase
    {
        private void Awake()
        {
            tileType = TileType.Blank;
            tileName = "Blank";
            description = "No effect";
        }

        internal override void OnLanded(PlayerData player)
        {
            // No effect
            Debug.Log($"[BlankTile] {player.PlayerName} landed on a Blank tile - no effect");
        }

        internal override void OnPassed(PlayerData player)
        {
            // No effect
        }
    }
}
