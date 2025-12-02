using UnityEngine;
using LastMansStash.Player;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board.TileEffects
{
    /// <summary>
    /// Casino Tile - Triggers the Casino mini-game when landed on.
    /// </summary>
    public class CasinoTile : TileBase
    {
        private void Awake()
        {
            tileType = TileType.Casino;
            tileName = "Casino";
            description = "Trigger Casino mini-game";
        }

        internal override void OnLanded(PlayerData player)
        {
            Debug.Log($"[CasinoTile] {player.PlayerName} landed on Casino - Starting mini-game!");
            
            // TODO: Trigger Casino mini-game (Phase 10)
            // For now, just log
            Debug.Log("[CasinoTile] Casino mini-game will be implemented in Phase 10");
        }

        internal override void OnPassed(PlayerData player)
        {
            // No effect when passing
        }
    }
}
