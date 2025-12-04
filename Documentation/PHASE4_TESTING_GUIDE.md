# Phase 4 Testing Guide - Main Menu & Lobby Setup

## Overview
This guide will help you set up and test the Main Menu and Lobby scenes in Unity before committing.

---

## 🎯 Prerequisites

**Before starting**:
- ✅ All Phase 4 scripts are in `Assets/Scripts/`
- ✅ Unity is open with the project loaded
- ✅ Photon PUN 2 is configured (from Phase 1)

---

## Part 1: Main Menu Scene Setup

### Step 1: Create Main Menu Scene

1. **Create new scene**: File → New Scene → Empty Scene
2. **Save scene**: `Assets/Scenes/MainMenu.unity`
3. **Add to build settings**: File → Build Settings → Add Open Scenes

### Step 2: Create GameNetworkManager GameObject

1. **Create empty GameObject**: Right-click Hierarchy → Create Empty
2. **Rename**: `GameNetworkManager`
3. **Add components**:
   - Add Component → `GameNetworkManager` (our script)
   - The script inherits from `MonoBehaviourPunCallbacks` so it will handle Photon callbacks
4. **Mark as DontDestroyOnLoad** (optional): Already handled in script

### Step 3: Create RoomManager GameObject

1. **Create empty GameObject**: Right-click Hierarchy → Create Empty
2. **Rename**: `RoomManager`
3. **Add component**: Add Component → `RoomManager` (our script)

### Step 4: Create SettingsManager GameObject

1. **Create empty GameObject**: Right-click Hierarchy → Create Empty
2. **Rename**: `SettingsManager`
3. **Add component**: Add Component → `SettingsManager` (our script)
4. **Mark as DontDestroyOnLoad** (optional): Already handled in script

### Step 5: Create Main Menu UI

#### 5.1 Create Canvas

1. **Create Canvas**: Right-click Hierarchy → UI → Canvas
2. **Rename**: `Main Menu Canvas`
3. **Canvas settings**:
   - Render Mode: Screen Space - Overlay
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080

#### 5.2 Create Main Menu Panel

1. **Create Panel**: Right-click Canvas → UI → Panel
2. **Rename**: `MainMenuPanel`
3. **Add background** (optional): Set color or add background image

#### 5.3 Add Title Text

1. **Create Text**: Right-click MainMenuPanel → UI → Text - TextMeshPro
2. **Rename**: `TitleText`
3. **Set text**: "LAST MAN'S STASH"
4. **Font**: Bebas Neue, Size: 72, Uppercase
5. **Position**: Top center of screen
6. **Color**: Your choice (e.g., white or gold)

#### 5.4 Add Create Room Button

1. **Create Button**: Right-click MainMenuPanel → UI → Button - TextMeshPro
2. **Rename**: `CreateRoomButton`
3. **Button Text**: "CREATE ROOM"
4. **Font**: Archivo Black, Size: 28, Uppercase
5. **Position**: Center-left area

#### 5.5 Add Join Room Section

1. **Create Input Field**: Right-click MainMenuPanel → UI → Input Field - TextMeshPro
2. **Rename**: `RoomCodeInput`
3. **Settings**:
   - Placeholder Text: "ENTER CODE"
   - Font: Special Elite, Size: 20
   - Character Limit: 6
   - Content Type: Alphanumeric
4. **Position**: Center area

5. **Create Button**: Right-click MainMenuPanel → UI → Button - TextMeshPro
6. **Rename**: `JoinRoomButton`
7. **Button Text**: "JOIN ROOM"
8. **Font**: Archivo Black, Size: 28, Uppercase
9. **Position**: Next to input field

#### 5.6 Add Quick Match Button

1. **Create Button**: Right-click MainMenuPanel → UI → Button - TextMeshPro
2. **Rename**: `QuickMatchButton`
3. **Button Text**: "QUICK MATCH"
4. **Font**: Archivo Black, Size: 28, Uppercase
5. **Position**: Center-right area

#### 5.7 Add Settings & Quit Buttons

1. **Settings Button**:
   - Create Button → Rename: `SettingsButton`
   - Text: "SETTINGS"
   - Font: Archivo Black, Size: 24
   - Position: Bottom-left

2. **Quit Button**:
   - Create Button → Rename: `QuitButton`
   - Text: "QUIT"
   - Font: Archivo Black, Size: 24
   - Position: Bottom-right

#### 5.8 Create Settings Panel (Hidden by Default)

1. **Create Panel**: Right-click Canvas → UI → Panel
2. **Rename**: `SettingsPanel`
3. **Disable** (uncheck checkbox in Inspector)
4. **Add Title Text**:
   - Right-click SettingsPanel → UI → Text - TextMeshPro
   - Rename: `SettingsTitleText`
   - Text: "SETTINGS"
   - Font: Bebas Neue, Size: 48
   - Position: Top-center of panel
5. **Add Back Button**:
   - Right-click SettingsPanel → UI → Button - TextMeshPro
   - Rename: `BackButton`
   - Text: "BACK"
   - Font: Archivo Black, Size: 24
   - Position: Bottom-left or bottom-center of panel
6. **Add SettingsUI Component**:
   - Select `SettingsPanel`
   - Add Component → `SettingsUI`
   - In Inspector, assign:
     - Back Button: Drag `BackButton`
   - Note: Other settings fields can be left empty for now (full settings in future phase)

*Note: Full Settings UI with tabs will be implemented in Phase 13 - for now just basic back functionality*

#### 5.9 Create Loading Panel (Hidden by Default)

1. **Create Panel**: Right-click Canvas → UI → Panel
2. **Rename**: `LoadingPanel`
3. **Disable** (uncheck checkbox in Inspector)
4. **Add Loading Text**:
   - Create Text → Rename: `LoadingText`
   - Text: "Loading..."
   - Center position

#### 5.10 Create Error Panel (Hidden by Default)

1. **Create Panel**: Right-click Canvas → UI → Panel
2. **Rename**: `ErrorPanel`
3. **Disable** (uncheck checkbox in Inspector)
4. **Add Error Text**:
   - Create Text → Rename: `ErrorText`
   - Text: "Error message"
5. **Add Close Button**:
   - Create Button → Rename: `ErrorCloseButton`
   - **Important**: Find the Text child of this button (expand button in Hierarchy)
   - The button has a Text (TMP) child - this is `ErrorCloseButtonText`
   - Default text: "OK" (will change to "RETRY" for connection errors dynamically)

### Step 6: Wire Up MainMenuManager

1. **Create empty GameObject**: Right-click Hierarchy → Create Empty
2. **Rename**: `MainMenuManager`
3. **Add component**: Add Component → `MainMenuManager`
4. **Assign references in Inspector**:
   - Room Manager: Drag `RoomManager` GameObject
   - Main Menu UI: Will auto-find

### Step 7: Wire Up MainMenuUI

1. **Select Canvas**: Click `Main Menu Canvas` in Hierarchy
2. **Add component**: Add Component → `MainMenuUI`
3. **Assign references in Inspector**:
   - Main Menu Panel: Drag `MainMenuPanel`
   - Settings Panel: Drag `SettingsPanel`
   - Loading Panel: Drag `LoadingPanel`
   - **Buttons**:
     - Create Room Button: Drag `CreateRoomButton`
     - Join Room Button: Drag `JoinRoomButton`
     - Quick Match Button: Drag `QuickMatchButton`
     - Settings Button: Drag `SettingsButton`
     - Quit Button: Drag `QuitButton`
   - **Input**: Room Code Input: Drag `RoomCodeInput`
   - **Loading**: Loading Text: Drag `LoadingText`
   - **Error**: 
     - Error Panel: Drag `ErrorPanel`
     - Error Text: Drag `ErrorText`
     - Error Close Button: Drag `ErrorCloseButton`
     - Error Close Button Text: Drag the **Text (TMP)** child of `ErrorCloseButton`

### Step 8: Update Bootstrap Scene

1. **Open Bootstrap scene**: `Assets/Scenes/Bootstrap.unity`
2. **Find BootstrapManager** in Hierarchy
3. **Update transition** (if needed):
   - Currently loads "MainMenu" scene after connection
   - This should already be set from Phase 1

---

## Part 2: Lobby Scene Setup

### Step 1: Create Lobby Scene

1. **Create new scene**: File → New Scene → Empty Scene
2. **Save scene**: `Assets/Scenes/Lobby.unity`
3. **Add to build settings**: File → Build Settings → Add Open Scenes

### Step 2: Create Lobby UI Canvas

1. **Create Canvas**: Right-click Hierarchy → UI → Canvas
2. **Rename**: `Lobby Canvas`
3. **Canvas settings**: Same as Main Menu (Scale With Screen Size, 1920x1080)

### Step 3: Create Room Code Display

1. **Create Panel**: Right-click Canvas → UI → Panel
2. **Rename**: `RoomCodePanel`
3. **Position & Size**:
   - **Anchor**: Top-center (Shift+Alt+Click top-center preset)
   - **Rect Transform**:
     - **Pos X**: 0 (centered)
     - **Pos Y**: -50 to -80 (offset from top edge)
     - **Width**: 400-500 pixels
     - **Height**: 80-120 pixels

4. **Add Code Text**:
   - Create Text → Rename: `RoomCodeText`
   - Font: Archivo Black, Size: 36, Bold
   - Text: "ABC 123"
   - Center position

5. **Add Copy Button** (Optional):
   - Create Button → Rename: `CopyButton`
   - Text: "📋 COPY"
   - Position: Next to code

6. **Add RoomCodeDisplay component**:
   - Select `RoomCodePanel`
   - Add Component → `RoomCodeDisplay`
   - Assign:
     - Code Text: Drag `RoomCodeText`
     - Copy Button: Drag `CopyButton` (if created)
     - Use Spacing: Check (for "ABC 123" format)

### Step 4: Create Player List

1. **Create Panel**: Right-click Canvas → UI → Panel
2. **Rename**: `PlayerListPanel`
3. **Position & Size**:
   - **Anchor**: Left-center (Shift+Alt+Click middle-left preset)
   - **Rect Transform**:
     - **Pos X**: 50-80 (offset from left edge)
     - **Pos Y**: 0 (centered vertically)
     - **Width**: 400-500 pixels
     - **Height**: 450-500 pixels (fits 5 slots + spacing)

4. **Add Vertical Layout Group**:
   - Select `PlayerListPanel`
   - Add Component → Vertical Layout Group
   - Settings:
     - Child Alignment: Upper Center
     - Spacing: 10
     - Child Force Expand: Width = true, Height = false
     - **Control Child Size**: Width = true, Height = false
   - **⚠️ IMPORTANT**: Only player slot prefabs should be children of this panel (added dynamically by code)

5. **Create Player Slot Prefab**:
   - **Right-click PlayerListPanel** → UI → Panel
   - **Rename**: `PlayerSlot`
   - **Rect Transform**:
     - Width: **400 pixels**
     - Height: **75 pixels**
   - **Add Text child**: Right-click PlayerSlot → UI → Text - TextMeshPro
   - **Rename text**: `PlayerNameText`
   - **Text settings**:
     - Font: Bebas Neue, Size: 24
     - Alignment: Middle-Left
     - **Anchor**: Stretch (left-right, top-bottom)
     - **Margins**: Left: 15, Right: 15, Top: 10, Bottom: 10
   - **Save as prefab**: 
     - Create folder: `Assets/Prefabs/` (if doesn't exist)
     - Drag `PlayerSlot` from Hierarchy to `Assets/Prefabs/` folder
   - **Delete from Hierarchy** (LobbyUI will create them dynamically)


### Step 5: Create Bottom Panel (Player Count + Action Buttons)

**Purpose**: Groups player count and buttons in one organized container at the bottom of the screen.

1. **Create Bottom Panel**:
   - **Right-click Canvas** → UI → Panel
   - **Rename**: `BottomPanel`
   - **Rect Transform**:
     - **Anchor**: Bottom-stretch (bottom row, middle preset - stretches full width)
     - **Pivot**: X = 0.5, Y = 0
     - **Pos Y**: 0 (sits at bottom edge)
     - **Height**: 80-100 pixels
     - **Left**: 0, **Right**: 0 (full width)
   - **Background** (optional): Semi-transparent dark color for contrast

2. **Add Horizontal Layout Group**:
   - Select `BottomPanel`
   - Add Component → Horizontal Layout Group
   - **Settings**:
     - Child Alignment: **Middle-Center**
     - Spacing: **30-40** (space between elements)
     - Child Force Expand: Width = **false**, Height = **false**
     - Child Control Size: Width = **false**, Height = **false**
     - Padding: Left = **50**, Right = **50**, Top = **15**, Bottom = **15**

3. **Add Player Count Text**:
   - **Right-click BottomPanel** (make it a child) → UI → Text - TextMeshPro
   - **Rename**: `PlayerCountText`
   - **Text settings**:
     - Text: "0/5 Players"
     - Font: Special Elite, Size: 18
     - Alignment: Center
     - Color: White or light gray
   - **Rect Transform**:
     - Width: **150-200 pixels**
     - Height: **40 pixels**
   - The Horizontal Layout Group will position it automatically on the left

4. **Add Start Game Button** (Host only):
   - **Right-click BottomPanel** → UI → Button - TextMeshPro
   - **Rename**: `StartGameButton`
   - **Button settings**:
     - Text: "START GAME"
     - Font: Archivo Black, Size: 28, Uppercase
     - **Disable by default** (uncheck in Inspector - only host will see it)
   - **Rect Transform**:
     - Width: **250-300 pixels**
     - Height: **60 pixels**
   - The Horizontal Layout Group will center it

5. **Add Leave Room Button**:
   - **Right-click BottomPanel** → UI → Button - TextMeshPro
   - **Rename**: `LeaveRoomButton`
   - **Button settings**:
     - Text: "LEAVE ROOM"
     - Font: Archivo Black, Size: 24
     - Color: Red or warning color (to indicate destructive action)
   - **Rect Transform**:
     - Width: **200-250 pixels**
     - Height: **50-60 pixels**
   - The Horizontal Layout Group will position it on the right

**Visual Result**:
```
┌────────────────────────────────────────┐
│   Room Code: ABC 123                   │ ← Top
│                                        │
│  [Player List]                         │ ← Left
│                                        │
├────────────────────────────────────────┤
│  3/5 Players  [START GAME] [LEAVE ROOM]│ ← Bottom Panel
└────────────────────────────────────────┘
```


### Step 6: Create Loading Panel

1. **Create Panel**: Right-click Canvas → UI → Panel
2. **Rename**: `LoadingPanel`
3. **Disable** (uncheck checkbox)
4. **Add Loading Text**:
   - Create Text → Rename: `LoadingText`
   - Text: "Loading..."

### Step 7: Wire Up LobbyManager

1. **Create empty GameObject**: Right-click Hierarchy → Create Empty
2. **Rename**: `LobbyManager`
3. **Add component**: Add Component → `LobbyManager`
4. **Assign**: Lobby UI: Will auto-find

### Step 8: Wire Up LobbyUI

1. **Select Canvas**: Click `Lobby Canvas`
2. **Add component**: Add Component → `LobbyUI`
3. **Assign references in Inspector**:
   - Room Code Display: Drag `RoomCodePanel` (has RoomCodeDisplay component)
   - Player List Container: Drag `PlayerListPanel`
   - Player Slot Prefab: Drag prefab from `Assets/Prefabs/PlayerSlot`
   - Player Count Text: Drag `PlayerCountText`
   - Start Game Button: Drag `StartGameButton`
   - Leave Room Button: Drag `LeaveRoomButton`
   - Loading Panel: Drag `LoadingPanel`
   - Loading Text: Drag `LoadingText`

---

## 🧪 Testing

### Test 1: Main Menu - Create Room

1. **Play the game** (make sure Bootstrap scene is first in Build Settings)
2. **Wait for Bootstrap** to connect to Photon
3. **Main Menu should load**
4. **Click "CREATE ROOM"**
5. **Expected**:
   - Loading panel shows "Creating room..."
   - Room is created with 6-character code
   - Scene transitions to Lobby
   - Room code displays correctly (e.g., "ABC 123")
   - Your player appears in player list
   - You see "[HOST]" tag next to your name
   - Player count shows "1/5 Players"

### Test 2: Main Menu - Join Room

1. **Open a second instance** of Unity (or build the game)
2. **In second instance**:
   - Enter the 6-character room code from Test 1
   - Click "JOIN ROOM"
3. **Expected**:
   - Second player joins the room
   - Both players see each other in Lobby
   - Player count updates to "2/5 Players"
   - Only first player (host) sees "START GAME" button

### Test 3: Main Menu - Quick Match

1. **If rooms exist**: Joins random room
2. **If no rooms exist**: Creates new room
3. **Expected**: Either joins existing or creates new, then transitions to Lobby

### Test 4: Room Code Input Validation

1. **In Main Menu**, try entering room codes:
   - Type lowercase letters → **Should auto-capitalize**
   - Type special characters → **Should reject**
   - Type more than 6 characters → **Should limit to 6**
   - "JOIN ROOM" button → **Disabled until 6 valid characters entered**

### Test 5: Leave Room

1. **In Lobby**, click "LEAVE ROOM"
2. **Expected**:
   - Loading shown
   - Returns to Main Menu
   - Other players see you leave (player count updates)

### Test 6: Settings (Basic)

1. **In Main Menu**, click "SETTINGS"
2. **Expected**:
   - Settings panel shows
   - Main Menu hides
3. **Click "BACK"**
4. **Expected**: Returns to Main Menu

---

## ✅ Verification Checklist

Before committing, verify:

**Main Menu**:
- [ ] Bootstrap connects to Photon
- [ ] Main Menu loads after Bootstrap
- [ ] All buttons are visible and clickable
- [ ] Room code input validates correctly (6 chars, A-Z 0-9)
- [ ] Room code input auto-capitalizes
- [ ] Join button disabled until valid code entered

**Room Creation**:
- [ ] Create Room generates 6-character code
- [ ] Code uses only A-Z and 0-9
- [ ] Transitions to Lobby after creation
- [ ] Room code displays in Lobby

**Room Joining**:
- [ ] Can join room with valid code
- [ ] Invalid code shows error
- [ ] Multiple players can join same room
- [ ] Player list updates in real-time

**Lobby**:
- [ ] Room code displays correctly (with spacing: "ABC 123")
- [ ] Player list shows all joined players (1-5)
- [ ] Host is indicated with [HOST] tag
- [ ] Empty slots show "Waiting for player..."
- [ ] Player count is accurate (e.g., "3/5 Players")
- [ ] Start Game button only visible to host
- [ ] Leave Room returns to Main Menu

**Network Sync**:
- [ ] Two clients can join same room
- [ ] Both see each other in player list
- [ ] Player joining/leaving updates list on both clients
- [ ] Host status transfers if original host leaves

---

## 🐛 Common Issues & Fixes

**Issue**: Can't find scripts in Inspector  
**Fix**: Make sure all `.cs` files are compiled (check Console for errors)

**Issue**: Buttons don't respond  
**Fix**: Make sure you wired up button references in MainMenuUI component

**Issue**: Room code input doesn't validate  
**Fix**: Check that MainMenuUI is attached to Canvas and RoomCodeInput is assigned

**Issue**: Lobby shows no players  
**Fix**: Make sure NetworkManager instance exists and is connected

**Issue**: Can't join room  
**Fix**: Make sure both clients are connected to Photon (same region)

---

## 📝 Next Steps After Testing

Once everything works:
1. **Take screenshots** of Main Menu and Lobby (optional)
2. **Note any bugs** found during testing
3. **Let me know** - I'll fix bugs and then commit Phase 4!

---

**Happy Testing!** 🧪✨
