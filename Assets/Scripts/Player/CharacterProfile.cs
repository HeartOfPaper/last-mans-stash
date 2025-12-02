using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Player
{
    /// <summary>
    /// ScriptableObject defining a playable character (The Crew).
    /// Contains character info, abilities, and visual assets.
    /// </summary>
    [CreateAssetMenu(fileName = "Character", menuName = "Last Man's Stash/Character Profile")]
    public class CharacterProfile : ScriptableObject
    {
        [Header("Character Identity")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private string characterName;
        [TextArea(2, 4)]
        [SerializeField] private string characterDescription;

        [Header("Ability")]
        [SerializeField] private string abilityName;
        [TextArea(3, 6)]
        [SerializeField] private string abilityDescription;

        [Header("Visual Assets")]
        [SerializeField] private Sprite characterPortrait;
        [SerializeField] private GameObject characterModel; // 3D token model
        [SerializeField] private Color characterColor = Color.white;

        [Header("Starting Bonuses (Optional)")]
        [SerializeField] private int startingMoneyBonus = 0;
        [SerializeField] private int startingCardsBonus = 0;

        // Properties
        public CharacterType CharacterType => characterType;
        public string CharacterName => characterName;
        public string CharacterDescription => characterDescription;
        public string AbilityName => abilityName;
        public string AbilityDescription => abilityDescription;
        public Sprite CharacterPortrait => characterPortrait;
        public GameObject CharacterModel => characterModel;
        public Color CharacterColor => characterColor;
        public int StartingMoneyBonus => startingMoneyBonus;
        public int StartingCardsBonus => startingCardsBonus;

        private void OnValidate()
        {
            // Auto-set character name based on type
            if (string.IsNullOrEmpty(characterName))
            {
                characterName = characterType.ToString().Replace("The", "The ");
            }
        }

        public override string ToString()
        {
            return $"{characterName}: {abilityName}";
        }
    }
}
