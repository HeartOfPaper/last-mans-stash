using UnityEngine;
using LastMansStash.Player;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board.TileEffects
{
    /// <summary>
    /// Pawn Shop Tile - Draw 1 Dagger Card (two-faced card).
    /// Smuggler ability: Draw 2, choose 1 to keep.
    /// </summary>
    public class PawnShopTile : TileBase
    {
        private void Awake()
        {
            tileType = TileType.PawnShop;
            tileName = "Pawn Shop";
            description = "Draw 1 Dagger Card";
        }

        internal override void OnLanded(PlayerData player)
        {
            Debug.Log($"[PawnShopTile] {player.PlayerName} landed on Pawn Shop");

            // Smuggler ability: Draw 2 Dagger Cards, choose 1 to keep
            if (player.Character == CharacterType.TheSmuggler && player.CanUseSmugglerAbility())
            {
                Debug.Log("[PawnShopTile] Smuggler ability: Draw 2 Dagger Cards, choose 1!");
                player.ResetSmugglerCounter();
                
                // TODO: Draw 2 Dagger Cards and show choice UI (Phase 7)
                Debug.Log("[PawnShopTile] Smuggler choice logic will be implemented in Phase 7");
            }
            else
            {
                // Normal: Draw 1 Dagger Card
                Debug.Log("[PawnShopTile] Drawing 1 Dagger Card");
                
                // TODO: Draw 1 Dagger Card from Dagger deck (Phase 7)
                Debug.Log("[PawnShopTile] Dagger Card draw will be implemented in Phase 7");
            }

            // Increment Smuggler rounds
            if (player.Character == CharacterType.TheSmuggler)
            {
                player.IncrementSmugglerRounds();
            }
        }

        internal override void OnPassed(PlayerData player)
        {
            // No effect when passing
        }
    }
}
