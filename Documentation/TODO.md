# Last Man's Stash - Development TODO

**Game**: 4-5 Player Online Multiplayer Heist Board Game  
**Engine**: Unity + Photon PUN 2  
**Author**: HeartOfPaper  
**Repository**: https://github.com/HeartOfPaper/last-mans-stash

---

## 🔀 Git Branching Workflow

### Branch Strategy

**Main Branch**: `main` (protected - only merge from phase branches)

**Phase Branches**: Create a new branch for each phase:
```
phase-1-foundation          ✅ MERGED
phase-2-data-structures     ✅ MERGED
phase-3-board-system        ⏳ CURRENT
phase-4-ui-main-menu
phase-5-networking-setup
phase-6-lobby-system
phase-7-card-system
phase-8-turn-system
phase-9-game-mechanics
phase-10-casino-minigame
phase-11-polish-optimization
phase-12-testing-bugfixes
phase-13-audio-vfx
phase-14-balancing
phase-15-release-prep
```

### Workflow Commands

**1. Start New Phase**:
```bash
# Create and switch to new phase branch
git checkout -b phase-3-board-system

# Push branch to GitHub
git push -u origin phase-3-board-system
```

**2. During Development**:
```bash
# Commit frequently with clear messages
git add .
git commit -m "Added BoardManager spatial ordering algorithm"
git push
```

**3. Complete Phase**:
```bash
# Switch to main and merge
git checkout main
git merge phase-3-board-system --no-ff -m "Merge Phase 3: Board System Complete"

# Push merged main
git push origin main

# Delete local branch (optional)
git branch -d phase-3-board-system

# Delete remote branch (optional)
git push origin --delete phase-3-board-system
```

### Commit Message Format

**Structure**: `[Phase X] Category: Description`

**Examples**:
- `[Phase 3] Board: Added BoardManager with spatial tile ordering`
- `[Phase 4] UI: Created MainMenu scene with responsive layout`
- `[Phase 5] Network: Implemented RPC for player actions`
- `[Fix] Security: Changed PlayerData methods to internal`
- `[Docs] Updated README with installation instructions`

**Categories**: Board, UI, Network, Cards, Mechanics, Audio, VFX, Polish, Fix, Docs, Test

---

## 📋 Current Status

**Latest Branch**: `main` (Phase 1 & 2 complete)  
**Next Branch**: `phase-3-board-system`  
**Overall Progress**: ~7% (2/15 phases complete)

---

**Legend**: 
- `[ ]` Not started
- `[/]` In progress
- `[x]` Completed

---

## Phase 1: Foundation & Setup

### Photon Integration
- [x] Install Photon PUN 2 from Unity Asset Store
- [x] Create Photon account and get AppId
- [x] Configure Photon AppId in Unity
- [x] Test basic connection to Photon servers

### Project Structure
- [x] Create folder structure:
  - [x] `Scripts/Core/Data/`
  - [x] `Scripts/Core/GameRules/`
  - [x] `Scripts/Core/Utilities/`
  - [x] `Scripts/Board/`
  - [x] `Scripts/Board/TileEffects/`
  - [x] `Scripts/Player/`
  - [x] `Scripts/Player/CharacterAbilities/`
  - [x] `Scripts/Cards/`
  - [x] `Scripts/Cards/Movement/`
  - [x] `Scripts/Cards/Casino/`
  - [x] `Scripts/Cards/Safehouse/`
  - [x] `Scripts/Cards/Chaos/`
  - [x] `Scripts/Cards/Chaos/ChaosEffects/`
  - [x] `Scripts/Cards/Dagger/`
  - [x] `Scripts/Cards/Dagger/Bluffs/`
  - [x] `Scripts/Cards/Dagger/RaffleTickets/`
  - [x] `Scripts/Casino/`
  - [x] `Scripts/Networking/`
  - [x] `Scripts/Managers/`
  - [x] `Scripts/UI/MainMenu/`
  - [x] `Scripts/UI/Lobby/`
  - [x] `Scripts/UI/Game/`
  - [x] `Prefabs/Tiles/`
  - [x] `Prefabs/Player/`
  - [x] `Prefabs/UI/`
  - [x] `Prefabs/NetworkedObjects/`
  - [x] `Resources/PhotonPrefabs/`
  - [x] `Scenes/`
  - [x] `ScriptableObjects/Characters/`
  - [x] `ScriptableObjects/Cards/Movement/`
  - [x] `ScriptableObjects/Cards/Dagger/`
  - [x] `ScriptableObjects/Cards/Casino/`
  - [x] `ScriptableObjects/Cards/Chaos/`
  - [x] `ScriptableObjects/DeckSettings/`

### Bootstrap Scene
- [x] Create `Bootstrap.unity` scene
- [x] Create `BootstrapManager.cs`
- [x] Create `PhotonConnector.cs`
- [x] Implement Photon connection logic
- [x] Create loading UI (spinner, status text)
  - **Font**: Special Elite, 36pt
- [ ] Test scene transition to Main Menu

---

## Font Style Guide

**Fonts Used**: Bebas Neue, Archivo Black, Special Elite

### Typography Rules:
- **Bebas Neue** (Big, Bold, Uppercase)
  - Game titles, scene headers, player names, big numbers
  - Sizes: 48-72pt
  
- **Archivo Black** (Strong Headers & Numbers)
  - Section headers, buttons, timers, money displays
  - Sizes: 24-48pt
  
- **Special Elite** (Body Text, Crime/Noir Feel)
  - Status text, descriptions, labels, chat
  - Sizes: 14-36pt

---

## Phase 2: Core Data Structures

### Enums & Constants
- [x] Create `GameEnums.cs` with all enums:
  - [x] TileType
  - [x] GameState
  - [x] PlayerStatus (Human/Zombie/Spectre)
  - [x] CardType
  - [x] ChaosEventType
  - [x] MovementValue
  - [x] DaggerBluffType
  - [x] DaggerRaffleType
  - [x] CharacterType
  - [x] TurnPhase
  - [x] CasinoCardType
  - [x] JokerPenalty
- [x] Create `GameConstants.cs` with constants:
  - [x] Starting money (10)
  - [x] Starting cards (5)
  - [x] Max hand size (8)
  - [x] Last Resort cost scale
  - [x] Draft timer (15s)
  - [x] All card probabilities
  - [x] Chaos effect durations
  - [x] Character ability values
  - [x] Win condition percentages

### Tile System
- [x] Create `Tile.cs` data class
- [x] Create `TileIdentifier.cs` component

### Player Data
- [x] Create `PlayerData.cs` with:
  - [x] Money (bucks)
  - [x] Movement cards list
  - [x] Dagger cards list
  - [x] Character reference
  - [x] Status (Human/Zombie/Spectre)
  - [x] Last Resort cost tracker
  - [x] Active effects list
  - [x] Character ability tracking

### Card Data
- [x] Create `CardBase.cs` abstract class
- [x] Create `MovementCard.cs`
- [x] Create `DaggerCard.cs` (two-faced)
- [x] Create `CasinoCard.cs`
- [x] Create `ChaosCardBase.cs`

### Character Profiles
- [x] Create `CharacterProfile.cs` ScriptableObject
- [x] Create 7 character profile assets:
  - [x] Hacker
  - [x] Grifter
  - [x] Runner
  - [x] Insider
  - [x] Thug
  - [x] Smuggler
  - [x] Mastermind

---

## Phase 3: Board System

### Board Manager
- [x] Create `BoardManager.cs`
- [x] Implement tile finding (FindObjectsOfType<TileIdentifier>)
- [x] Implement spatial ordering (clockwise from Start)
- [x] Implement board center calculation
- [x] Add visual debugging (gizmos)

### Tile Base System
- [x] Create `TileBase.cs` abstract class with:
  - [x] OnLanded() method
  - [x] OnPassed() method
  - [x] Tile type property

### Tile Implementations
- [x] Create `StartTile.cs` (gain 5 on land, 3 on pass)
- [x] Create `BlankTile.cs` (no effect)
- [x] Create `CasinoTile.cs` (trigger mini-game)
- [x] Create `SafehouseTile.cs` (draw movement card)
- [x] Create `VaultTile.cs` (steal bucks)
- [x] Create `PawnShopTile.cs` (draw dagger card)
- [x] Create `PayphoneTile.cs` (draw chaos card)

### Testing
- [ ] Create test scene with placeholder tiles
- [ ] Test tile ordering
- [ ] Verify gizmos show correct path

---

## Phase 4: Networking - Rooms & Lobby

### Network Manager
- [ ] Create `NetworkManager.cs`
- [ ] Implement Photon callbacks
- [ ] Create `PhotonCallbacks.cs`

### Room System
- [ ] Create `RoomManager.cs`
- [ ] Implement room creation
- [ ] Implement room joining (by code)
- [ ] Implement quick match (random room)
- [ ] Generate unique room codes

### Main Menu Scene
- [ ] Create `MainMenu.unity` scene
- [ ] Create `MainMenuManager.cs`
- [ ] Create `MainMenuUI.cs`
- [ ] Create UI elements:
  - [ ] Title logo - **Bebas Neue, 72pt, uppercase**
  - [ ] Create Room button - **Archivo Black, 28pt, uppercase**
  - [ ] Join Room button + input field - **Archivo Black, 28pt button / Special Elite, 20pt input**
  - [ ] Quick Match button - **Archivo Black, 28pt, uppercase**
  - [ ] Settings button - **Archivo Black, 24pt**
  - [ ] Quit button - **Archivo Black, 24pt**

### Settings System
- [ ] Create `SettingsManager.cs`
- [ ] Create `SettingsUI.cs`
- [ ] Implement settings:
  - [ ] Audio (Master, Music, SFX, Mute)
  - [ ] Graphics (Quality, VSync, Fullscreen, Resolution)
  - [ ] Gameplay (Player name, Turn timer, Colorblind mode)
  - [ ] Controls (Camera sensitivity, Invert)
- [ ] Save/load settings to PlayerPrefs

### Lobby Scene
- [ ] Create `Lobby.unity` scene
- [ ] Create `LobbyManager.cs`
- [ ] Create `LobbyUI.cs`
- [ ] Create `RoomCodeDisplay.cs` - **Archivo Black, 36pt**
- [ ] Display player list (4-5 slots) - **Player names: Bebas Neue, 24pt**
- [ ] Show room code - **Archivo Black, 36pt**

### Draft System
- [ ] Create `CharacterDraftSystem.cs`
- [ ] Implement randomized draft order assignment - **Display: Special Elite, 18pt**
- [ ] Implement 3-character selection pool
- [ ] Implement carry-over mechanic (2 unpicked + 1 new random)
- [ ] Create `DraftTimer.cs` (15s countdown) - **Archivo Black, 32pt**
- [ ] Create `CharacterCard.cs` UI component:
  - [ ] Character name - **Archivo Black, 24pt, uppercase**
  - [ ] Ability description - **Special Elite, 14pt**
- [ ] Implement auto-assign on timeout
- [ ] Sync draft state across network
- [ ] Test draft with multiple clients

---

## Phase 5: Player System & Synchronization

### Networked Player
- [ ] Create `NetworkedPlayer.cs` with PhotonView
- [ ] Implement player spawning
- [ ] Sync player data across network:
  - [ ] Money
  - [ ] Card counts
  - [ ] Position
  - [ ] Status (Human/Zombie/Spectre)

### Player Controller
- [ ] Create `PlayerController.cs`
- [ ] Create `PlayerMovement.cs`
- [ ] Create `PlayerMoney.cs`
- [ ] Create `PlayerStatus.cs`
- [ ] Implement movement around board
- [ ] Add visual player token

### Player Elimination
- [ ] Create `PlayerElimination.cs`
- [ ] Implement Busted logic (can't afford Last Resort)
- [ ] Create `ZombieRebirth.cs`
- [ ] Implement Zombie Apocalypse rebirth
- [ ] Create `SpectreMode.cs`
- [ ] Allow Spectres to view Movement Cards or Bucks

### Cost Tracker
- [ ] Create `PlayerCostTracker.cs`
- [ ] Track Last Resort purchases per player
- [ ] Implement escalating cost (3, 5, 7, 10, 12, 15...)

---

## Phase 6: Turn System

### Turn Manager
- [ ] Create `TurnManager.cs`
- [ ] Implement turn order (from draft order)
- [ ] Create `RoundManager.cs` (track rounds)
- [ ] Implement turn phases:
  - [ ] Draw phase
  - [ ] Action phase (play cards)
  - [ ] Movement phase
  - [ ] Tile effect phase
  - [ ] End phase (hand limit check)

### Turn UI
- [ ] Create `TurnIndicatorUI.cs`
- [ ] Show current player's turn - **Bebas Neue, 28pt**
- [ ] Show round number - **Bebas Neue, 28pt**
- [ ] Show active Chaos effects - **Special Elite, 16pt**
- [ ] Show active Dagger effects (per player) - **Special Elite, 16pt**
- [ ] Create "End Turn" button - **Archivo Black, 28pt**

### Network Sync
- [ ] Sync turn state across all clients
- [ ] Implement turn validation (only active player can act)
- [ ] Test turn progression with multiple players

---

## Phase 7: Card System

### Deck Managers
- [ ] Create `DeckManager.cs` (manages all 5 decks)
- [ ] Create `CardGenerator.cs` (probabilistic generation)
- [ ] Create `ProbabilityHelper.cs` utility

### Movement Cards
- [ ] Create `MovementDeckGenerator.cs`
- [ ] Implement M0-M5 card generation
- [ ] Create 6 Movement Card ScriptableObjects

### Safehouse Deck
- [ ] Create `SafehouseDeckGenerator.cs`
- [ ] Implement probabilistic M1-M5 generation (30%, 25%, 20%, 15%, 10%)

### Casino Deck
- [ ] Create `CasinoCardGenerator.cs`
- [ ] Implement card probabilities:
  - [ ] M0 (5%)
  - [ ] M1-M3 (40%)
  - [ ] M4-M5 (10%)
  - [ ] Duds (20%)
  - [ ] Jokers (25%)
- [ ] Create `JokerCard.cs` with penalty variants
- [ ] Implement Joker escalation (5% → 25%+)
- [ ] Create Casino Card ScriptableObjects

### Chaos Deck
- [ ] Create `ChaosDeckGenerator.cs`
- [ ] Create `ChaosCardBase.cs`
- [ ] Implement Chaos cards:
  - [ ] `MarketCrashCard.cs` (Last Resort x2 for 3 rounds)
  - [ ] `StingOperationCard.cs` (Safehouses → Hazards for 3 rounds)
  - [ ] `DistractedCard.cs` (M4/M5 → M1 for 2 rounds)
  - [ ] `StockExchangeCard.cs` (pass hands left)
  - [ ] `PoliceRaidCard.cs` (pay 10 or discard 1 card)
  - [ ] `ZombieApocalypseCard.cs` (enable rebirth)
  - [ ] `TheConfessionCard.cs` (special trigger)
- [ ] Create Chaos Card ScriptableObjects

### Dagger Deck
- [ ] Create `DaggerDeckGenerator.cs`
- [ ] Create `DaggerCard.cs` (two-faced)
- [ ] Create `BluffFace.cs`
- [ ] Create `RaffleTicketFace.cs`
- [ ] Implement Bluff cards:
  - [ ] `JokerBluff.cs` (steal from temp pile)
  - [ ] `InvertedBluff.cs` (invert effects)
  - [ ] `DoubleBluff.cs` (double effects)
  - [ ] `ScamBluff.cs` (discard card)
  - [ ] `CallBluff.cs` (cancel another bluff)
- [ ] Implement Raffle Ticket cards:
  - [ ] `RobTheRobberCard.cs` (counter Vault)
  - [ ] `HiredHelpCard.cs` (rearrange Casino queue)
  - [ ] `ImmunityCard.cs` (ignore Hazard/Vault for 3 turns)
  - [ ] `HackerCard.cs` (use lowest cost for Last Resort)
  - [ ] `ProfessionalCard.cs` (steal random Movement Card)
- [ ] Create Dagger Card ScriptableObjects

### Hand Management
- [ ] Create `HandManager.cs`
- [ ] Implement max hand size (8 Movement Cards)
- [ ] Create hand UI
- [ ] Implement card drawing
- [ ] Create `CardPlayHandler.cs`
- [ ] Validate card plays (1 Movement, multiple Raffle Tickets)

### Last Resort Shop
- [ ] Create `LastResortShop.cs`
- [ ] Implement forced purchase when no Movement Cards
- [ ] Apply escalating cost per player
- [ ] Create `LastResortUI.cs`
- [ ] Show "Buy from Last Resort" button (only when no Movement Cards):
  - [ ] Button text - **Archivo Black, 28pt, uppercase**
  - [ ] Cost display - **Archivo Black, 36pt (numbers), Special Elite, 18pt (label)**

---

## Phase 8: Casino Mini-Game

### Core System
- [ ] Create `CasinoMiniGame.cs`
- [ ] Create `CasinoQueue.cs` (4-card queue)
- [ ] Implement queue generation (maintain 4 cards)
- [ ] Create `TempPile.cs` (temporary card storage)

### Bluff System
- [ ] Create `BluffSystem.cs`
- [ ] Allow opponents to attach Bluffs to unseen cards
- [ ] Implement Bluff reveal sequence
- [ ] Implement Bluff effects
- [ ] Handle Inverted Bluff interactions

### Casino UI
- [ ] Create `CasinoUI.cs`
- [ ] Show 4-card queue (Card #1 visible to opponents only)
  - [ ] Card numbers - **Bebas Neue, 32pt**
  - [ ] Card type labels - **Special Elite, 16pt**
- [ ] Create "Flip" / "Pass" buttons - **Archivo Black, 32pt, uppercase**
- [ ] Show Temp pile - **Card count: Archivo Black, 24pt**
- [ ] Create Bluff attachment interface:
  - [ ] Bluff names - **Special Elite, 16pt**
  - [ ] Effects - **Special Elite, 14pt**
- [ ] Show opponent view (for non-active players) - **Special Elite, 18pt**

### Testing
- [ ] Test Flip/Pass decisions
- [ ] Test Joker escalation
- [ ] Test Bluff attachments
- [ ] Test Inverted Bluff interactions
- [ ] Test with multiple players

---

## Phase 9: Character Abilities

### Base System
- [ ] Create `CharacterAbilityBase.cs`
- [ ] Create `AbilityManager.cs`

### Character Implementations
- [ ] Create `HackerAbility.cs` (2 bucks off for 3 Last Resort purchases)
- [ ] Create `GrifterAbility.cs` (steal 8 instead of 5, every other turn)
- [ ] Create `RunnerAbility.cs` (draw 1 extra card every 4 rounds)
- [ ] Create `InsiderAbility.cs` (peek at Casino card twice per game)
- [ ] Create `ThugAbility.cs` (start with Professional Dagger Card)

### Testing
- [ ] Test each ability
- [ ] Verify network sync
- [ ] Test ability UI indicators

---

## Phase 10: Game Rules & Win Conditions

### Win Condition System
- [ ] Implement Human win (last standing, 100% stash)
- [ ] Implement Zombie team win (60% Human, 40% Zombies)
- [ ] Implement all-Zombie win (100% shared)

### The Confession
- [ ] Create `TheConfessionEvent.cs`
- [ ] Create `ConfessionTrigger.cs`
- [ ] Trigger when 2 Humans + 3+ Zombies exist
- [ ] Implement solo Human vs all Zombies endgame
- [ ] Handle 3 outcomes:
  - [ ] Solo Human survives (100%)
  - [ ] Solo Human turns Zombie (100% shared)
  - [ ] Solo Human Busted (100% shared)

### Game Over
- [ ] Create `GameOverManager.cs`
- [ ] Create `GameOverUI.cs`
- [ ] Create `StashDistribution.cs`
- [ ] Show winner(s) - **Bebas Neue, 60pt, uppercase**
- [ ] Display stash distribution - **Archivo Black, 28pt (percentages), Special Elite, 16pt (labels)**
- [ ] Show final scoreboard - **Player names: Bebas Neue 24pt, Stats: Archivo Black 18pt**
- [ ] Add buttons - **Archivo Black, 24pt**:
  - [ ] "Rematch"
  - [ ] "Return to Lobby"
  - [ ] "Main Menu"

---

## Phase 11: Game Scene & HUD

### Game Scene Setup
- [ ] Create `Game.unity` scene
- [ ] Create `GameSceneManager.cs`
- [ ] Set up camera
- [ ] Set up lighting

### Game HUD
- [ ] Create `GameHUD.cs`
- [ ] Implement Top Bar:
  - [ ] Current turn indicator - **Bebas Neue, 24pt**
  - [ ] Round number - **Bebas Neue, 24pt**
  - [ ] Active Chaos effects display - **Special Elite, 14pt**
  - [ ] Active Dagger effects display (per player) - **Special Elite, 14pt**
- [ ] Implement Player HUD (bottom):
  - [ ] Your bucks - **Archivo Black, 32pt (numbers), Special Elite 18pt (label)**
  - [ ] Your Movement Cards (hand) - **Card values: Bebas Neue, 20pt**
  - [ ] Your Dagger Cards - **Card names: Special Elite, 14pt**
  - [ ] Your character portrait
  - [ ] Your status (Human/Zombie/Spectre) - **Special Elite, 16pt**
  - [ ] Last Resort cost tracker - **Archivo Black, 20pt**
- [ ] Implement Other Players Panel (side):
  - [ ] Each player's name - **Bebas Neue, 20pt**
  - [ ] Bucks, card count - **Archivo Black, 18pt**
  - [ ] Status - **Special Elite, 14pt**

### Tile Interaction UI
- [ ] Create `TileInteractionUI.cs`
- [ ] Create interaction panels for each tile type
- [ ] Test tile interactions

### Notifications
- [ ] Create `NotificationManager.cs`
- [ ] Show notifications for:
  - [ ] Turn start/end
  - [ ] Card played
  - [ ] Tile effects
  - [ ] Player actions
  - [ ] Chaos events

---

## Phase 12: 3D Assets Integration

### Tile Models (Create in Blender)
- [ ] Start Tile (golden platform with heist bag)
- [ ] Blank Tile (simple hexagon)
- [ ] Casino Tile (dice, cards, chips)
- [ ] Safehouse Tile (fortified bunker)
- [ ] Vault Tile (bank vault door)
- [ ] Pawn Shop Tile (shop counter)
- [ ] Payphone Tile (payphone booth)

### Player Token Models (Create in Blender)
- [ ] Hacker Token (hooded figure with laptop)
- [ ] Grifter Token (suited con artist)
- [ ] Runner Token (athletic figure)
- [ ] Insider Token (professional in disguise)
- [ ] Thug Token (tough criminal)

### Import & Setup
- [ ] Import all tile models
- [ ] Import all player token models
- [ ] Set up materials and textures
- [ ] Create prefabs
- [ ] Add TileIdentifier components to tiles
- [ ] Test in scene

---

## Phase 13: Polish & Effects

### Visual Effects
- [ ] Create `MoneyGainEffect.prefab` (particle effect)
- [ ] Create `MoneyLoseEffect.prefab` (particle effect)
- [ ] Create `ZombieTransformEffect.prefab`
- [ ] Create `CardDrawEffect.prefab`
- [ ] Create `BustedEffect.prefab`

### Audio
- [ ] Create `AudioManager.cs`
- [ ] Add background music
- [ ] Add SFX for:
  - [ ] Card draw
  - [ ] Card play
  - [ ] Money gain/loss
  - [ ] Tile landing
  - [ ] Casino flip
  - [ ] Zombie transformation
  - [ ] Player elimination
  - [ ] Turn start/end

### Animations
- [ ] Player token movement animation
- [ ] Card flip animations
- [ ] UI transitions
- [ ] Button hover effects

---

## Phase 14: Testing & Bug Fixes

### Single Player Testing
- [ ] Test all tile effects
- [ ] Test all card effects
- [ ] Test character abilities
- [ ] Test Casino mini-game
- [ ] Test Last Resort shop
- [ ] Test win conditions

### Multiplayer Testing
- [ ] Test with 4 players
- [ ] Test with 5 players
- [ ] Test draft system
- [ ] Test turn progression
- [ ] Test network sync
- [ ] Test disconnection handling
- [ ] Test host migration
- [ ] Test The Confession event

### Edge Cases
- [ ] Player disconnects mid-turn
- [ ] Host leaves room
- [ ] Invalid moves/actions
- [ ] Simultaneous actions
- [ ] Game state desync

---

## Phase 15: Final Polish & Release Prep

### Optimization
- [ ] Optimize 3D models (poly count)
- [ ] Optimize network traffic
- [ ] Reduce memory usage
- [ ] Profile performance

### UI/UX Polish
- [ ] Add tooltips
- [ ] Add help/tutorial screen
- [ ] Improve visual feedback
- [ ] Add loading screens
- [ ] Add error messages

### Documentation
- [ ] Write player guide
- [ ] Create tutorial
- [ ] Document controls
- [ ] Add credits screen

### Build & Deploy
- [ ] Test build on Windows
- [ ] Test build on Mac
- [ ] Test build on Linux (if applicable)
- [ ] Prepare for Steam/Epic Games Store
- [ ] Create store page assets
- [ ] Submit for review

---

## Ongoing Tasks

### Code Quality
- [ ] Add comments to complex logic
- [ ] Refactor duplicate code
- [ ] Follow naming conventions
- [ ] Remove debug code

### Version Control
- [ ] Commit regularly
- [ ] Write meaningful commit messages
- [ ] Create branches for features
- [ ] Merge to main when stable

---

**Total Tasks**: ~300+

**Estimated Timeline**: 3-4 weeks of focused development

**Last Updated**: 2025-12-01
