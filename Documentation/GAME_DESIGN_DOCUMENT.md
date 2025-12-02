# Last Man's Stash - Complete Game Design Document

**Genre**: Competitive / Press-Your-Luck / Hand Management  
**Player Count**: 4–5  
**Game Time**: 60–90 minutes  
**Theme**: Heist Gone Wrong / Cops & Robbers

---

## Table of Contents
1. [Game Overview](#1-game-overview)
2. [Tiles](#2-tiles)
3. [Cards](#3-cards)
4. [Player Abilities (The Crew)](#4-player-abilities-the-crew)
5. [Game Rules](#5-game-rules)
6. [Scenes](#6-scenes)
7. [Prefabs](#7-prefabs)
8. [3D Models](#8-3d-models-to-create)
9. [Scripts](#9-scripts)
10. [ScriptableObjects](#10-scriptableobjects)

---

## 1. Game Overview

### Premise
A crew of thieves has just pulled off a massive heist. The cops are closing in, but there's a twist: the police believe there was one fewer robber than there actually was (N–1). The first N–1 players to get Busted are caught. The last player standing escapes with the entire stash.

### Win & Loss Conditions
- **Win (Human)**: Be the last Human player not eliminated. Win 100% of the stash.
- **Win (Zombie)**: If a Human player wins, all Zombie players also win. The Human keeps 60% of the stash and Zombies share the remaining 40%.
- **If only Zombies remain**: They share the entire stash equally.
- **Loss**: You are Busted if you cannot play or buy a Movement Card.

### Core Mechanics
- Hand Management
- Press-Your-Luck (Casino mini-game)
- Asymmetrical Powers (The Crew)
- Player Elimination & Rebirth (Zombies)

---

## 2. Tiles

### Tile Distribution (30-40 total)

| Tile Name | Count | Effect | Script |
|-----------|-------|--------|--------|
| **Start** | 1 | Gain 5 bucks when landed on, 3 when passed | `StartTile.cs` |
| **Blank Tile** | 10-15 | No effect | `BlankTile.cs` |
| **Casino** | 4 | Trigger the Casino mini-game | `CasinoTile.cs` |
| **Safehouse** | 4 | Draw 1 Movement Card (M1–M5) | `SafehouseTile.cs` |
| **Vault** | 4 | Choose a player, steal 5 bucks (8 for Grifter) | `VaultTile.cs` |
| **Pawn Shop** | 4 | Draw 1 Dagger Card | `PawnShopTile.cs` |
| **Payphone** | 2 | Draw and resolve 1 Chaos Card | `PayphoneTile.cs` |

### Tile Details

#### Start Tile
- **Effect**: Gain 5 bucks when landed on, 3 when passed
- **Script**: `StartTile.cs`

#### Blank Tile
- **Effect**: None
- **Script**: `BlankTile.cs`

#### Casino Tile
- **Effect**: Triggers the Casino mini-game (see Casino Mini-Game section)
- **Script**: `CasinoTile.cs`
- **Related Scripts**: `CasinoMiniGame.cs`, `CasinoCardGenerator.cs`, `BluffSystem.cs`

#### Safehouse Tile
- **Effect**: Draw 1 Movement Card (M1–M5) from Safehouse Deck
- **Special**: Becomes a Hazard during "Sting Operation" Chaos event (discard 1 Movement Card, move 1 forward)
- **Script**: `SafehouseTile.cs`

#### Vault Tile
- **Effect**: Choose a player, steal 5 bucks (8 for Grifter)
- **Counter**: "Rob the Robber" Dagger Card can reverse this
- **Script**: `VaultTile.cs`

#### Pawn Shop Tile
- **Effect**: Draw 1 Dagger Card
- **Script**: `PawnShopTile.cs`

#### Payphone Tile
- **Effect**: Draw and resolve 1 Chaos Card (cannot draw "The Confession")
- **Script**: `PayphoneTile.cs`

### Base Tile System
- **Base Script**: `TileBase.cs` - Abstract class all tiles inherit from
- **Tile Identifier**: `TileIdentifier.cs` - Component to mark tiles in scene
- **Board Manager**: `BoardManager.cs` - Manages all tiles and board state

---

## 3. Cards

### Card Decks (5 Total)

#### 1. Casino Deck
**Purpose**: Used during Casino mini-game  
**Generation**: Probabilistic (infinite)

| Card Type | Probability | Effect |
|-----------|-------------|--------|
| M0 | 5% | Activate tile again |
| M1–M3 | 40% | Normal Movement Cards |
| M4–M5 | 10% | Stronger Movement Cards |
| Duds | 20% | No effect |
| Jokers | 25% | Various penalties (see Joker Variants) |

**Joker Variants** (Penalty Effects):
- Lose 1 Movement Card
- Lose 2 Movement Cards
- Lose 3 bucks
- Lose 5 bucks
- Lose 7 bucks

**Joker Escalation** (Probability increases with depth):
- Card #1: 5%
- Card #2: 10%
- Card #3: 15%
- Card #4: 20%
- Card #5+: 25%+

**Scripts**:
- `CasinoCardGenerator.cs` - Generates cards probabilistically
- `CasinoCard.cs` - Individual card data
- `JokerCard.cs` - Joker penalty logic

---

#### 2. Safehouse Deck
**Purpose**: Drawn from Safehouse tiles  
**Generation**: Probabilistic (infinite)  
**Contents**: Movement Cards (M1–M5)

| Card Type | Probability |
|-----------|-------------|
| M1 | 30% |
| M2 | 25% |
| M3 | 20% |
| M4 | 15% |
| M5 | 10% |

**Scripts**:
- `SafehouseDeckGenerator.cs`
- `MovementCard.cs`

---

#### 3. Chaos Deck
**Purpose**: Drawn from Payphone tiles or triggered by events  
**Generation**: Probabilistic (infinite)

| Card Name | Effect | Script |
|-----------|--------|--------|
| **Market Crash** | Last Resort costs double for 3 rounds | `MarketCrashCard.cs` |
| **Sting Operation** | Safehouses become Hazards for 3 rounds (discard 1 Movement Card, move 1 forward) | `StingOperationCard.cs` |
| **Distracted** | M4/M5 count as M1 for 2 rounds | `DistractedCard.cs` |
| **Stock Exchange** | All players pass their Movement Card hands left | `StockExchangeCard.cs` |
| **Police Raid** | All players pay 10 bucks or discard 1 Movement Card | `PoliceRaidCard.cs` |
| **Zombie Apocalypse** | Enables rebirth as Zombies | `ZombieApocalypseCard.cs` |
| **The Confession** | Special automatic trigger event (see Rules) | `TheConfessionCard.cs` |

**Scripts**:
- `ChaosDeckGenerator.cs`
- `ChaosCardBase.cs` - Base class for all Chaos cards

---

#### 4. Dagger Deck
**Purpose**: Two-faced cards for Casino Bluffs and Raffle Tickets  
**Generation**: Probabilistic (infinite)

**Two-Faced Cards**: Each Dagger has two sides:
1. **Bluff Face** (Casino use)
2. **Raffle Ticket Face** (normal turn use)

##### Bluff Faces (Casino Mini-Game)
| Bluff Type | Effect | Script |
|------------|--------|--------|
| **Joker** | Steal one card from Temp pile when triggered | `JokerBluff.cs` |
| **Inverted** | Invert card effect (Joker ↔ Reward) | `InvertedBluff.cs` |
| **Double** | Double effect | `DoubleBluff.cs` |
| **Scam** | Discard attached card immediately | `ScamBluff.cs` |
| **Call** | Cancel another Bluff on the same card | `CallBluff.cs` |

##### Raffle Ticket Faces (Normal Turn)
| Raffle Ticket | Effect | Script |
|---------------|--------|--------|
| **Rob the Robber** | If targeted by The Vault, steal 5 from attacker | `RobTheRobberCard.cs` |
| **Hired Help** | Look at top 3 Casino cards and rearrange | `HiredHelpCard.cs` |
| **Immunity** | Ignore next Hazard or Vault effect (3 turns) | `ImmunityCard.cs` |
| **Hacker** | Next Last Resort purchase uses lowest player cost | `HackerCard.cs` |
| **Professional** | Steal one random Movement Card from target player | `ProfessionalCard.cs` |

**Scripts**:
- `DaggerDeckGenerator.cs`
- `DaggerCard.cs` - Two-faced card data
- `BluffFace.cs` - Bluff side logic
- `RaffleTicketFace.cs` - Raffle Ticket side logic

---

#### 5. Movement Deck
**Purpose**: Used only for Last Resort purchases  
**Generation**: Infinite draw with escalating cost

**Cost Scale** (per player): 3, 5, 7, 10, 12, 15, 18, 21...

**Scripts**:
- `MovementDeckGenerator.cs`
- `LastResortShop.cs`
- `PlayerCostTracker.cs`

---

### Card System Scripts
| Script | Purpose |
|--------|---------|
| `CardBase.cs` | Base class for all cards |
| `CardGenerator.cs` | Probabilistic card generation system |
| `DeckManager.cs` | Manages all 5 decks |
| `HandManager.cs` | Manages player's hand (max 8 Movement Cards) |
| `CardPlayHandler.cs` | Validates and executes card plays |

---

## 4. Player Abilities (The Crew)

### Character Profiles

| Character | Ability Name | Effect | Script |
|-----------|--------------|--------|--------|
| **The Hacker** | Cost Reduction | Pay 2 bucks less for any 3 purchases from Last Resort Shop | `HackerAbility.cs` |
| **The Grifter** | Enhanced Theft | When using The Vault, steal 8 bucks instead of 5 (every other turn) | `GrifterAbility.cs` |
| **The Runner** | Card Draw | Every 4 rounds, draw 1 extra Movement Card | `RunnerAbility.cs` |
| **The Insider** | Casino Peek | During Casino mini-game, peek at next card twice per game | `InsiderAbility.cs` |
| **The Thug** | Starting Advantage | Start the game with the "Professional" Dagger Card | `ThugAbility.cs` |

**Base Scripts**:
- `CharacterAbilityBase.cs` - Abstract class for abilities
- `CharacterProfile.cs` - ScriptableObject for character data
- `AbilityManager.cs` - Manages ability triggers

---

## 5. Game Rules

### Setup
1. Each player selects a Character Mat and matching pawn
2. Place all pawns on the Start tile
3. Each player begins with:
   - **10 bucks**
   - **5 random Movement Cards** from the Safehouse Deck
4. Determine play order

### Player Turn (Human or Zombie)
1. **Play one Movement Card** (M0–M5)
   - If no cards, you must buy one from the Last Resort
   - If you cannot afford it, you are **Busted**
2. **Resolve the tile effect**
3. **Optionally play Raffle Ticket (Normal Face) Dagger Cards**
4. **End turn**
   - Maximum hand size: **8 Movement Cards**

### The Last Resort Shop
If you start your turn with zero Movement Cards, you must buy one from the Movement Deck.

**Cost Scale** (per player, escalates individually):
- 1st purchase: 3 bucks
- 2nd purchase: 5 bucks
- 3rd purchase: 7 bucks
- 4th purchase: 10 bucks
- 5th purchase: 12 bucks
- And so on...

**Special**: The Hacker pays 2 bucks less for any 3 purchases.

### Casino Mini-Game

**Trigger**: Player lands on a Casino tile.

**Setup**:
1. Generate 4 cards from the Casino Deck (queue)
2. Generate a new card each time one is flipped, maintaining 4 in queue
3. Joker probability escalates with depth

**Sequence**:
1. **Opponents look at Card #1**. Active player does not.
2. **Each opponent may attach one Bluff** (Dagger Bluff side) to any unseen future card.
3. **Active player chooses to Flip or Pass**:

**If Pass**:
- Mini-game ends
- Collect all Movement Cards from Temp pile into hand
- Discard Duds and Bluffs

**If Flip**:
- Reveal attached Bluff(s), then reveal the card
- Apply Bluff effects, then card effects
- If a Joker is flipped, apply its penalty, discard Temp pile, end mini-game
- If safe, continue with the next card

**Inverted Bluff Interaction**:
- Converts Joker losses into equivalent rewards (lose 5 → gain 5, lose card → draw card)
- Converts normal Movement or buck rewards into penalties
- Duds unaffected

**Scripts**:
- `CasinoMiniGame.cs` - Main mini-game controller
- `CasinoQueue.cs` - Manages 4-card queue
- `BluffSystem.cs` - Handles Bluff attachments and reveals
- `CasinoUI.cs` - UI for mini-game

### Elimination, Zombies, and Spectres

#### Busted
You are eliminated if you cannot play or buy a Movement Card.

#### Zombie Rebirth
If **Zombie Apocalypse** is active, you return next turn as a Zombie:
- Place pawn on Start
- Discard hand and stash
- Draw 3 Movement Cards
- Reset cost tracker
- **You may only zombify once**

If Zombie Apocalypse is not active, you are permanently eliminated.

#### Spectres (Eliminated Players)
- May view other players' **Movement Cards or Bucks**
- Cannot view Dagger Cards
- Cannot interfere with play

**Scripts**:
- `PlayerElimination.cs`
- `ZombieRebirth.cs`
- `SpectreMode.cs`

### The Confession (Chaos Event)

**Type**: Conditional, automatic  
**Trigger**: When exactly 2 Humans remain and 3 or more Zombies exist, and Zombie Apocalypse is active. Cannot be drawn via Payphone.

**Effect**:
If one of the two Humans is Busted, they confess to the police. The game continues instead of ending. The last Human plays alone against all Zombies.

**Endgame Outcomes**:
1. Solo Human survives → Wins 100% of stash
2. Solo Human turns Zombie → All Zombies share 100% equally
3. Solo Human Busted while Human → All Zombies share 100% equally

**Scripts**:
- `TheConfessionEvent.cs`
- `ConfessionTrigger.cs`

### End of Game
The game ends when:
- Only one active player remains, OR
- The Confession resolves fully

If only Zombies remain, they share the entire stash equally.

---

## 6. Scenes

### Scene 1: Bootstrap
**Purpose**: Initialize Photon connection  
**File**: `Bootstrap.unity`

**Functionality**:
- Connect to Photon servers
- Load player preferences
- Transition to Main Menu

**UI Elements**:
- Loading spinner
- "Connecting..." text
- Photon status indicator

**Scripts**:
- `BootstrapManager.cs`
- `PhotonConnector.cs`

---

### Scene 2: Main Menu
**Purpose**: Entry point for players  
**File**: `MainMenu.unity`

**Functionality**:
- Create new room
- Join existing room (via room code)
- Quick match (random room)
- Settings
- Quit game

**UI Elements**:
- Title logo
- "Create Room" button
- "Join Room" button + input field for room code
- "Quick Match" button
- "Settings" button
- "Quit" button
- Player name display

**Settings Menu Options**:
- **Audio**:
  - Master Volume slider
  - Music Volume slider
  - SFX Volume slider
  - Mute toggle
- **Graphics**:
  - Quality preset dropdown (Low/Medium/High)
  - VSync toggle
  - Fullscreen toggle
  - Resolution dropdown
- **Gameplay**:
  - Player name input field
  - Turn timer toggle (optional)
  - Turn timer duration slider (if enabled)
  - Colorblind mode toggle
- **Controls**:
  - Camera sensitivity slider
  - Invert camera toggle
- **Other**:
  - Language dropdown (if localization planned)
  - Credits button
  - Reset to defaults button

**Scripts**:
- `MainMenuManager.cs`
- `RoomCreator.cs`
- `RoomJoiner.cs`
- `MainMenuUI.cs`
- `SettingsManager.cs` - Manages all settings and saves to PlayerPrefs
- `SettingsUI.cs` - UI controller for settings panel

---

### Scene 3: Lobby
**Purpose**: Wait for players and draft characters  
**File**: `Lobby.unity`

**Functionality**:
- Display all players in room
- **Draft Flow**:
1. Players join lobby (need 4 or 5 players)
2. Host clicks "Start Draft"
3. System assigns random draft order (e.g., Player 3 → Player 1 → Player 5 → Player 2 → Player 4)
4. **First player's turn**:
   - Sees 3 random characters from the 5 available
   - Picks one (15s timer)
   - Selected character is removed from pool
5. **Second player's turn**:
   - Sees the 2 unpicked characters from previous player
   - Plus 1 new random character from remaining pool
   - Total: 3 characters (if possible)
   - Picks one
   - Selected character is removed from pool
6. **Third player's turn**:
   - Sees the 2 unpicked characters from previous player
   - Plus 1 new random character from remaining pool
   - Total: 3 characters
   - Picks one
7. **Fourth player's turn**:
   - Sees the 2 unpicked characters from previous player
   - Only 2 characters remain in total pool, so no new random added
   - Picks one
8. **Fifth player** (if 5 players):
   - Gets the last remaining character automatically
9. Draft order = turn order in game
10. Game transitions to Game scenek
- **Draft Timer**: Countdown for current player's pick (15 seconds)
- Player list (4-5 slots):
  - Player name
  - Character portrait (empty until drafted)
  - Draft position number
  - "Picking..." indicator for active player
- **Character Selection Panel** (center):
  - 5 character cards (The Crew)
  - Each card shows:
    - Character portrait
    - Character name
    - Ability description
    - "TAKEN" overlay if already picked
  - Highlight available characters
  - "Select" button for current drafter
- "Start Game" button (host only, enabled when all drafted)
- "Leave Room" button

**Draft Flow**:
1. Players join lobby in order (1st = Host, 2nd, 3rd, etc.)
2. Draft begins automatically when room is full or host starts draft
3. Player 1 picks → Player 2 picks → ... → Player N picks
4. Draft order = turn order in game
5. Once all picked, host can start game

**Scripts**:
- `LobbyManager.cs`
- `LobbyUI.cs`
- `CharacterDraftSystem.cs` - Manages draft order and selection
- `DraftTimer.cs` - Countdown timer for picks
- `CharacterCard.cs` - Individual character card UI
- `RoomCodeDisplay.cs`

---

### Scene 4: Game
**Purpose**: Main gameplay  
**File**: `Game.unity`

**Functionality**:
- Display game board
- Show all players
- Turn management
- Card playing
- Tile interactions
- Casino mini-game
- Win/lose detection

**UI Elements**:

**Top Bar**:
- Current turn indicator
- Round number
- Active Chaos effects display
- Active dagger effects display(Unique for each player)

**Player HUD** (bottom):
- Your bucks
- Your Movement Cards (hand, max 8)
- Your Dagger Cards
- Your character portrait
- Your status (Human/Zombie/Spectre)
- Last Resort cost tracker

**Other Players Panel** (side):
- Each player's name, bucks, card count, status

**Center**:
- Game board (3D view)
- Tile interaction popup
- Casino mini-game overlay

**Card Play Area**:
- Movement card slot (play 1)
- Raffle Ticket slots (play multiple)

**Action Buttons**:
- "End Turn" button
- "Use Ability" button
- "Buy from Last Resort" button(only available when no movement cards in hand)

**Casino Mini-Game UI**:
- 4-card queue display
- "Flip" / "Pass" buttons
- Temp pile display
- Bluff attachment interface
- Opponent view (for non-active players)

**Notifications**:
- Turn start/end
- Card played
- Tile effects
- Player actions
- Chaos events

**Scripts**:
- `GameSceneManager.cs`
- `GameHUD.cs`
- `TurnIndicatorUI.cs`
- `PlayerStatusUI.cs`
- `TileInteractionUI.cs`
- `CasinoUI.cs`
- `NotificationManager.cs`
- `LastResortUI.cs`

---

### Scene 5: Game Over
**Purpose**: Display results  
**File**: `GameOver.unity` (or popup in Game scene)

**Functionality**:
- Show winner(s)
- Display final stats
- Stash distribution breakdown
- Rematch option
- Return to lobby

**UI Elements**:
- Winner announcement (large)
- Stash distribution:
  - Human: 60% or 100%
  - Zombies: 40% shared or 100% shared
- Final scoreboard (all players)
- "Rematch" button
- "Return to Lobby" button
- "Main Menu" button

**Scripts**:
- `GameOverManager.cs`
- `GameOverUI.cs`
- `StashDistribution.cs`
- `ScoreboardDisplay.cs`

---

## 7. Prefabs

### Network Prefabs (Must be in Resources/)
| Prefab Name | Purpose | Components |
|-------------|---------|------------|
| `NetworkedPlayer.prefab` | Player instance | `PhotonView`, `NetworkedPlayer.cs`, `PlayerController.cs` |
| `NetworkedGameManager.prefab` | Game state sync | `PhotonView`, `GameStateSync.cs` |

### Tile Prefabs
| Prefab Name | Purpose |
|-------------|---------|
| `Tile_Start.prefab` | Start tile (from Blender) |
| `Tile_Blank.prefab` | Blank tile (from Blender) |
| `Tile_Casino.prefab` | Casino tile (from Blender) |
| `Tile_Safehouse.prefab` | Safehouse tile (from Blender) |
| `Tile_Vault.prefab` | Vault tile (from Blender) |
| `Tile_PawnShop.prefab` | Pawn Shop tile (from Blender) |
| `Tile_Payphone.prefab` | Payphone tile (from Blender) |

### Player Prefabs
| Prefab Name | Purpose |
|-------------|---------|
| `PlayerToken_Hacker.prefab` | Hacker character token |
| `PlayerToken_Grifter.prefab` | Grifter character token |
| `PlayerToken_Runner.prefab` | Runner character token |
| `PlayerToken_Insider.prefab` | Insider character token |
| `PlayerToken_Thug.prefab` | Thug character token |

### UI Prefabs
| Prefab Name | Purpose |
|-------------|---------|
| `CardUI_Movement.prefab` | Movement card display |
| `CardUI_Dagger.prefab` | Dagger card display (two-faced) |
| `CardUI_Casino.prefab` | Casino card display |
| `PlayerSlot.prefab` | Lobby player slot |
| `NotificationPopup.prefab` | In-game notification |
| `TileInteractionPanel.prefab` | Tile effect UI |
| `CasinoMiniGamePanel.prefab` | Casino mini-game UI |
| `BluffAttachmentUI.prefab` | Bluff attachment interface |

### Effect Prefabs
| Prefab Name | Purpose |
|-------------|---------|
| `MoneyGainEffect.prefab` | Particle effect for gaining bucks |
| `MoneyLoseEffect.prefab` | Particle effect for losing bucks |
| `ZombieTransformEffect.prefab` | Effect when becoming zombie |
| `CardDrawEffect.prefab` | Effect when drawing cards |
| `BustedEffect.prefab` | Effect when player is eliminated |

---

## 8. 3D Models to Create

### Tiles (7 models)
1. **Start Tile** - Golden platform with heist bag icon
2. **Blank Tile** - Simple hexagonal platform
3. **Casino Tile** - Dice, cards, chips theme
4. **Safehouse Tile** - Fortified bunker look
5. **Vault Tile** - Bank vault door
6. **Pawn Shop Tile** - Shop counter with items
7. **Payphone Tile** - Old-school payphone booth

**Specifications**:
- Hexagonal or circular shape
- Low-poly style (500-2000 tris each)
- Distinct colors/themes for easy identification
- Flat top surface for player tokens

### Player Tokens (5 models)
1. **Hacker Token** - Hooded figure with laptop
2. **Grifter Token** - Suited con artist
3. **Runner Token** - Athletic figure
4. **Insider Token** - Professional in disguise
5. **Thug Token** - Tough criminal

**Specifications**:
- Simple character representations
- Low-poly (300-800 tris each)
- Distinct silhouettes
- ~1 unit tall

### Props (Optional)
1. **Money Stack** - Visual for bucks
2. **Card Stack** - Visual for decks
3. **Zombie Indicator** - Flag/marker for zombie players
4. **Bluff Token** - Visual for attached Bluffs

---

## 9. Scripts

### Core Scripts
| Script | Purpose | Location |
|--------|---------|----------|
| `GameManager.cs` | Main game loop controller | `Scripts/Managers/` |
| `TurnManager.cs` | Turn-based logic | `Scripts/Managers/` |
| `RoundManager.cs` | Round tracking | `Scripts/Managers/` |
| `UIManager.cs` | UI state management | `Scripts/Managers/` |
| `AudioManager.cs` | Sound effects and music | `Scripts/Managers/` |

### Networking Scripts
| Script | Purpose | Location |
|--------|---------|----------|
| `NetworkManager.cs` | Photon connection manager | `Scripts/Networking/` |
| `RoomManager.cs` | Room creation/joining | `Scripts/Networking/` |
| `GameStateSync.cs` | Syncs game state | `Scripts/Networking/` |
| `NetworkedPlayer.cs` | Networked player controller | `Scripts/Networking/` |
| `RPCHandler.cs` | Handles all RPC calls | `Scripts/Networking/` |
| `PhotonCallbacks.cs` | Photon event callbacks | `Scripts/Networking/` |

### Board Scripts
| Script | Purpose | Location |
|--------|---------|----------|
| `BoardManager.cs` | Manages board state | `Scripts/Board/` |
| `TileBase.cs` | Abstract base for all tiles | `Scripts/Board/` |
| `TileIdentifier.cs` | Marks tiles in scene | `Scripts/Board/` |
| `StartTile.cs` | Start tile logic | `Scripts/Board/TileEffects/` |
| `BlankTile.cs` | Blank tile logic | `Scripts/Board/TileEffects/` |
| `CasinoTile.cs` | Casino tile logic | `Scripts/Board/TileEffects/` |
| `SafehouseTile.cs` | Safehouse tile logic | `Scripts/Board/TileEffects/` |
| `VaultTile.cs` | Vault tile logic | `Scripts/Board/TileEffects/` |
| `PawnShopTile.cs` | Pawn Shop tile logic | `Scripts/Board/TileEffects/` |
| `PayphoneTile.cs` | Payphone tile logic | `Scripts/Board/TileEffects/` |

### Player Scripts
| Script | Purpose | Location |
|--------|---------|----------|
| `PlayerData.cs` | Player state data | `Scripts/Player/` |
| `PlayerController.cs` | Player controller | `Scripts/Player/` |
| `PlayerMovement.cs` | Movement logic | `Scripts/Player/` |
| `PlayerMoney.cs` | Money (bucks) management | `Scripts/Player/` |
| `PlayerStatus.cs` | Human/Zombie/Spectre status | `Scripts/Player/` |
| `PlayerElimination.cs` | Busted logic | `Scripts/Player/` |
| `ZombieRebirth.cs` | Zombie rebirth logic | `Scripts/Player/` |
| `SpectreMode.cs` | Spectre viewing logic | `Scripts/Player/` |
| `PlayerCostTracker.cs` | Last Resort cost tracking | `Scripts/Player/` |
| `CharacterAbilityBase.cs` | Base ability class | `Scripts/Player/CharacterAbilities/` |
| `HackerAbility.cs` | Hacker ability | `Scripts/Player/CharacterAbilities/` |
| `GrifterAbility.cs` | Grifter ability | `Scripts/Player/CharacterAbilities/` |
| `RunnerAbility.cs` | Runner ability | `Scripts/Player/CharacterAbilities/` |
| `InsiderAbility.cs` | Insider ability | `Scripts/Player/CharacterAbilities/` |
| `ThugAbility.cs` | Thug ability | `Scripts/Player/CharacterAbilities/` |

### Card Scripts - Core
| Script | Purpose | Location |
|--------|---------|----------|
| `CardBase.cs` | Base class for all cards | `Scripts/Cards/` |
| `CardGenerator.cs` | Probabilistic card generation | `Scripts/Cards/` |
| `DeckManager.cs` | Manages all 5 decks | `Scripts/Cards/` |
| `HandManager.cs` | Player hand (max 8) | `Scripts/Cards/` |
| `CardPlayHandler.cs` | Playing cards | `Scripts/Cards/` |

### Card Scripts - Movement
| Script | Purpose | Location |
|--------|---------|----------|
| `MovementCard.cs` | Movement card (M0-M5) | `Scripts/Cards/Movement/` |
| `MovementDeckGenerator.cs` | Movement deck generation | `Scripts/Cards/Movement/` |
| `LastResortShop.cs` | Last Resort shop logic | `Scripts/Cards/Movement/` |

### Card Scripts - Casino
| Script | Purpose | Location |
|--------|---------|----------|
| `CasinoCardGenerator.cs` | Casino card generation | `Scripts/Cards/Casino/` |
| `CasinoCard.cs` | Casino card data | `Scripts/Cards/Casino/` |
| `JokerCard.cs` | Joker penalty logic | `Scripts/Cards/Casino/` |

### Card Scripts - Safehouse
| Script | Purpose | Location |
|--------|---------|----------|
| `SafehouseDeckGenerator.cs` | Safehouse deck generation | `Scripts/Cards/Safehouse/` |

### Card Scripts - Chaos
| Script | Purpose | Location |
|--------|---------|----------|
| `ChaosDeckGenerator.cs` | Chaos deck generation | `Scripts/Cards/Chaos/` |
| `ChaosCardBase.cs` | Base class for Chaos cards | `Scripts/Cards/Chaos/` |
| `MarketCrashCard.cs` | Market Crash effect | `Scripts/Cards/Chaos/ChaosEffects/` |
| `StingOperationCard.cs` | Sting Operation effect | `Scripts/Cards/Chaos/ChaosEffects/` |
| `DistractedCard.cs` | Distracted effect | `Scripts/Cards/Chaos/ChaosEffects/` |
| `StockExchangeCard.cs` | Stock Exchange effect | `Scripts/Cards/Chaos/ChaosEffects/` |
| `PoliceRaidCard.cs` | Police Raid effect | `Scripts/Cards/Chaos/ChaosEffects/` |
| `ZombieApocalypseCard.cs` | Zombie Apocalypse effect | `Scripts/Cards/Chaos/ChaosEffects/` |
| `TheConfessionCard.cs` | The Confession event | `Scripts/Cards/Chaos/ChaosEffects/` |

### Card Scripts - Dagger
| Script | Purpose | Location |
|--------|---------|----------|
| `DaggerDeckGenerator.cs` | Dagger deck generation | `Scripts/Cards/Dagger/` |
| `DaggerCard.cs` | Two-faced card data | `Scripts/Cards/Dagger/` |
| `BluffFace.cs` | Bluff side logic | `Scripts/Cards/Dagger/` |
| `RaffleTicketFace.cs` | Raffle Ticket side logic | `Scripts/Cards/Dagger/` |

### Card Scripts - Dagger Bluffs
| Script | Purpose | Location |
|--------|---------|----------|
| `JokerBluff.cs` | Joker Bluff | `Scripts/Cards/Dagger/Bluffs/` |
| `InvertedBluff.cs` | Inverted Bluff | `Scripts/Cards/Dagger/Bluffs/` |
| `DoubleBluff.cs` | Double Bluff | `Scripts/Cards/Dagger/Bluffs/` |
| `ScamBluff.cs` | Scam Bluff | `Scripts/Cards/Dagger/Bluffs/` |
| `CallBluff.cs` | Call Bluff | `Scripts/Cards/Dagger/Bluffs/` |

### Card Scripts - Dagger Raffle Tickets
| Script | Purpose | Location |
|--------|---------|----------|
| `RobTheRobberCard.cs` | Rob the Robber | `Scripts/Cards/Dagger/RaffleTickets/` |
| `HiredHelpCard.cs` | Hired Help | `Scripts/Cards/Dagger/RaffleTickets/` |
| `ImmunityCard.cs` | Immunity | `Scripts/Cards/Dagger/RaffleTickets/` |
| `HackerCard.cs` | Hacker | `Scripts/Cards/Dagger/RaffleTickets/` |
| `ProfessionalCard.cs` | Professional | `Scripts/Cards/Dagger/RaffleTickets/` |

### Casino Mini-Game Scripts
| Script | Purpose | Location |
|--------|---------|----------|
| `CasinoMiniGame.cs` | Main mini-game controller | `Scripts/Casino/` |
| `CasinoQueue.cs` | 4-card queue management | `Scripts/Casino/` |
| `BluffSystem.cs` | Bluff attachments & reveals | `Scripts/Casino/` |
| `TempPile.cs` | Temporary card pile | `Scripts/Casino/` |
| `CasinoUI.cs` | Casino UI controller | `Scripts/Casino/` |

### UI Scripts
| Script | Purpose | Location |
|--------|---------|----------|
| `MainMenuUI.cs` | Main menu UI | `Scripts/UI/MainMenu/` |
| `SettingsManager.cs` | Settings management & persistence | `Scripts/UI/MainMenu/` |
| `SettingsUI.cs` | Settings panel UI | `Scripts/UI/MainMenu/` |
| `LobbyUI.cs` | Lobby UI | `Scripts/UI/Lobby/` |
| `CharacterDraftSystem.cs` | Draft order & selection logic | `Scripts/UI/Lobby/` |
| `DraftTimer.cs` | Draft countdown timer | `Scripts/UI/Lobby/` |
| `CharacterCard.cs` | Character card UI | `Scripts/UI/Lobby/` |
| `GameHUD.cs` | Game HUD | `Scripts/UI/Game/` |
| `PlayerStatusUI.cs` | Player status display | `Scripts/UI/Game/` |
| `TileInteractionUI.cs` | Tile interaction popup | `Scripts/UI/Game/` |
| `NotificationManager.cs` | Notifications | `Scripts/UI/Game/` |
| `GameOverUI.cs` | Game over screen | `Scripts/UI/Game/` |
| `LastResortUI.cs` | Last Resort shop UI | `Scripts/UI/Game/` |
| `StashDistribution.cs` | Stash distribution display | `Scripts/UI/Game/` |

### Utility Scripts
| Script | Purpose | Location |
|--------|---------|----------|
| `GameEnums.cs` | All game enums | `Scripts/Core/Data/` |
| `GameConstants.cs` | Game constants | `Scripts/Core/Data/` |
| `ProbabilityHelper.cs` | Probabilistic generation helper | `Scripts/Core/Utilities/` |
| `Extensions.cs` | Extension methods | `Scripts/Core/Utilities/` |

**Total Scripts**: ~100+

---

## 10. ScriptableObjects

### Character Profiles
**Location**: `Assets/ScriptableObjects/Characters/`

| File | Purpose |
|------|---------|
| `Character_Hacker.asset` | Hacker character data |
| `Character_Grifter.asset` | Grifter character data |
| `Character_Runner.asset` | Runner character data |
| `Character_Insider.asset` | Insider character data |
| `Character_Thug.asset` | Thug character data |

**Script**: `CharacterProfile.cs`

**Fields**:
- Character Name
- Description
- Ability Name
- Ability Description
- Character Portrait (Sprite)
- Character Model (Prefab)

---

### Card Data - Movement
**Location**: `Assets/ScriptableObjects/Cards/Movement/`

- `Card_M0.asset`
- `Card_M1.asset`
- `Card_M2.asset`
- `Card_M3.asset`
- `Card_M4.asset`
- `Card_M5.asset`

**Script**: `MovementCardData.cs`

**Fields**:
- Card Name
- Movement Value (0-5)
- Card Art (Sprite)

---

### Card Data - Dagger
**Location**: `Assets/ScriptableObjects/Cards/Dagger/`

- `Dagger_JokerBluff_RobTheRobber.asset`
- `Dagger_InvertedBluff_HiredHelp.asset`
- `Dagger_DoubleBluff_Immunity.asset`
- `Dagger_ScamBluff_Hacker.asset`
- `Dagger_CallBluff_Professional.asset`

**Script**: `DaggerCardData.cs`

**Fields**:
- Card Name
- Bluff Face Type
- Bluff Face Description
- Bluff Face Art (Sprite)
- Raffle Ticket Face Type
- Raffle Ticket Face Description
- Raffle Ticket Face Art (Sprite)

---

### Card Data - Casino
**Location**: `Assets/ScriptableObjects/Cards/Casino/`

- `Casino_M0.asset`
- `Casino_M1.asset`
- `Casino_M2.asset`
- `Casino_M3.asset`
- `Casino_M4.asset`
- `Casino_M5.asset`
- `Casino_Dud.asset`
- `Casino_Joker_Lose1Card.asset`
- `Casino_Joker_Lose2Cards.asset`
- `Casino_Joker_Lose3Bucks.asset`
- `Casino_Joker_Lose5Bucks.asset`
- `Casino_Joker_Lose7Bucks.asset`

**Script**: `CasinoCardData.cs`

**Fields**:
- Card Type (Movement/Dud/Joker)
- Effect Description
- Card Art (Sprite)
- Joker Penalty (if applicable)

---

### Card Data - Chaos
**Location**: `Assets/ScriptableObjects/Cards/Chaos/`

- `Chaos_MarketCrash.asset`
- `Chaos_StingOperation.asset`
- `Chaos_Distracted.asset`
- `Chaos_StockExchange.asset`
- `Chaos_PoliceRaid.asset`
- `Chaos_ZombieApocalypse.asset`
- `Chaos_TheConfession.asset`

**Script**: `ChaosCardData.cs`

**Fields**:
- Card Name
- Effect Description
- Duration (if applicable)
- Card Art (Sprite)

---

### Deck Generation Settings
**Location**: `Assets/ScriptableObjects/DeckSettings/`

| File | Purpose |
|------|---------|
| `CasinoDeckSettings.asset` | Probabilities for Casino cards |
| `SafehouseDeckSettings.asset` | Probabilities for Safehouse cards |
| `ChaosDeckSettings.asset` | Probabilities for Chaos cards |
| `DaggerDeckSettings.asset` | Probabilities for Dagger cards |

**Script**: `DeckGenerationSettings.cs`

**Fields**:
- Card Type Probabilities (dictionary)
- Joker Escalation Curve (for Casino)

---

### Game Settings
**Location**: `Assets/ScriptableObjects/`

| File | Purpose |
|------|---------|
| `GameSettings.asset` | Global game settings |

**Script**: `GameSettings.cs`

**Fields**:
- Starting Money (10 bucks)
- Starting Movement Cards (5)
- Max Hand Size (8)
- Last Resort Cost Scale (3, 5, 7, 10, 12, 15...)
- Player Count (4-5)
- Board Tile Count (30-40)

---

## Summary

- **7 Tile Types** (30-40 tiles total)
- **5 Card Decks** (Casino, Safehouse, Chaos, Dagger, Movement)
- **5 Characters** (The Crew)
- **5 Scenes** (Bootstrap, Main Menu, Lobby, Game, Game Over)
- **~30 Prefabs** (networked, tiles, players, UI, effects)
- **12 3D Models** (7 tiles + 5 player tokens)
- **~100+ Scripts** organized by feature
- **~35 ScriptableObjects** for data

---

**This document is the single source of truth for the game. All implementation should reference this document.**
