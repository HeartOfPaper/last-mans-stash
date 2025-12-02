# Phase 3 Testing Guide - Board System

**Purpose**: Test the BoardManager and tile system in Unity

---

## Prerequisites

Before testing, ensure:
- ✅ All Phase 3 scripts are in `Assets/Scripts/` folders
- ✅ Unity project is open
- ✅ You're on the `phase-3-board-system` branch

---

## Step 1: Create Test Scene

1. In Unity, create a new scene: **File → New Scene → Basic (Built-in)**
2. Save as: `Assets/Scenes/BoardTest.unity`
3. This is just for testing - won't be committed to the final game

---

## Step 2: Create Board Center

1. Create an empty GameObject: **GameObject → Create Empty**
2. Rename to `BoardCenter`
3. Position at `(0, 0, 0)`
4. This will be the visual center of your board

---

## Step 3: Create Placeholder Tiles

Create 9 simple tiles around the center (one of each type):

### Method: Use Cubes as Placeholders

1. **Create Start Tile**:
   - Create: **GameObject → 3D Object → Cube**
   - Rename to: `Tile_Start`
   - Position: `(0, 0, 6)` (North of center)
   - Add Component: `TileIdentifier`
   - Set TileType: `Start`
   - Add Component: `StartTile`
   - Change cube color to green (create material if needed)

2. **Create Blank Tiles** (x2):
   - Cube at `(4.2, 0, 4.2)` - Northeast
   - Cube at `(6, 0, 0)` - East
   - Each: Add `TileIdentifier` (TileType: Blank) + `BlankTile` script

3. **Create Casino Tile**:
   - Cube at `(4.2, 0, -4.2)` - Southeast
   - Add `TileIdentifier` (TileType: Casino) + `CasinoTile` script
   - Color: Red/Gold

4. **Create Safehouse Tile**:
   - Cube at `(0, 0, -6)` - South
   - Add `TileIdentifier` (TileType: Safehouse) + `SafehouseTile` script
   - Color: Blue

5. **Create Vault Tile**:
   - Cube at `(-4.2, 0, -4.2)` - Southwest
   - Add `TileIdentifier` (TileType: Vault) + `VaultTile` script
   - Color: Yellow

6. **Create Pawn Shop Tile**:
   - Cube at `(-6, 0, 0)` - West
   - Add `TileIdentifier` (TileType: PawnShop) + `PawnShopTile` script
   - Color: Purple

7. **Create Payphone Tile**:
   - Cube at `(-4.2, 0, 4.2)` - Northwest
   - Add `TileIdentifier` (TileType: Payphone) + `PayphoneTile` script
   - Color: Orange

**Quick Layout** (bird's eye view - 9 tiles):
```
           Start
            [G]
              
Payphone  [O]   [B] Blank
  (NW)            (NE)

PawnShop [P]      [B] Blank
  (W)              (E)

           [R/G]   [R] Casino
Vault(SW)  [Y]      (SE)
    
         Safehouse
            [B]
            (S)
```

---

## Step 4: Add BoardManager

1. Create empty GameObject: **GameObject → Create Empty**
2. Rename to: `_BoardManager`
3. Add Component: `BoardManager`
4. In Inspector:
   - ✅ Auto Find Tiles: **TRUE**
   - ✅ Show Debug Gizmos: **TRUE**

---

## Step 5: Test - Scene View

1. **Select** `_BoardManager` in Hierarchy
2. In **Scene view**, you should see:
   - 🟡 Yellow sphere at board center
   - 🔵 Cyan lines connecting tiles in order
   - 🟢 Green ring around Start tile
3. **Select** `_BoardManager` and look at **Inspector**:
   - Board Center should show calculated position
   - Total Tile Count should be 9
   - Board Tiles list should show 9 tiles in order

---

## Step 6: Test - Console View

1. **Enter Play Mode** (press Play button)
2. Check **Console** window:
   - Should see: `[BoardManager] Finding tiles in scene...`
   - Should see: `[BoardManager] Found 9 tiles`
   - Should see: `[BoardManager] Board initialized with 9 tiles`
   - Should see tile order: `Start → Blank → Blank → Casino → Safehouse → Vault → PawnShop → Payphone`

3. **Verify Order**: Tiles should be listed **clockwise** from Start

---

## Step 7: Test Tile Ordering

**Expected Order** (clockwise from Start at North):
```
0: Start (North)
1: Blank (Northeast)
2: Blank (East)
3: Casino (Southeast)
4: Safehouse (South)
5: Vault (Southwest)
6: PawnShop (West)
7: Payphone (Northwest)
```

**If order is wrong**:
- Check tile positions (they should form a rough circle)
- Make sure Start tile is correctly identified
- BoardManager uses angle-based sorting from center

---

## Step 8: Test Gizmo Labels

1. In Scene view, **select** `_BoardManager`
2. Labels should appear above each tile showing:
   - `[0] Start`
   - `[1] Blank`
   - `[2] Blank`
   - etc.

This confirms the indexing is correct!

---

## Step 9: Test BoardManager Methods

Add this test script to verify BoardManager API:

**Create**: `Assets/Scripts/BoardManagerTest.cs`

```csharp
using UnityEngine;
using LastMansStash.Managers;
using static LastMansStash.Core.GameEnums;

public class BoardManagerTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== BoardManager Test ===");
        
        // Test GetTileAtIndex
        var tile0 = BoardManager.Instance.GetTileAtIndex(0);
        Debug.Log($"Tile 0: {tile0.TileType}");
        
        // Test GetTileByType
        var startTile = BoardManager.Instance.GetTileByType(TileType.Start);
        Debug.Log($"Start Tile Index: {startTile.Index}");
        
        // Test GetTilesByType
        var blankTiles = BoardManager.Instance.GetTilesByType(TileType.Blank);
        Debug.Log($"Blank Tile Count: {blankTiles.Count}");
        
        Debug.Log("=== Test Complete ===");
    }
}
```

**Attach** this script to `_BoardManager` GameObject and run again.

**Expected Output**:
```
=== BoardManager Test ===
Tile 0: Start
Start Tile Index: 0
Blank Tile Count: 2
=== Test Complete ===
```

---

## ✅ Success Criteria

Phase 3 is working if:

1. ✅ BoardManager finds all 9 tiles
2. ✅ Tiles are ordered clockwise from Start
3. ✅ Gizmos display correctly (cyan lines, green ring)
4. ✅ Console shows correct tile order
5. ✅ GetTileAtIndex() returns correct tiles
6. ✅ GetTileByType() finds correct tiles

---

## 🐛 Troubleshooting

### "No tiles found in scene"
- **Fix**: Make sure each tile GameObject has `TileIdentifier` component

### "No Start tile found"
- **Fix**: One tile must have `TileType` set to `Start` in TileIdentifier

### Wrong tile order
- **Fix**: Arrange tiles in a rough circle around (0,0,0)
- Tiles should be evenly spaced in a ring formation

### Gizmos not showing
- **Fix**: Make sure "Show Debug Gizmos" is checked in BoardManager
- Enable Gizmos in Scene view (top right, Gizmos button)

---

## 🔄 Next Steps

After testing is successful:

1. **Delete test scene** (BoardTest.unity)
2. **Delete test script** (BoardManagerTest.cs)
3. **Let me (the AI) know it works** - I'll commit the final changes and help you merge Phase 3

---

## 📊 Phase 3 Stats

**Files Created**: 9 C# scripts (637 lines total)
- BoardManager.cs (252 lines)
- TileBase.cs (62 lines) 
- 7 Tile implementations (323 lines)

**Features**:
- Spatial tile ordering algorithm
- Visual debugging with gizmos
- Character ability support in tiles
- Placeholders for future phases

**Ready for**: Phase 4 (Main Menu & Networking)

---

**Questions?** Check console logs or review BoardManager.cs comments!
