using UnityEngine;
using LastMansStash.Player;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board.TileEffects
{
    /// <summary>
    /// Start Tile - Players gain 5 bucks when landing, 3 when passing.
    /// This is the starting position of the board.
    /// </summary>
    public class StartTile : TileBase
    {
        private void Awake()
        {
            tileType = TileType.Start;
            tileName = "Start";
            description = "Land: Gain 5 bucks | Pass: Gain 3 bucks";
        }

        public override void OnLanded(PlayerData player)
        {
            Debug.Log($"[StartTile] {player.PlayerName} landed on Start tile");
            
            // Gain 5 bucks
            player.AddMoney(Core.GameConstants.START_TILE_LAND_MONEY);
            
            Debug.Log($"[StartTile] {player.PlayerName} gained {Core.GameConstants.START_TILE_LAND_MONEY} bucks (Land bonus)");
        }

        public override void OnPassed(PlayerData player)
        {
            Debug.Log($"[StartTile] {player.PlayerName} passed Start tile");
            
            // Gain 3 bucks
            player.AddMoney(Core.GameConstants.START_TILE_PASS_MONEY);
            
            Debug.Log($"[StartTile] {player.PlayerName} gained {Core.GameConstants.START_TILE_PASS_MONEY} bucks (Pass bonus)");
        }
    }
}
