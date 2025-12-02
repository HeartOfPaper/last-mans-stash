# Last Man's Stash - Project State Review

**Last Updated**: 2025-12-01  
**Phase Completed**: Phase 1 (Foundation & Setup)  
**Purpose**: Ensures consistency across different development sessions and models

---

## 🎯 Project Overview

**Game Name**: Last Man's Stash  
**Genre**: Multiplayer Turn-Based Board Game (Heist/Cops & Robbers Theme)  
**Players**: 4-5  
**Networking**: Photon PUN 2  
**Unity Version**: (Current project version)  
**Platform Target**: PC (Steam/Epic Games Store compatible)

---

## ✅ Completed Work (Phase 1)

### 1. Project Structure ✓
All folders created in `Assets/`:

```
Assets/
├── Fonts/                          ← User added: Bebas Neue, Archivo Black, Special Elite
├── Prefabs/
│   ├── Effects/
│   ├── NetworkedObjects/
│   ├── Player/
│   ├── Tiles/
│   └── UI/
├── Resources/
│   └── PhotonPrefabs/              ← Networked prefabs MUST go here
├── Scenes/
│   └── Bootstrap.unity             ← ✓ Created
├── ScriptableObjects/
│   ├── Cards/
│   │   ├── Casino/
│   │   ├── Chaos/
│   │   ├── Dagger/
│   │   └── Movement/
│   ├── Characters/                 ← Contains 5 existing character assets
│   └── DeckSettings/
└── Scripts/
    ├── Board/
    │   └── TileEffects/
    ├── Cards/
    │   ├── Casino/
    │   ├── Chaos/
    │   │   └── ChaosEffects/
    │   ├── Dagger/
    │   │   ├── Bluffs/
    │   │   └── RaffleTickets/
    │   ├── Movement/
    │   └── Safehouse/
    ├── Casino/
    ├── Core/
    │   ├── Data/
    │   ├── GameRules/
    │   └── Utilities/
    ├── Managers/
    │   └── BootstrapManager.cs     ← ✓ Created
    ├── Networking/
    │   └── PhotonConnector.cs      ← ✓ Created
    ├── Player/
    │   └── CharacterAbilities/
    └── UI/
        ├── BootstrapUI.cs          ← ✓ Created
        ├── Game/
        ├── Lobby/
        └── MainMenu/
```

---

### 2. Photon PUN 2 Setup ✓

**Status**: Installed and configured  
**Version**: PUN 2 FREE  
**Source**: Unity Asset Store / Photon Website  

**Configuration**:
- App ID configured in Unity (Window > Photon Unity Networking > Highlight Server Settings)
- Server Settings location: `Assets/Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset`
- Auto-Join Lobby: Disabled (manual join)
- AutomaticallySyncScene: True (set in code)
- Game Version: "1.0"

**Test Results**: ✓ Connection successful  
**Region**: Auto-select (best region)

---

### 3. Scripts Created

#### A. `BootstrapManager.cs`
**Location**: `Assets/Scripts/Managers/BootstrapManager.cs`  
**Namespace**: `LastMansStash`

**Purpose**: First scene manager - initializes Photon and transitions to Main Menu

**Key Features**:
- Singleton pattern
- Minimum load time: 1.5 seconds
- Status updates via BootstrapUI
- Scene transition to "MainMenu"

**Dependencies**:
- `PhotonConnector.cs` (must exist in same scene)
- `BootstrapUI.cs` (optional, for UI updates)

**Inspector Fields**:
- `mainMenuSceneName`: "MainMenu" (string)
- `minimumLoadTime`: 1.5f (float)
- `bootstrapUI`: Reference to BootstrapUI component

**Important Methods**:
- `Start()`: Initiates Photon connection
- `OnPhotonReady()`: Called by PhotonConnector when ready
- `UpdateStatus(string)`: Updates UI status text

---

#### B. `PhotonConnector.cs`
**Location**: `Assets/Scripts/Networking/PhotonConnector.cs`  
**Namespace**: `LastMansStash.Networking`

**Purpose**: Manages Photon connection lifecycle

**Key Features**:
- Singleton pattern with DontDestroyOnLoad
- Inherits from MonoBehaviourPunCallbacks
- Auto-joins lobby after connecting to master server

**Inspector Fields**:
- `gameVersion`: "1.0" (string)

**Important Settings** (Set in Awake):
```csharp
PhotonNetwork.AutomaticallySyncScene = true;
PhotonNetwork.GameVersion = gameVersion;
```

**Callbacks Implemented**:
- `OnConnectedToMaster()`: Joins lobby
- `OnJoinedLobby()`: Notifies BootstrapManager
- `OnDisconnected(DisconnectCause)`: Logs warning
- `OnLeftLobby()`: Logs info

**Connection Flow**:
1. Bootstrap calls `Connect()`
2. PUN connects to Photon servers
3. `OnConnectedToMaster()` fires
4. Auto-join lobby
5. `OnJoinedLobby()` fires
6. Notifies `BootstrapManager.Instance.OnPhotonReady()`

---

#### C. `BootstrapUI.cs`
**Location**: `Assets/Scripts/UI/BootstrapUI.cs`  
**Namespace**: `LastMansStash.UI`

**Purpose**: Loading screen UI for Bootstrap scene

**Key Features**:
- Rotating spinner animation
- Status text updates

**Inspector Fields**:
- `statusText`: TextMeshProUGUI reference
- `loadingSpinner`: Image reference
- `spinSpeed`: 180f (degrees/second)

**Update Loop**: Rotates spinner continuously

**Public Methods**:
- `SetStatus(string status)`: Updates status text

---

### 4. Bootstrap Scene Setup

**File**: `Assets/Scenes/Bootstrap.unity`

**Hierarchy Structure**:
```
Bootstrap Scene
├── _Managers
│   ├── Transform
│   ├── BootstrapManager
│   └── PhotonConnector
└── LoadingUI (Canvas)
    ├── Canvas (Screen Space - Overlay, Pixel Perfect)
    ├── CanvasScaler
    ├── GraphicRaycaster
    ├── BootstrapUI
    ├── Background (Panel - Black, full screen)
    ├── StatusText (TextMeshProUGUI)
    │   ├── Text: "Initializing..."
    │   ├── Font: Special Elite, 36pt
    │   ├── Alignment: Center/Center
    │   ├── Color: White
    │   └── Rect: Center anchor, Pos(0, -50), Size(600, 100)
    └── LoadingSpinner (Image)
        ├── Color: White
        ├── Rect: Center anchor, Pos(0, 50), Size(80, 80)
        └── (Rotates via BootstrapUI script)
```

**Component Connections**:
1. `_Managers` GameObject:
   - BootstrapManager.bootstrapUI → `LoadingUI`
   - PhotonConnector (no references needed)

2. `LoadingUI` GameObject:
   - BootstrapUI.statusText → `StatusText`
   - BootstrapUI.loadingSpinner → `LoadingSpinner`

**Build Settings**:
- Bootstrap.unity is at index 0 (first scene to load)

---

### 5. Typography System

**Fonts Installed**: 
- Bebas Neue (TTF/OTF in `Assets/Fonts/`)
- Archivo Black (TTF/OTF in `Assets/Fonts/`)
- Special Elite (TTF/OTF in `Assets/Fonts/`)

**Usage Rules** (CRITICAL - Must follow for consistency):

| Font | Usage | Typical Sizes | Style |
|------|-------|---------------|-------|
| **Bebas Neue** | Titles, scene headers, player names, big numbers, game logo | 48-72pt | Uppercase, Bold |
| **Archivo Black** | Section headers, buttons, timers, money displays, character names | 24-48pt | Mixed or Uppercase |
| **Special Elite** | Body text, descriptions, status messages, labels, chat, flavor text | 14-36pt | Sentence case, Typewriter feel |

**Current Implementation**:
- Bootstrap StatusText: **Special Elite, 36pt**

**Future UI Font Matrix** (documented in TODO.md lines 66-82):
- Main Menu Title: Bebas Neue, 72pt, uppercase
- Buttons: Archivo Black, 24-32pt
- Room Code: Archivo Black, 36pt
- Player Names: Bebas Neue, 20-24pt
- Descriptions: Special Elite, 14-18pt
- Money/Stats: Archivo Black, 18-36pt

---

## 🎮 Design Decisions & Standards

### Networking Architecture

**Host Authority Pattern**:
- Host (room master) is authoritative for all game state
- Prevents cheating
- All actions validated by host

**State Sync Strategy**:
- **Photon Custom Properties**: Persistent state (player money, cards, status)
- **RPCs**: Actions and events (play card, move, trigger effect)
- **PhotonTransformView**: Player positions
- **PhotonView**: All networked GameObjects

**Networked Prefab Requirements**:
1. Must be in `Resources/PhotonPrefabs/` folder
2. Must have PhotonView component
3. Must be registered in Photon settings

---

### Code Standards

**Namespaces**:
- Root: `LastMansStash`
- Networking: `LastMansStash.Networking`
- UI: `LastMansStash.UI`
- Cards: `LastMansStash.Cards` (not yet created)
- Board: `LastMansStash.Board` (not yet created)

**Singleton Pattern** (for managers):
```csharp
public static ClassName Instance { get; private set; }

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // If needed
    }
    else
    {
        Destroy(gameObject);
        return;
    }
}
```

**Used By**:
- BootstrapManager (no DontDestroyOnLoad - scene-specific)
- PhotonConnector (WITH DontDestroyOnLoad - persists across scenes)

**Inspector Attributes**:
```csharp
[Header("Settings")]
[SerializeField] private Type fieldName;
```

**Logging Convention**:
- Use Debug.Log for info
- Use Debug.LogWarning for warnings
- Use Debug.LogError for errors
- Prefix custom logs: `Debug.Log("[Bootstrap] Message")`

---

### Scene Management

**Scene Flow**:
1. **Bootstrap** → Connects to Photon → Loads Main Menu
2. **Main Menu** → Create/Join Room → Loads Lobby
3. **Lobby** → Character Draft → Start Game → Loads Game
4. **Game** → Play → End Game → Loads Game Over
5. **Game Over** → Rematch/Lobby/Main Menu

**Scene Loading**:
- Use `SceneManager.LoadScene()` for non-networked transitions (Bootstrap → Main Menu)
- Use `PhotonNetwork.LoadLevel()` for networked transitions (Lobby → Game)
- `PhotonNetwork.AutomaticallySyncScene = true` ensures all clients load same scene

---

## 📋 Reference Documents

### Primary Documents (Use These):
1. **GAME_DESIGN_DOCUMENT.md** - Single source of truth for all game mechanics
2. **TODO.md** - Checklist of all tasks (300+ tasks, includes font specifications)
3. **PROJECT_STATE_REVIEW.md** - This document (state snapshot)

### Setup Guides:
1. **PHOTON_SETUP.md** - How to install and configure Photon PUN 2
2. **BOOTSTRAP_SCENE_SETUP.md** - How to create Bootstrap scene
3. **BOOTSTRAP_UI_SETUP.md** - How to create loading UI

### Deprecated (Do Not Use):
1. **implementation_plan.md** (in `.gemini/` artifacts) - Old plan from before project restart

---

## 🔍 Critical Information for Future Sessions

### 1. CharacterProfile ScriptableObjects
**Location**: `Assets/ScriptableObjects/`  
**Existing Assets** (from before restart - reusable):
- Hacker.asset
- Grifter.asset
- Runner.asset
- Insider.asset
- Thug.asset

**Status**: These exist but need corresponding `CharacterProfile.cs` script (Phase 2)

---

### 2. Deleted Files (Fresh Start)
The following were deleted before Phase 1:
- Old `Player_prefab.prefab` (had missing script references)
- Old `SampleScene.unity` (default Unity scene)
- Old `Tile_Mat.mat` (old material)
- All old C# scripts from previous implementation

**Current State**: Clean slate, no legacy code conflicts

---

### 3. Phase 1 Test Status

**What Works**:
- ✅ Photon connection
- ✅ Loading UI displays
- ✅ Status text updates
- ✅ Spinner animation
- ✅ Font styling (Special Elite, 36pt)

**Expected Error** (Normal):
```
Scene 'MainMenu' couldn't be loaded because it has not been added to the build settings
```
This is expected - MainMenu scene doesn't exist yet.

**To Test Fully**: Create MainMenu scene in Phase 4

---

## 📊 Project Statistics

**Phase 1 Completion**: 100%  
**Total Phases**: 15  
**Overall Completion**: ~7% (Phase 1 of 15)

**Files Created**: 3 scripts, 1 scene  
**Folders Created**: 35+  
**External Assets**: Photon PUN 2, 3 fonts

**Tasks Completed**: ~45 / 300+  
**Next Phase**: Phase 2 (Core Data Structures)

---

## 🚀 Next Steps (Phase 2 Preview)

When resuming work, Phase 2 will create:

1. **GameEnums.cs** - All enums (TileType, PlayerStatus, CardType, etc.)
2. **GameConstants.cs** - Constants (starting money, max hand size, etc.)
3. **Tile.cs** - Tile data class
4. **TileIdentifier.cs** - Component for marking tiles
5. **PlayerData.cs** - Player state data
6. **CardBase.cs** - Base card class
7. **CharacterProfile.cs** - ScriptableObject for characters

**No Unity scene work** - Just C# data structures

---

## ⚠️ Important Notes for Consistency

### 1. File Naming
- Scripts: PascalCase (e.g., `BootstrapManager.cs`)
- Scenes: PascalCase (e.g., `Bootstrap.unity`)
- Prefabs: PascalCase with underscores (e.g., `Tile_Start.prefab`)
- ScriptableObjects: PascalCase (e.g., `Hacker.asset`)

### 2. Namespace Usage
Always use appropriate namespaces:
```csharp
namespace LastMansStash
{
    // Core game code
}

namespace LastMansStash.Networking
{
    // Network-specific code
}

namespace LastMansStash.UI
{
    // UI-specific code
}
```

### 3. Unity Version Compatibility
- TextMeshPro is used (not legacy Text)
- Import TMP Essentials when prompted
- Photon PUN 2 (not PUN Classic or Fusion)

### 4. Font Asset Creation
For each font, must create TMP Font Asset:
1. Window → TextMeshPro → Font Asset Creator
2. Select font file
3. Generate Atlas
4. Use in TextMeshProUGUI components

---

## 🔗 Dependencies & Packages

### Required:
- **Photon PUN 2 FREE** (Unity Asset Store)
- **TextMesh Pro** (Unity Package Manager - usually included)

### Optional:
- Input System (for future player controls)

---

## 📝 Uncommitted Changes (If Using Git)

If using version control, the following should be committed:

**Scenes**:
- `Assets/Scenes/Bootstrap.unity`
- `Assets/Scenes/Bootstrap.unity.meta`

**Scripts**:
- `Assets/Scripts/Managers/BootstrapManager.cs`
- `Assets/Scripts/Networking/PhotonConnector.cs`
- `Assets/Scripts/UI/BootstrapUI.cs`
- All `.meta` files for above

**Documentation**:
- `GAME_DESIGN_DOCUMENT.md`
- `TODO.md`
- `PROJECT_STATE_REVIEW.md`
- `PHOTON_SETUP.md`
- `BOOTSTRAP_SCENE_SETUP.md`
- `BOOTSTRAP_UI_SETUP.md`

**Photon Settings**:
- `Assets/Photon/` (entire folder)

**Fonts**:
- `Assets/Fonts/` (if added to repo)

**ScriptableObjects**:
- `Assets/ScriptableObjects/` (existing character assets)

---

## 🎯 Quality Checklist

Before proceeding to Phase 2, verify:

- [ ] Bootstrap scene plays without errors (except MainMenu not found)
- [ ] Photon connection shows "Connected to Photon Master Server" in Console
- [ ] Loading UI displays with correct font (Special Elite, 36pt)
- [ ] Spinner rotates smoothly
- [ ] Status text updates: "Initializing..." → "Connecting..." → "Connected!"
- [ ] All 35+ folders exist in correct structure
- [ ] No missing script references in scene
- [ ] Photon App ID is configured

---

## 🔐 Secrets & Configuration

**Photon App ID**: Stored in `PhotonServerSettings.asset` (do NOT commit to public repos)  
**Alternative**: Use environment variable or config file not in source control

---

## 📖 Code Snippets for Reference

### Connecting to Photon (Already Implemented)
```csharp
PhotonNetwork.AutomaticallySyncScene = true;
PhotonNetwork.GameVersion = "1.0";
PhotonNetwork.ConnectUsingSettings();
```

### Loading Scene (Bootstrap → Main Menu)
```csharp
SceneManager.LoadScene("MainMenu");
```

### Loading Scene (Networked - for later phases)
```csharp
PhotonNetwork.LoadLevel("Game");
```

---

## 🎨 UI Color Palette (To Be Established)

**Not yet defined** - Will be established when creating Main Menu UI.

**Theme**: Heist/Crime/Noir  
**Suggested Direction**: 
- Dark backgrounds (blacks, dark grays)
- Accent colors (gold for money, red for danger, green for safe)
- High contrast for readability

---

## 📞 Support & Troubleshooting

### Common Issues:

**1. "PhotonConnector not found!"**
- **Solution**: Make sure `_Managers` GameObject has both BootstrapManager AND PhotonConnector components

**2. "Scene 'MainMenu' couldn't be loaded"**
- **Solution**: Normal for Phase 1 - MainMenu scene will be created in Phase 4

**3. Spinner not rotating**
- **Solution**: Check BootstrapUI.loadingSpinner reference is assigned in Inspector

**4. Font not displaying correctly**
- **Solution**: Ensure TMP Font Asset was created from the font file

---

## ✅ Phase 1 Sign-Off

**Status**: COMPLETE  
**Quality**: Production-ready  
**Technical Debt**: None  
**Blockers**: None  

**Ready for Phase 2**: ✅ YES

---

**End of Project State Review**
