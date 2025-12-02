using UnityEngine;
using LastMansStash.Player;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board.TileEffects
{
    /// <summary>
    /// Safehouse Tile - Draw 1 Movement Card from Safehouse deck.
    /// (Can become Hazard during Sting Operation chaos event)
    /// </summary>
    public class SafehouseTile : TileBase
    {
        private bool isHazard = false; // Changed by Sting Operation chaos card

        private void Awake()
        {
            tileType = TileType.Safehouse;
            tileName = "Safehouse";
            description = "Draw 1 Movement Card";
        }

        internal override void OnLanded(PlayerData player)
        {
            if (isHazard)
            {
                // Sting Operation effect: Hazard
                Debug.Log($"[SafehouseTile] {player.PlayerName} landed on Hazard (Sting Operation active)");
                Debug.Log("[SafehouseTile] Player must discard 1 card and move 1 space forward");
                
                // TODO: Implement Hazard logic (Phase 9)
                return;
            }

            Debug.Log($"[SafehouseTile] {player.PlayerName} landed on Safehouse");
            
            // TODO: Draw 1 Movement Card from Safehouse deck (Phase 7)
            Debug.Log("[SafehouseTile] Drawing 1 Movement Card (to be implemented in Phase 7)");

            // Runner ability: Draw 2 instead of 1
            if (player.Character == CharacterType.TheRunner && player.CanUseRunnerAbility())
            {
                Debug.Log("[SafehouseTile] Runner ability: Draw 2 cards instead of 1!");
                player.ResetRunnerCounter();
                // TODO: Draw extra card
            }
        }

        internal override void OnPassed(PlayerData player)
        {
            // No effect when passing
        }

        /// <summary>
        /// Set Hazard mode (called by Sting Operation chaos card)
        /// Internal to prevent unauthorized tile state changes
        /// </summary>
        internal void SetHazard(bool hazard)
        {
            isHazard = hazard;
            tileName = hazard ? "Hazard (Sting Operation)" : "Safehouse";
            description = hazard ? "Discard 1 card, move 1 forward" : "Draw 1 Movement Card";
        }
    }
}
