using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Cards
{
    /// <summary>
    /// Casino card used in the Casino mini-game.
    /// Can be Movement card, Dud, or Joker with varying probabilities.
    /// </summary>
    [CreateAssetMenu(fileName = "CasinoCard", menuName = "Last Man's Stash/Cards/Casino Card")]
    public class CasinoCard : CardBase
    {
        [Header("Casino Card Properties")]
        [SerializeField] private CasinoCardType casinoCardType;
        
        [Header("Movement Card (if type is Movement)")]
        [SerializeField] private MovementValue movementValue;

        [Header("Joker Card (if type is Joker)")]
        [SerializeField] private JokerPenalty jokerPenalty;

        // Properties
        public CasinoCardType CasinoCardType => casinoCardType;
        public MovementValue MovementValue => movementValue;
        public JokerPenalty JokerPenalty => jokerPenalty;

        private void OnValidate()
        {
            cardType = CardType.Casino;

            switch (casinoCardType)
            {
                case CasinoCardType.Movement:
                    cardName = $"M{(int)movementValue}";
                    // Description set manually in ScriptableObject
                    break;

                case CasinoCardType.Dud:
                    cardName = "Dud";
                    // Description set manually in ScriptableObject
                    break;

                case CasinoCardType.Joker:
                    cardName = "Joker";
                    // Description set manually in ScriptableObject
                    break;
            }
        }

        public override int GetCardID()
        {
            // Casino cards: 300-399 range
            // Movement: 300-305, Dud: 310, Joker: 320-324
            switch (casinoCardType)
            {
                case CasinoCardType.Movement:
                    return 300 + (int)movementValue;
                case CasinoCardType.Dud:
                    return 310;
                case CasinoCardType.Joker:
                    return 320 + (int)jokerPenalty;
                default:
                    return 300;
            }
        }

        public override void Play(Player.PlayerData player)
        {
            switch (casinoCardType)
            {
                case CasinoCardType.Movement:
                    // Add card to temp pile
                    Debug.Log($"Added M{(int)movementValue} to temp pile");
                    break;

                case CasinoCardType.Dud:
                    Debug.Log("Dud - no effect");
                    break;

                case CasinoCardType.Joker:
                    ApplyJokerPenalty(player);
                    break;
            }
        }

        public override bool CanPlay(Player.PlayerData player)
        {
            // Casino cards are played automatically during Casino mini-game
            return true;
        }

        /// <summary>
        /// Apply the Joker penalty to the player
        /// </summary>
        private void ApplyJokerPenalty(Player.PlayerData player)
        {
            switch (jokerPenalty)
            {
                case JokerPenalty.LoseOneCard:
                    Debug.Log($"{player.PlayerName} loses 1 Movement Card");
                    // Remove 1 card from player's hand
                    break;

                case JokerPenalty.LoseTwoCards:
                    Debug.Log($"{player.PlayerName} loses 2 Movement Cards");
                    // Remove 2 cards from player's hand
                    break;

                case JokerPenalty.LoseThreeBucks:
                    player.RemoveMoney(3);
                    Debug.Log($"{player.PlayerName} loses 3 bucks");
                    break;

                case JokerPenalty.LoseFiveBucks:
                    player.RemoveMoney(5);
                    Debug.Log($"{player.PlayerName} loses 5 bucks");
                    break;

                case JokerPenalty.LoseSevenBucks:
                    player.RemoveMoney(7);
                    Debug.Log($"{player.PlayerName} loses 7 bucks");
                    break;
            }

            // Discard temp pile
            Debug.Log("Temp pile discarded - Casino mini-game ends");
        }

        /// <summary>
        /// Check if this is a Joker card
        /// </summary>
        public bool IsJoker()
        {
            return casinoCardType == CasinoCardType.Joker;
        }

        /// <summary>
        /// Check if this is a Dud
        /// </summary>
        public bool IsDud()
        {
            return casinoCardType == CasinoCardType.Dud;
        }

        /// <summary>
        /// Get the movement value (0 if not a movement card)
        /// </summary>
        public int GetMovementValue()
        {
            return casinoCardType == CasinoCardType.Movement ? (int)movementValue : 0;
        }
    }
}
