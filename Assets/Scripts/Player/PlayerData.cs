using System.Collections.Generic;
using UnityEngine;
using static LastMansStash.Core.GameEnums;

namespace LastMansStash.Player
{
    /// <summary>
    /// Data class representing a player's complete state.
    /// Holds all player information that needs to be synced across network.
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        [Header("Player Identity")]
        [SerializeField] private string playerName;
        [SerializeField] private int playerID; // Photon ID
        [SerializeField] private CharacterType character;

        [Header("Player State")]
        [SerializeField] private PlayerStatus status = PlayerStatus.Human;
        [SerializeField] private int currentTileIndex = 0;
        [SerializeField] private bool hasZombified = false; // Can only zombify once

        [Header("Resources")]
        [SerializeField] private int money;
        [SerializeField] private List<int> movementCardValues = new List<int>(); // Stores M0-M5 values
        [SerializeField] private List<int> daggerCardIDs = new List<int>(); // Stores Dagger card IDs

        [Header("Last Resort Tracking")]
        [SerializeField] private int lastResortPurchaseCount = 0;

        [Header("Active Effects")]
        [SerializeField] private List<string> activeEffects = new List<string>(); // Active status effects

        [Header("Character Ability Tracking")]
        [SerializeField] private int hackerDiscountUsed = 0; // Count of discounted purchases
        [SerializeField] private int grifterTurnsSinceLastVault = 0; // Turns since last Vault use
        [SerializeField] private int runnerRoundsSinceLastUse = 0; // Rounds since Runner used ability
        [SerializeField] private int insiderPeeksRemaining = 2; // Peeks remaining
        [SerializeField] private bool thugStartingCardGiven = false; // Starting Dagger given
        [SerializeField] private int smugglerRoundsSinceLastUse = 0; // Rounds since Smuggler used ability
        [SerializeField] private int mastermindSwapsRemaining = 2; // Position swaps remaining

        // === PROPERTIES ===

        public string PlayerName => playerName;
        public int PlayerID => playerID;
        public CharacterType Character => character;
        public PlayerStatus Status => status;
        public int CurrentTileIndex => currentTileIndex;
        public bool HasZombified => hasZombified;
        public int Money => money;
        public List<int> MovementCardValues => movementCardValues;
        public List<int> DaggerCardIDs => daggerCardIDs;
        public int LastResortPurchaseCount => lastResortPurchaseCount;
        public List<string> ActiveEffects => activeEffects;

        // Character ability properties
        public int HackerDiscountUsed => hackerDiscountUsed;
        public int GrifterTurnsSinceLastVault => grifterTurnsSinceLastVault;
        public int RunnerRoundsSinceLastUse => runnerRoundsSinceLastUse;
        public int InsiderPeeksRemaining => insiderPeeksRemaining;
        public bool ThugStartingCardGiven => thugStartingCardGiven;
        public int SmugglerRoundsSinceLastUse => smugglerRoundsSinceLastUse;
        public int MastermindSwapsRemaining => mastermindSwapsRemaining;

        // === CONSTRUCTOR ===

        public PlayerData(string name, int id, CharacterType characterType)
        {
            playerName = name;
            playerID = id;
            character = characterType;
            status = PlayerStatus.Human;
            money = Core.GameConstants.STARTING_MONEY;
            currentTileIndex = 0;
            hasZombified = false;
            lastResortPurchaseCount = 0;
        }

        // === MONEY MANAGEMENT ===

        internal void AddMoney(int amount)
        {
            money += amount;
            if (money < 0) money = 0;
        }

        internal void RemoveMoney(int amount)
        {
            money -= amount;
            if (money < 0) money = 0;
        }

        public bool HasEnoughMoney(int amount)
        {
            return money >= amount;
        }

        // === CARD MANAGEMENT ===

        internal void AddMovementCard(int cardValue)
        {
            if (movementCardValues.Count < Core.GameConstants.MAX_HAND_SIZE)
            {
                movementCardValues.Add(cardValue);
            }
        }

        internal void RemoveMovementCard(int cardValue)
        {
            movementCardValues.Remove(cardValue);
        }

        internal void AddDaggerCard(int cardID)
        {
            daggerCardIDs.Add(cardID);
        }

        internal void RemoveDaggerCard(int cardID)
        {
            daggerCardIDs.Remove(cardID);
        }

        public int GetMovementCardCount()
        {
            return movementCardValues.Count;
        }

        public int GetDaggerCardCount()
        {
            return daggerCardIDs.Count;
        }

        // === POSITION ===

        internal void SetTileIndex(int index)
        {
            currentTileIndex = index;
        }

        internal void MoveTiles(int spaces)
        {
            currentTileIndex += spaces;
            // Board wrapping handled by BoardManager
        }

        // === STATUS MANAGEMENT ===

        internal void SetStatus(PlayerStatus newStatus)
        {
            status = newStatus;
        }

        internal void Zombify()
        {
            if (!hasZombified)
            {
                status = PlayerStatus.Zombie;
                hasZombified = true;

                // Reset for zombie state
                money = 0;
                movementCardValues.Clear();
                daggerCardIDs.Clear();
                currentTileIndex = 0; // Return to Start
                lastResortPurchaseCount = 0;
                activeEffects.Clear();
            }
        }

        internal void BecomeSpectre()
        {
            status = PlayerStatus.Spectre;
        }

        // === LAST RESORT ===

        public int GetLastResortCost()
        {
            if (lastResortPurchaseCount >= Core.GameConstants.LAST_RESORT_COSTS.Length)
            {
                // Cost continues to escalate beyond array
                int baseIndex = Core.GameConstants.LAST_RESORT_COSTS.Length - 1;
                int overage = lastResortPurchaseCount - baseIndex;
                return Core.GameConstants.LAST_RESORT_COSTS[baseIndex] + (overage * 3);
            }
            return Core.GameConstants.LAST_RESORT_COSTS[lastResortPurchaseCount];
        }

        internal void IncrementLastResortPurchaseCount()
        {
            lastResortPurchaseCount++;
        }

        // === EFFECTS ===

        internal void AddEffect(string effectName)
        {
            if (!activeEffects.Contains(effectName))
            {
                activeEffects.Add(effectName);
            }
        }

        internal void RemoveEffect(string effectName)
        {
            activeEffects.Remove(effectName);
        }

        public bool HasEffect(string effectName)
        {
            return activeEffects.Contains(effectName);
        }

        // === CHARACTER ABILITIES ===

        internal void UseHackerDiscount()
        {
            hackerDiscountUsed++;
        }

        public bool CanUseHackerDiscount()
        {
            return character == CharacterType.TheHacker && 
                   hackerDiscountUsed < Core.GameConstants.HACKER_DISCOUNT_COUNT;
        }

        internal void IncrementGrifterTurns()
        {
            grifterTurnsSinceLastVault++;
        }

        internal void ResetGrifterCounter()
        {
            grifterTurnsSinceLastVault = 0;
        }

        public bool CanUseGrifterAbility()
        {
            return character == CharacterType.TheGrifter && 
                   grifterTurnsSinceLastVault >= Core.GameConstants.GRIFTER_ABILITY_COOLDOWN;
        }

        internal void IncrementRunnerRounds()
        {
            runnerRoundsSinceLastUse++;
        }

        internal void ResetRunnerCounter()
        {
            runnerRoundsSinceLastUse = 0;
        }

        public bool CanUseRunnerAbility()
        {
            return character == CharacterType.TheRunner && 
                   runnerRoundsSinceLastUse >= 3; // Once every 3 rounds
        }

        internal void UseInsiderPeek()
        {
            if (insiderPeeksRemaining > 0)
            {
                insiderPeeksRemaining--;
            }
        }

        public bool CanUseInsiderPeek()
        {
            return character == CharacterType.TheInsider && insiderPeeksRemaining > 0;
        }

        internal void GiveThugStartingCard()
        {
            thugStartingCardGiven = true;
        }

        internal void IncrementSmugglerRounds()
        {
            smugglerRoundsSinceLastUse++;
        }

        internal void ResetSmugglerCounter()
        {
            smugglerRoundsSinceLastUse = 0;
        }

        public bool CanUseSmugglerAbility()
        {
            return character == CharacterType.TheSmuggler && 
                   smugglerRoundsSinceLastUse >= 3;
        }

        internal void UseMastermindSwap()
        {
            if (mastermindSwapsRemaining > 0)
            {
                mastermindSwapsRemaining--;
            }
        }

        public bool CanUseMastermindAbility()
        {
            return character == CharacterType.TheMastermind && mastermindSwapsRemaining > 0;
        }

        // === UTILITY ===

        public override string ToString()
        {
            return $"{playerName} ({character}) - {status} - ${money} - {movementCardValues.Count} cards";
        }
    }
}
