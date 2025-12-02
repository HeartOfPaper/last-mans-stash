using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Cards
{
    /// <summary>
    /// Two-faced Dagger card with Bluff side (Casino) and Raffle Ticket side (Normal).
    /// Can be used in two different ways depending on context.
    /// </summary>
    [CreateAssetMenu(fileName = "DaggerCard", menuName = "Last Man's Stash/Cards/Dagger Card")]
    public class DaggerCard : CardBase
    {
        [Header("Dagger Card - Bluff Face")]
        [SerializeField] private DaggerBluffType bluffType;
        [SerializeField] private string bluffName;
        [TextArea(2, 4)]
        [SerializeField] private string bluffDescription;
        [SerializeField] private Sprite bluffArt;

        [Header("Dagger Card - Raffle Ticket Face")]
        [SerializeField] private DaggerRaffleType raffleType;
        [SerializeField] private string raffleName;
        [TextArea(2, 4)]
        [SerializeField] private string raffleDescription;
        [SerializeField] private Sprite raffleArt;

        [Header("Usage Tracking")]
        [SerializeField] private bool isBluffSideUp = false; // Which face is currently active

        // Bluff Face Properties
        public DaggerBluffType BluffType => bluffType;
        public string BluffName => bluffName;
        public string BluffDescription => bluffDescription;
        public Sprite BluffArt => bluffArt;

        // Raffle Ticket Face Properties
        public DaggerRaffleType RaffleType => raffleType;
        public string RaffleName => raffleName;
        public string RaffleDescription => raffleDescription;
        public Sprite RaffleArt => raffleArt;

        // Current Face
        public bool IsBluffSideUp => isBluffSideUp;

        private void OnValidate()
        {
            cardType = CardType.Dagger;
            cardName = isBluffSideUp ? bluffName : raffleName;
            description = isBluffSideUp ? bluffDescription : raffleDescription;
            cardArt = isBluffSideUp ? bluffArt : raffleArt;
        }

        public override int GetCardID()
        {
            // Dagger cards: 200-299 range
            // ID = 200 + (bluffType * 10) + raffleType
            return 200 + ((int)bluffType * 10) + (int)raffleType;
        }

        public override void Play(Player.PlayerData player)
        {
            if (isBluffSideUp)
            {
                Debug.Log($"{player.PlayerName} played Bluff: {bluffName}");
                // Bluff logic handled by Casino mini-game
            }
            else
            {
                Debug.Log($"{player.PlayerName} played Raffle Ticket: {raffleName}");
                // Raffle ticket effect logic
                PlayRaffleTicket(player);
            }
        }

        public override bool CanPlay(Player.PlayerData player)
        {
            if (isBluffSideUp)
            {
                // Bluffs can only be played during Casino mini-game
                return false; // Will be overridden by Casino UI
            }
            else
            {
                // Raffle tickets can be played during normal turn
                return true;
            }
        }

        /// <summary>
        /// Flip the card to the other face
        /// Internal to prevent players from flipping cards inappropriately
        /// </summary>
        internal void Flip()
        {
            isBluffSideUp = !isBluffSideUp;
            OnValidate();
        }

        /// <summary>
        /// Set which face is showing
        /// Internal to prevent players from manipulating card state
        /// </summary>
        internal void SetFace(bool bluffSide)
        {
            isBluffSideUp = bluffSide;
            OnValidate();
        }

        /// <summary>
        /// Get the current face name
        /// </summary>
        public string GetCurrentFaceName()
        {
            return isBluffSideUp ? bluffName : raffleName;
        }

        /// <summary>
        /// Play the Raffle Ticket effect (to be implemented by specific cards)
        /// </summary>
        protected virtual void PlayRaffleTicket(Player.PlayerData player)
        {
            // Will be overridden by specific raffle ticket implementations
            Debug.Log($"Playing {raffleName} effect");
        }

        /// <summary>
        /// Apply the Bluff effect during Casino mini-game (to be implemented)
        /// </summary>
        public virtual void ApplyBluffEffect()
        {
            // Will be implemented in Casino system
            Debug.Log($"Applying {bluffName} effect");
        }

        public override string ToString()
        {
            return $"Dagger Card - Bluff: {bluffName} / Raffle: {raffleName}";
        }
    }
}
