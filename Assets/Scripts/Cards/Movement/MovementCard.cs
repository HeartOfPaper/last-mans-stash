using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Cards
{
    /// <summary>
    /// Movement card (M0-M5) used for moving around the board.
    /// </summary>
    [CreateAssetMenu(fileName = "MovementCard", menuName = "Last Man's Stash/Cards/Movement Card")]
    public class MovementCard : CardBase
    {
        [Header("Movement Card Properties")]
        [SerializeField] private MovementValue movementValue;

        public MovementValue MovementValue => movementValue;
        public int Value => (int)movementValue;

        private void OnValidate()
        {
            cardType = CardType.Movement;
            cardName = $"M{(int)movementValue}";
        }

        public override int GetCardID()
        {
            // Simple ID: card type (100-199) + value
            return 100 + (int)movementValue;
        }

        public override void Play(Player.PlayerData player)
        {
            // Movement logic will be handled by game manager
            // This just validates the card can be played
            Debug.Log($"{player.PlayerName} played {cardName}");
        }

        public override bool CanPlay(Player.PlayerData player)
        {
            // Can always play a movement card if it's your turn
            return true;
        }
    }
}
