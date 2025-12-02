namespace LastMansStash.Core
{
    /// <summary>
    /// All game enumerations in one place for easy reference.
    /// </summary>
    public static class GameEnums
    {
        /// <summary>
        /// Types of tiles on the board
        /// </summary>
        public enum TileType
        {
            Start,      // Starting tile - gain 5 bucks when landed, 3 when passed
            Blank,      // No effect
            Casino,     // Trigger Casino mini-game
            Safehouse,  // Draw 1 Movement Card (can become Hazard)
            Vault,      // Steal 5 bucks (8 for Grifter)
            PawnShop,   // Draw 1 Dagger Card
            Payphone    // Draw and resolve 1 Chaos Card
        }

        /// <summary>
        /// Overall game state
        /// </summary>
        public enum GameState
        {
            WaitingForPlayers,  // In lobby
            CharacterDraft,     // Drafting characters
            Playing,            // Active game
            GameOver            // Game ended
        }

        /// <summary>
        /// Player status (Human, Zombie, or Spectre)
        /// </summary>
        public enum PlayerStatus
        {
            Human,      // Normal player
            Zombie,     // Eliminated but revived (can only happen once)
            Spectre     // Permanently eliminated (can view cards/bucks but not interfere)
        }

        /// <summary>
        /// Types of cards
        /// </summary>
        public enum CardType
        {
            Movement,   // M0-M5 movement cards
            Dagger,     // Two-faced cards (Bluff + Raffle Ticket)
            Casino,     // Casino deck cards (M0-M5, Duds, Jokers)
            Chaos       // Chaos event cards
        }

        /// <summary>
        /// Movement card values (M0-M5)
        /// </summary>
        public enum MovementValue
        {
            M0 = 0,
            M1 = 1,
            M2 = 2,
            M3 = 3,
            M4 = 4,
            M5 = 5
        }

        /// <summary>
        /// Chaos event types
        /// </summary>
        public enum ChaosEventType
        {
            MarketCrash,        // Last Resort costs double for 3 rounds
            StingOperation,     // Safehouses become Hazards for 3 rounds
            Distracted,         // M4/M5 count as M1 for 2 rounds
            StockExchange,      // All players pass Movement Card hands left
            PoliceRaid,         // All players pay 10 bucks or discard 1 card
            ZombieApocalypse,   // Enables rebirth as Zombies
            TheConfession       // Special automatic trigger (2 Humans + 3+ Zombies)
        }

        /// <summary>
        /// Dagger card - Bluff face types (for Casino mini-game)
        /// </summary>
        public enum DaggerBluffType
        {
            Joker,      // Steal one card from Temp pile when triggered
            Inverted,   // Invert card effect (Joker ↔ Reward)
            Double,     // Double effect
            Scam,       // Discard attached card immediately
            Call        // Cancel another Bluff on the same card
        }

        /// <summary>
        /// Dagger card - Raffle Ticket face types (for normal turn)
        /// </summary>
        public enum DaggerRaffleType
        {
            RobTheRobber,   // If targeted by Vault, steal 5 from attacker
            HiredHelp,      // Look at top 3 Casino cards and rearrange
            Immunity,       // Ignore next Hazard or Vault effect (3 turns)
            Hacker,         // Next Last Resort purchase uses lowest player cost
            Professional    // Steal one random Movement Card from target player
        }

        /// <summary>
        /// Character types (The Crew)
        /// </summary>
        public enum CharacterType
        {
            TheHacker,      // Pay 2 bucks less for any 3 Last Resort purchases
            TheGrifter,     // Steal 8 instead of 5 from Vault (every other turn)
            TheRunner,      // Draw 2 cards instead of 1 at Safehouses (once per 3 rounds)
            TheInsider,     // Peek at next Casino card twice per game
            TheThug,        // Start with Professional Dagger Card
            TheSmuggler,    // Draw 2 Daggers at Pawn Shop, choose 1 (once per 3 rounds)
            TheMastermind   // Swap positions with another player (twice per game)
        }

        /// <summary>
        /// Turn phases
        /// </summary>
        public enum TurnPhase
        {
            Start,          // Start of turn
            PlayCards,      // Play movement and raffle ticket cards
            Movement,       // Move on board
            TileEffect,     // Resolve tile effect
            End             // End of turn
        }

        /// <summary>
        /// Casino card types (for Casino deck)
        /// </summary>
        public enum CasinoCardType
        {
            Movement,   // M0-M5
            Dud,        // No effect
            Joker       // Penalty (lose cards/money)
        }

        /// <summary>
        /// Joker penalty types
        /// </summary>
        public enum JokerPenalty
        {
            LoseOneCard,    // Lose 1 Movement Card
            LoseTwoCards,   // Lose 2 Movement Cards
            LoseThreeBucks, // Lose 3 bucks
            LoseFiveBucks,  // Lose 5 bucks
            LoseSevenBucks  // Lose 7 bucks
        }
    }
}
