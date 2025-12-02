using UnityEngine;
using LastMansStash.Player;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Board.TileEffects
{
    /// <summary>
    /// Payphone Tile - Draw and resolve 1 Chaos Card immediately.
    /// Chaos cards trigger game-changing events.
    /// </summary>
    public class PayphoneTile : TileBase
    {
        private void Awake()
        {
            tileType = TileType.Payphone;
            tileName = "Payphone";
            description = "Draw and resolve 1 Chaos Card";
        }

        internal override void OnLanded(PlayerData player)
        {
            Debug.Log($"[PayphoneTile] {player.PlayerName} landed on Payphone");
            Debug.Log("[PayphoneTile] Drawing 1 Chaos Card...");

            // TODO: Draw and resolve 1 Chaos Card (Phase 8)
            Debug.Log("[PayphoneTile] Chaos Card system will be implemented in Phase 8");
            
            // Chaos cards can trigger:
            // - Market Crash (Last Resort x2 for 3 rounds)
            // - Sting Operation (Safehouses → Hazards for 3 rounds)
            // - Distracted (M4/M5 → M1 for 2 rounds)
            // - Stock Exchange (pass hands left)
            // - Police Raid (pay 10 or discard 1)
            // - Zombie Apocalypse (enable rebirth)
            // - The Confession (special trigger: 2 Humans + 3+ Zombies)
        }

        internal override void OnPassed(PlayerData player)
        {
            // No effect when passing
        }
    }
}
