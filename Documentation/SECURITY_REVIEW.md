# Access Safety & Encapsulation Review

**Review Date**: 2025-12-02  
**Purpose**: Ensure no user/player can cheat by modifying game values

---

## 🔒 Critical Security Concerns

### ⚠️ PlayerData.cs - MAJOR SECURITY RISK

**Problem**: All modification methods are **public** and can be called by anyone:

```csharp
public void AddMoney(int amount)  // ❌ PUBLIC - can be exploited!
public void RemoveMoney(int amount)
public void AddMovementCard(int cardValue)
public void RemoveMovementCard(int cardValue)
// ... etc
```

**Risk**: In networked game, malicious client could call:
```csharp
playerData.AddMoney(9999);  // Instant cheat!
```

---

## ✅ SOLUTION: Host Authority Pattern

Since you're using **Photon PUN 2**, implement **Host Authority**:

### Approach 1: Internal Methods (Recommended)
Change all modification methods to `internal`:

```csharp
internal void AddMoney(int amount)  // ✅ Only accessible within assembly
internal void RemoveMoney(int amount)
internal void AddMovementCard(int cardValue)
```

**Then**: Only managers (GameManager, TileEffectManager) can modify player data.

### Approach 2: [PunRPC] Validation
Keep methods public but add RPC validation:

```csharp
public void AddMoney(int amount)
{
    // Only host can modify
    if (!PhotonNetwork.IsMasterClient)
    {
        Debug.LogError("Only host can modify player data!");
        return;
    }
    money += amount;
}
```

---

## 📋 File-by-File Security Audit

### ✅ SAFE FILES (No Changes Needed):

#### 1. **GameEnums.cs**
- **Status**: ✅ Safe
- **Reason**: Just enum definitions, no mutable state

#### 2. **GameConstants.cs**
- **Status**: ✅ Safe
- **Reason**: All `const` and `readonly`, cannot be modified at runtime

#### 3. **TileIdentifier.cs**
- **Status**: ✅ Safe
- **Reason**: Component only read by BoardManager, no player access

#### 4. **CardBase.cs + All Card Types**
- **Status**: ✅ Safe
- **Reason**: ScriptableObjects are **design-time only**, players can't modify

#### 5. **CharacterProfile.cs**
- **Status**: ✅ Safe
- **Reason**: ScriptableObject, read-only at runtime

#### 6. **BootstrapManager.cs**
- **Status**: ✅ Safe
- **Reason**: Singleton, no sensitive data

#### 7. **PhotonConnector.cs**
- **Status**: ✅ Safe
- **Reason**: Singleton, no exploitable methods

#### 8. **BootstrapUI.cs**
- **Status**: ✅ Safe
- **Reason**: UI component, no game logic

---

### ⚠️ NEEDS REVIEW:

#### **Tile.cs**
**Current**:
```csharp
public void SetVisualObject(GameObject visual) // ⚠️ Public setter
public void SetIndex(int newIndex)            // ⚠️ Public setter
```

**Risk**: Low (only used by BoardManager during setup)

**Recommendation**: Change to `internal`:
```csharp
internal void SetVisualObject(GameObject visual) // ✅
internal void SetIndex(int newIndex)            // ✅
```

---

#### **DaggerCard.cs**
**Current**:
```csharp
public void Flip()                    // ⚠️ Public
public void SetFace(bool bluffSide)   // ⚠️ Public
```

**Risk**: Medium (player could flip cards inappropriately)

**Recommendation**: Change to `internal` OR add validation:
```csharp
internal void Flip()  // ✅ Only managers can flip
```

---

### 🚨 CRITICAL - PlayerData.cs

**ALL modification methods must be protected!**

#### Methods That Need Protection:
```csharp
// Money (CRITICAL)
public void AddMoney(int amount)              // ❌
public void RemoveMoney(int amount)           // ❌

// Cards (CRITICAL)
public void AddMovementCard(int cardValue)    // ❌
public void RemoveMovementCard(int cardValue) // ❌
public void AddDaggerCard(int cardID)         // ❌
public void RemoveDaggerCard(int cardID)      // ❌

// Position (MEDIUM RISK)
public void SetTileIndex(int index)           // ❌
public void MoveTiles(int spaces)             // ❌

// Status (CRITICAL)
public void SetStatus(PlayerStatus newStatus) // ❌
public void Zombify()                         // ❌
public void BecomeSpectre()                   // ❌

// Last Resort (CRITICAL)
public void IncrementLastResortPurchaseCount() // ❌

// Effects (MEDIUM RISK)
public void AddEffect(string effectName)      // ❌
public void RemoveEffect(string effectName)   // ❌

// Character Abilities (LOW RISK but protect anyway)
public void UseHackerDiscount()               // ❌
public void IncrementGrifterTurns()           // ❌
// ... etc
```

---

## 🛡️ Recommended Security Implementation

### Option 1: Internal Access (RECOMMENDED)

Change **all modification methods** in `PlayerData.cs` to `internal`:

```csharp
// Before
public void AddMoney(int amount)

// After
internal void AddMoney(int amount)  // ✅ Only accessible by managers
```

**Pros**:
- Simple, clean
- Compiler-enforced
- No runtime overhead

**Cons**:
- Can't call from other assemblies (not an issue for your game)

---

### Option 2: Static Manager Pattern

Create a `PlayerDataManager` that validates all changes:

```csharp
public static class PlayerDataManager
{
    public static void AddMoney(PlayerData player, int amount)
    {
        if (!PhotonNetwork.IsMasterClient) return; // Host only!
        
        player.AddMoneyInternal(amount); // Now internal
    }
}

// In PlayerData.cs
internal void AddMoneyInternal(int amount)
{
    money += amount;
}
```

**Pros**:
- Centralized validation
- Can add logging, anti-cheat

**Cons**:
- More code
- Slightly more complex

---

### Option 3: Property Setters (Read-Only)

Make properties read-only and use RPCs:

```csharp
public int Money { get; private set; }  // ✅ Can't be set externally

[PunRPC]
private void RPC_AddMoney(int amount)
{
    if (!PhotonNetwork.IsMasterClient) return;
    Money += amount;
}
```

**Pros**:
- Very secure
- Clear intent

**Cons**:
- Requires refactoring all methods

---

## 🎯 My Recommendation

**Use Option 1: Internal Access**

1. Change all modification methods in `PlayerData.cs` to `internal`
2. Change `Tile.cs` setters to `internal`
3. Change `DaggerCard.cs` Flip() to `internal`

**Why**:
- Simplest solution
- No performance impact
- Compiler-enforced security
- Managers can still access (same assembly)

---

## 📝 What Needs to Change

### PlayerData.cs (31 methods):
```csharp
// Change from public to internal:
internal void AddMoney(int amount)
internal void RemoveMoney(int amount)
internal void AddMovementCard(int cardValue)
internal void RemoveMovementCard(int cardValue)
internal void AddDaggerCard(int cardID)
internal void RemoveDaggerCard(int cardID)
internal void SetTileIndex(int index)
internal void MoveTiles(int spaces)
internal void SetStatus(PlayerStatus newStatus)
internal void Zombify()
internal void BecomeSpectre()
internal void IncrementLastResortPurchaseCount()
internal void AddEffect(string effectName)
internal void RemoveEffect(string effectName)
internal void UseHackerDiscount()
internal void IncrementGrifterTurns()
internal void ResetGrifterCounter()
internal void IncrementRunnerRounds()
internal void ResetRunnerCounter()
internal void UseInsiderPeek()
internal void GiveThugStartingCard()
internal void IncrementSmugglerRounds()
internal void ResetSmugglerCounter()
internal void UseMastermindSwap()
```

**Keep as public** (read-only, no risk):
```csharp
public bool HasEnoughMoney(int amount)  // ✅ Just a check
public int GetMovementCardCount()       // ✅ Read-only
public int GetDaggerCardCount()         // ✅ Read-only
public int GetLastResortCost()          // ✅ Calculation only
public bool HasEffect(string effectName) // ✅ Read-only
public bool CanUseHackerDiscount()      // ✅ Read-only
// ... all CanUse methods are fine
```

---

### Tile.cs (2 methods):
```csharp
internal void SetVisualObject(GameObject visual)
internal void SetIndex(int newIndex)
```

---

### DaggerCard.cs (2 methods):
```csharp
internal void Flip()
internal void SetFace(bool bluffSide)
```

---

## 🔐 Additional Security Measures

### 1. Photon Custom Properties
When syncing over network, use **Photon Custom Properties** (read-only for non-host):

```csharp
// Only host can set
PhotonNetwork.CurrentRoom.SetCustomProperties(
    new Hashtable { { "playerMoney", money } }
);

// All players can read
int money = (int)PhotonNetwork.CurrentRoom.CustomProperties["playerMoney"];
```

### 2. Server Validation
In `GameManager`, validate all actions:

```csharp
public void ProcessPlayerAction(PlayerData player, Action action)
{
    if (!PhotonNetwork.IsMasterClient)
    {
        Debug.LogError("Only host can process actions!");
        return;
    }
    
    // Now safe to modify player data
}
```

---

## ✅ Implementation Checklist

- [ ] Change 31 methods in `PlayerData.cs` to `internal`
- [ ] Change 2 methods in `Tile.cs` to `internal`
- [ ] Change 2 methods in `DaggerCard.cs` to `internal`
- [ ] Test that managers can still access (same assembly)
- [ ] Document in code: "Internal to prevent client-side tampering"

---

## 📊 Security Summary

| File | Risk Level | Action Needed |
|------|------------|---------------|
| **PlayerData.cs** | 🔴 CRITICAL | Change to internal |
| **Tile.cs** | 🟡 LOW | Change to internal (best practice) |
| **DaggerCard.cs** | 🟡 MEDIUM | Change to internal |
| All other files | 🟢 SAFE | No changes needed |

---

## 🎯 Final Recommendation

**YES, you need to fix PlayerData.cs immediately!**

This is the single biggest security hole. Once you make those methods `internal`, your game will be safe from client-side cheating (as long as you follow host authority pattern in Phase 5).

**Want me to make these changes now?**
