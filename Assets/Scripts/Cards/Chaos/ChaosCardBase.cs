using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Cards
{
    /// <summary>
    /// Base class for all Chaos event cards.
    /// Chaos cards trigger game-changing events.
    /// </summary>
    [CreateAssetMenu(fileName = "ChaosCard", menuName = "Last Man's Stash/Cards/Chaos Card")]
    public class ChaosCardBase : CardBase
    {
        [Header("Chaos Card Properties")]
        [SerializeField] private ChaosEventType chaosEventType;
        
        [Header("Effect Duration")]
        [SerializeField] private int durationRounds = 0; // 0 = instant effect
        [SerializeField] private bool isOneTimeEffect = true;

        [Header("Effect Flags")]
        [SerializeField] private bool affectsAllPlayers = true;
        [SerializeField] private bool canBeNegated = false;

        // Properties
        public ChaosEventType ChaosEventType => chaosEventType;
        public int DurationRounds => durationRounds;
        public bool IsOneTimeEffect => isOneTimeEffect;
        public bool AffectsAllPlayers => affectsAllPlayers;
        public bool CanBeNegated => canBeNegated;

        private void OnValidate()
        {
            cardType = CardType.Chaos;
            cardName = chaosEventType.ToString();
            
            // Description is set manually in ScriptableObject (designer-friendly)
            // Duration is set automatically based on constants
            SetDurationFromType();
        }

        public override int GetCardID()
        {
            // Chaos cards: 400-499 range
            return 400 + (int)chaosEventType;
        }

        public override void Play(Player.PlayerData player)
        {
            Debug.Log($"Chaos Event Triggered: {cardName}");
            ApplyChaosEffect();
        }

        public override bool CanPlay(Player.PlayerData player)
        {
            // Chaos cards are drawn and resolved automatically
            return true;
        }

        /// <summary>
        /// Apply the chaos effect - override in specific chaos card implementations
        /// </summary>
        protected virtual void ApplyChaosEffect()
        {
            switch (chaosEventType)
            {
                case ChaosEventType.MarketCrash:
                    Debug.Log("Market Crash! Last Resort costs double for 3 rounds");
                    break;

                case ChaosEventType.StingOperation:
                    Debug.Log("Sting Operation! Safehouses become Hazards for 3 rounds");
                    break;

                case ChaosEventType.Distracted:
                    Debug.Log("Distracted! M4/M5 count as M1 for 2 rounds");
                    break;

                case ChaosEventType.StockExchange:
                    Debug.Log("Stock Exchange! All players pass their Movement Card hands left");
                    break;

                case ChaosEventType.PoliceRaid:
                    Debug.Log("Police Raid! All players pay 10 bucks or discard 1 card");
                    break;

                case ChaosEventType.ZombieApocalypse:
                    Debug.Log("Zombie Apocalypse! Players can now return as Zombies");
                    break;

                case ChaosEventType.TheConfession:
                    Debug.Log("The Confession! Final showdown triggered");
                    break;
            }
        }

        /// <summary>
        /// Set the duration based on chaos type (uses constants)
        /// </summary>
        private void SetDurationFromType()
        {
            switch (chaosEventType)
            {
                case ChaosEventType.MarketCrash:
                    durationRounds = Core.GameConstants.MARKET_CRASH_DURATION;
                    break;

                case ChaosEventType.StingOperation:
                    durationRounds = Core.GameConstants.STING_OPERATION_DURATION;
                    break;

                case ChaosEventType.Distracted:
                    durationRounds = Core.GameConstants.DISTRACTED_DURATION;
                    break;

                case ChaosEventType.StockExchange:
                case ChaosEventType.PoliceRaid:
                case ChaosEventType.ZombieApocalypse:
                case ChaosEventType.TheConfession:
                    durationRounds = 0;
                    break;
            }
        }

        /// <summary>
        /// Check if this is The Confession
        /// </summary>
        public bool IsTheConfession()
        {
            return chaosEventType == ChaosEventType.TheConfession;
        }

        /// <summary>
        /// Check if this is Zombie Apocalypse
        /// </summary>
        public bool IsZombieApocalypse()
        {
            return chaosEventType == ChaosEventType.ZombieApocalypse;
        }
    }
}
