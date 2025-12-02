namespace LastMansStash.Core
{
    /// <summary>
    /// Game constants - all magic numbers and configuration values.
    /// Centralized for easy balancing and adjustments.
    /// </summary>
    public static class GameConstants
    {
        // ===== PLAYER SETTINGS =====
        
        /// <summary>
        /// Minimum number of players required
        /// </summary>
        public const int MIN_PLAYERS = 4;

        /// <summary>
        /// Maximum number of players allowed
        /// </summary>
        public const int MAX_PLAYERS = 5;

        /// <summary>
        /// Starting money for each player
        /// </summary>
        public const int STARTING_MONEY = 10;

        /// <summary>
        /// Number of Movement Cards each player starts with
        /// </summary>
        public const int STARTING_CARDS = 5;

        /// <summary>
        /// Maximum Movement Cards a player can hold
        /// </summary>
        public const int MAX_HAND_SIZE = 8;

        /// <summary>
        /// Number of times a player can become a Zombie (max = 1)
        /// </summary>
        public const int MAX_ZOMBIE_REBIRTHS = 1;

        // ===== LAST RESORT SHOP =====

        /// <summary>
        /// Last Resort cost progression (per player)
        /// Cost escalates: 3, 5, 7, 10, 12, 15, 18, 21...
        /// </summary>
        public static readonly int[] LAST_RESORT_COSTS = { 3, 5, 7, 10, 12, 15, 18, 21, 24, 27, 30 };

        /// <summary>
        /// Number of Last Resort purchases where Hacker gets discount
        /// </summary>
        public const int HACKER_DISCOUNT_COUNT = 3;

        /// <summary>
        /// Amount of discount Hacker gets
        /// </summary>
        public const int HACKER_DISCOUNT_AMOUNT = 2;

        // ===== TILE EFFECTS =====

        /// <summary>
        /// Money gained when landing on Start tile
        /// </summary>
        public const int START_TILE_LAND_MONEY = 5;

        /// <summary>
        /// Money gained when passing Start tile
        /// </summary>
        public const int START_TILE_PASS_MONEY = 3;

        /// <summary>
        /// Money stolen from Vault tile (normal)
        /// </summary>
        public const int VAULT_STEAL_AMOUNT = 5;

        /// <summary>
        /// Money stolen from Vault tile (Grifter ability)
        /// </summary>
        public const int VAULT_STEAL_AMOUNT_GRIFTER = 8;

        // ===== BOARD SETTINGS =====

        /// <summary>
        /// Total number of tiles on the board
        /// </summary>
        public const int TOTAL_BOARD_TILES = 32;

        /// <summary>
        /// Tile distribution on board
        /// </summary>
        public const int TILE_COUNT_START = 1;
        public const int TILE_COUNT_CASINO = 4;
        public const int TILE_COUNT_SAFEHOUSE = 4;
        public const int TILE_COUNT_VAULT = 4;
        public const int TILE_COUNT_PAWN_SHOP = 4;
        public const int TILE_COUNT_PAYPHONE = 2;
        public const int TILE_COUNT_BLANK = 13;

        // ===== DRAFT SYSTEM =====

        /// <summary>
        /// Number of characters to show each player during draft
        /// </summary>
        public const int DRAFT_CHARACTERS_SHOWN = 3;

        /// <summary>
        /// Time limit for each draft pick (seconds)
        /// </summary>
        public const float DRAFT_TIMER_SECONDS = 15f;

        // ===== CASINO MINI-GAME =====

        /// <summary>
        /// Number of cards in Casino queue
        /// </summary>
        public const int CASINO_QUEUE_SIZE = 4;

        /// <summary>
        /// Base Joker probability (Card #1)
        /// </summary>
        public const float JOKER_BASE_PROBABILITY = 0.05f; // 5%

        /// <summary>
        /// Joker probability increment per card depth
        /// </summary>
        public const float JOKER_PROBABILITY_INCREMENT = 0.05f; // +5% per card

        /// <summary>
        /// Maximum Joker probability
        /// </summary>
        public const float JOKER_MAX_PROBABILITY = 0.25f; // 25%

        // ===== CARD PROBABILITIES =====

        // Safehouse Deck (Movement Cards)
        public const float SAFEHOUSE_M1_PROBABILITY = 0.30f; // 30%
        public const float SAFEHOUSE_M2_PROBABILITY = 0.25f; // 25%
        public const float SAFEHOUSE_M3_PROBABILITY = 0.20f; // 20%
        public const float SAFEHOUSE_M4_PROBABILITY = 0.15f; // 15%
        public const float SAFEHOUSE_M5_PROBABILITY = 0.10f; // 10%

        // Casino Deck
        public const float CASINO_M0_PROBABILITY = 0.05f;      // 5%
        public const float CASINO_M1_M3_PROBABILITY = 0.40f;   // 40%
        public const float CASINO_M4_M5_PROBABILITY = 0.10f;   // 10%
        public const float CASINO_DUD_PROBABILITY = 0.20f;     // 20%
        // Joker probability varies (JOKER_BASE_PROBABILITY + depth)

        // ===== CHAOS CARD EFFECTS =====

        /// <summary>
        /// Duration of Market Crash effect (rounds)
        /// </summary>
        public const int MARKET_CRASH_DURATION = 3;

        /// <summary>
        /// Last Resort cost multiplier during Market Crash
        /// </summary>
        public const int MARKET_CRASH_MULTIPLIER = 2;

        /// <summary>
        /// Duration of Sting Operation effect (rounds)
        /// </summary>
        public const int STING_OPERATION_DURATION = 3;

        /// <summary>
        /// Duration of Distracted effect (rounds)
        /// </summary>
        public const int DISTRACTED_DURATION = 2;

        /// <summary>
        /// Money required for Police Raid payment
        /// </summary>
        public const int POLICE_RAID_COST = 10;

        // ===== CHARACTER ABILITIES =====

        /// <summary>
        /// How often Grifter can use enhanced Vault steal (every N turns)
        /// </summary>
        public const int GRIFTER_ABILITY_COOLDOWN = 2;

        /// <summary>
        /// How often Runner draws extra card (every N rounds)
        /// </summary>
        public const int RUNNER_ABILITY_FREQUENCY = 4;

        /// <summary>
        /// Number of times Insider can peek at Casino cards
        /// </summary>
        public const int INSIDER_PEEK_COUNT = 2;

        // ===== DAGGER CARD EFFECTS =====

        /// <summary>
        /// Duration of Immunity effect (turns)
        /// </summary>
        public const int IMMUNITY_DURATION = 3;

        /// <summary>
        /// Number of Casino cards Hired Help can view
        /// </summary>
        public const int HIRED_HELP_CARDS_TO_VIEW = 3;

        // ===== WIN CONDITIONS =====

        /// <summary>
        /// Stash percentage for winning Human (when Zombies exist)
        /// </summary>
        public const float HUMAN_WIN_PERCENTAGE = 0.60f; // 60%

        /// <summary>
        /// Stash percentage for Zombies (shared, when Human wins)
        /// </summary>
        public const float ZOMBIE_WIN_PERCENTAGE = 0.40f; // 40%

        /// <summary>
        /// Number of Humans remaining to trigger The Confession
        /// </summary>
        public const int CONFESSION_HUMAN_COUNT = 2;

        /// <summary>
        /// Minimum number of Zombies to trigger The Confession
        /// </summary>
        public const int CONFESSION_ZOMBIE_COUNT = 3;

        // ===== UI SETTINGS =====

        /// <summary>
        /// Minimum load time for Bootstrap scene (seconds)
        /// </summary>
        public const float BOOTSTRAP_MIN_LOAD_TIME = 1.5f;

        /// <summary>
        /// Turn timer duration (optional, seconds)
        /// </summary>
        public const float TURN_TIMER_SECONDS = 60f;

        // ===== NETWORKING =====

        /// <summary>
        /// Photon game version
        /// </summary>
        public const string GAME_VERSION = "1.0";

        /// <summary>
        /// Room code length
        /// </summary>
        public const int ROOM_CODE_LENGTH = 6;
    }
}
