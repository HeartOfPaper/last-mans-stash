# Creating Character Profile Assets - Step-by-Step Guide

Follow these steps to create the 5 character ScriptableObject assets in Unity.

---

## Step 1: Open Unity

Make sure your "Last Man's Stash" project is open in Unity.

---

## Step 2: Navigate to Characters Folder

1. In the **Project** window, navigate to:
   ```
   Assets/ScriptableObjects/Characters/
   ```

---

## Step 3: Create The Hacker

1. **Right-click** in the `Characters/` folder
2. Select **Create** → **Last Man's Stash** → **Character Profile**
3. **Rename** the asset to: `Hacker`
4. **Select** the Hacker asset
5. In the **Inspector**, fill in the following:

   - **Character Type**: TheHacker
   - **Character Name**: "The Hacker"
   - **Character Description**: "A tech-savvy infiltrator who knows how to exploit the system."
   - **Ability Name**: "System Exploit"
   - **Ability Description**: "Pay 2 bucks less for any 3 Last Resort purchases."
   - **Character Portrait**: (Leave empty for now - add later)
   - **Character Model**: (Leave empty for now - Phase 12)
   - **Character Color**: Blue (#4A90E2 or your choice)

6. **Save** (Ctrl+S / Cmd+S)

---

## Step 4: Create The Grifter

1. **Right-click** in the `Characters/` folder → **Create** → **Last Man's Stash** → **Character Profile**
2. **Rename** to: `Grifter`
3. **Fill in Inspector**:

   - **Character Type**: TheGrifter
   - **Character Name**: "The Grifter"
   - **Character Description**: "A master con artist who can steal more than others."
   - **Ability Name**: "Master Thief"
   - **Ability Description**: "Steal 8 bucks instead of 5 from Vault tiles. Can only use every other turn."
   - **Character Color**: Red (#E24A4A or your choice)

4. **Save**

---

## Step 5: Create The Runner

1. **Right-click** → **Create** → **Last Man's Stash** → **Character Profile**
2. **Rename** to: `Runner`
3. **Fill in Inspector**:

   - **Character Type**: TheRunner
   - **Character Name**: "The Runner"
   - **Character Description**: "Fast and agile, always one step ahead."
   - **Ability Name**: "Safehouse Expert"
   - **Ability Description**: "When landing on Safehouse tiles, draw 2 Movement Cards instead of 1. Usable once every 3 rounds."
   - **Character Color**: Green (#4AE290 or your choice)

4. **Save**

---

## Step 6: Create The Insider

1. **Right-click** → **Create** → **Last Man's Stash** → **Character Profile**
2. **Rename** to: `Insider`
3. **Fill in Inspector**:

   - **Character Type**: TheInsider
   - **Character Name**: "The Insider"
   - **Character Description**: "Has connections everywhere and knows what's coming."
   - **Ability Name**: "Inside Information"
   - **Ability Description**: "Peek at the next Casino card twice per game before deciding to flip or pass."
   - **Character Color**: Purple (#9B4AE2 or your choice)

4. **Save**

---

## Step 7: Create The Thug

1. **Right-click** → **Create** → **Last Man's Stash** → **Character Profile**
2. **Rename** to: `Thug`
3. **Fill in Inspector**:

   - **Character Type**: TheThug
   - **Character Name**: "The Thug"
   - **Character Description**: "Tough and intimidating, knows how to get what they want."
   - **Ability Name**: "Professional Setup"
   - **Ability Description**: "Start the game with a Professional Dagger Card (steal random Movement Card from target)."
   - **Character Color**: Orange (#E2904A or your choice)

4. **Save**

---

## Step 8: Create The Smuggler

1. **Right-click** → **Create** → **Last Man's Stash** → **Character Profile**
2. **Rename** to: `Smuggler`
3. **Fill in Inspector**:

   - **Character Type**: TheSmuggler
   - **Character Name**: "The Smuggler"
   - **Character Description**: "A black market dealer with connections to the underworld."
   - **Ability Name**: "Black Market Connections"
   - **Ability Description**: "At Pawn Shop tiles, draw 2 Dagger Cards (choose 1 to keep, discard the other). Usable once every 3 rounds."
   - **Character Color**: Brown (#8B4513 or your choice)

4. **Save**

---

## Step 9: Create The Mastermind

1. **Right-click** → **Create** → **Last Man's Stash** → **Character Profile**
2. **Rename** to: `Mastermind`
3. **Fill in Inspector**:

   - **Character Type**: TheMastermind
   - **Character Name**: "The Mastermind"
   - **Character Description**: "A brilliant strategist who can manipulate the board to their advantage."
   - **Ability Name**: "Position Swap"
   - **Ability Description**: "Twice per game, swap your board position with any other player (both players move to each other's tiles)."
   - **Character Color**: Cyan (#4AE2E2 or your choice)

4. **Save**

---

## Step 10: Verify All Assets

You should now have **7 character assets** in `Assets/ScriptableObjects/Characters/`:

- ✓ Hacker.asset
- ✓ Grifter.asset
- ✓ Runner.asset
- ✓ Insider.asset
- ✓ Thug.asset
- ✓ Smuggler.asset
- ✓ Mastermind.asset

**Check each one** to make sure:
- Character Type is set correctly
- Name and description are filled in
- Ability name and description are filled in
- Color is assigned

---

## ✅ Done!

**Phase 2 is now 100% complete!** All character profiles are ready for use.

---

## 📝 Notes:

- **Portraits**: You can add character portrait sprites later when you have artwork
- **Models**: 3D character models will be added in Phase 12
- **Colors**: These colors will be used for player tokens, UI highlights, etc.
- **Starting Bonuses**: Leave at 0 for now (all characters start equal except for their unique abilities)

---

## 🎯 What's Next?

After creating these assets, you're ready for:
- **Phase 3**: Board System (BoardManager, tile ordering, tile effects)

**Let me know when you're done creating the assets!**
