using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Cards
{
    /// <summary>
    /// Abstract base class for all card types in the game.
    /// Provides common functionality for cards.
    /// </summary>
    public abstract class CardBase : ScriptableObject
    {
        [Header("CardBase Properties")]
        [SerializeField] protected string cardName;
        [SerializeField] protected CardType cardType;
        [TextArea(3, 5)]
        [SerializeField] protected string description;
        [SerializeField] protected Sprite cardArt;

        // Properties
        public string CardName => cardName;
        public CardType CardType => cardType;
        public string Description => description;
        public Sprite CardArt => cardArt;

        /// <summary>
        /// Unique ID for this card (for networking)
        /// </summary>
        public abstract int GetCardID();

        /// <summary>
        /// Play the card - implement specific behavior in derived classes
        /// </summary>
        public abstract void Play(Player.PlayerData player);

        /// <summary>
        /// Can this card be played right now?
        /// </summary>
        public abstract bool CanPlay(Player.PlayerData player);

        public override string ToString()
        {
            return $"{cardName} ({cardType})";
        }
    }
}
