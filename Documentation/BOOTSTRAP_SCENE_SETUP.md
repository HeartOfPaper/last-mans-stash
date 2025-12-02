# Bootstrap Scene Setup Guide

Follow these steps to create the Bootstrap scene in Unity.

---

## Step 1: Create the Bootstrap Scene

1. In Unity, go to **File** → **New Scene**
2. Save it immediately: **File** → **Save As**
3. Navigate to `Assets/Scenes/`
4. Name it: **`Bootstrap`**
5. Click **Save**

---

## Step 2: Set Up the Scene

### Create the Managers GameObject

1. Right-click in the Hierarchy → **Create Empty**
2. Name it: **`_Managers`** (underscore puts it at the top)
3. In the Inspector, click **Add Component**
4. Search for and add: **`BootstrapManager`**
5. With `_Managers` still selected:
   - Click **Add Component** again
   - Search for and add: **`PhotonConnector`**

### Configure BootstrapManager

1. Select the `_Managers` GameObject
2. In the Inspector, find the **BootstrapManager** component
3. Set **Main Menu Scene Name** to: `MainMenu`
4. **Minimum Load Time**: 1.5 (default is fine)

---

## Step 3: Set Bootstrap as the First Scene

1. Go to **File** → **Build Settings**
2. Make sure **Bootstrap** is listed in "Scenes In Build"
3. **Drag it to index 0** (the first scene)
4. Close Build Settings

---

## Step 4: Test the Bootstrap Scene

1. Make sure you're in the **Bootstrap** scene
2. Press **Play** ▶️
3. Open the **Console** (Window → General → Console)

### Expected Output:
```
=== BOOTSTRAP STARTED ===
Connecting to Photon...
Connected to Photon Master Server (Region: ...)
Joined Photon Lobby
Photon is ready!
Loading MainMenu scene...
```

You'll see an error about MainMenu scene not existing - that's normal! We'll create it next.

---

## Step 5: Stop Play Mode

Press **Stop** (or Play again) to exit Play Mode.

---

## ✅ Checklist:

- [x] Bootstrap scene created in Assets/Scenes/
- [x] _Managers GameObject with BootstrapManager + PhotonConnector
- [x] BootstrapManager configured with "MainMenu" scene name
- [x] Bootstrap scene is first in Build Settings
- [x] Tested and saw Photon connection messages

---

**Once complete, let me know and we'll create the Main Menu scene next!**
