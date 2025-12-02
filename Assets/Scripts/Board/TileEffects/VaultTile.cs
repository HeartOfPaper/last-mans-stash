using UnityEngine;
using LastMansStash.Player;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board.TileEffects
{
    /// <summary>
    /// Vault Tile - Steal bucks from another player.
    /// Amount depends on if Grifter ability is active.
    /// </summary>
    public class VaultTile : TileBase
    {
        private void Awake()
        {
            tileType = TileType.Vault;
            tileName = "Vault";
            description = "Steal 5 bucks from another player (8 for Grifter)";
        }

        internal override void OnLanded(PlayerData player)
        {
            Debug.Log($"[VaultTile] {player.PlayerName} landed on Vault");

            // Check if player has Immunity (Raffle Ticket effect)
            if (player.HasEffect("Immunity"))
            {
                Debug.Log($"[VaultTile] {player.PlayerName} has Immunity - Vault effect ignored!");
                return;
            }

            // Determine steal amount
            int stealAmount = Core.GameConstants.VAULT_STEAL_AMOUNT;

            // Grifter ability: Steal 8 instead of 5
            if (player.Character == CharacterType.TheGrifter && player.CanUseGrifterAbility())
            {
                stealAmount = Core.GameConstants.VAULT_STEAL_AMOUNT_GRIFTER;
                player.ResetGrifterCounter();
                Debug.Log($"[VaultTile] Grifter ability activated! Stealing {stealAmount} instead of {Core.GameConstants.VAULT_STEAL_AMOUNT}");
            }

            Debug.Log($"[VaultTile] {player.PlayerName} will steal {stealAmount} bucks from target player");
            
            // TODO: Implement player selection UI (Phase 6)
            // TODO: Transfer money from target to player (Phase 5)
            // TODO: Check for RobTheRobber Raffle Ticket on target (Phase 8)
            
            Debug.Log("[VaultTile] Vault steal logic will be fully implemented in Phase 5-8");

            // Increment Grifter turns
            if (player.Character == CharacterType.TheGrifter)
            {
                player.IncrementGrifterTurns();
            }
        }

        internal override void OnPassed(PlayerData player)
        {
            // No effect when passing
        }
    }
}
