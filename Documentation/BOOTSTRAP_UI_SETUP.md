# Bootstrap Scene - Loading UI Setup

Now let's add a simple loading screen to the Bootstrap scene.

---

## Step 1: Create the Canvas

1. **In the Bootstrap scene**, right-click in Hierarchy → **UI** → **Canvas**
2. The Canvas will be created with an EventSystem - that's good!
3. **Rename the Canvas** to: `LoadingUI`

### Configure Canvas:
1. Select `LoadingUI` in Hierarchy
2. In Inspector, **Canvas** component:
   - **Render Mode**: Screen Space - Overlay
   - **Pixel Perfect**: Checked ✓

---

## Step 2: Create Background Panel

1. Right-click on `LoadingUI` → **UI** → **Panel**
2. Rename it to: `Background`
3. In Inspector, **Image** component:
   - **Color**: Black with full opacity (R:0, G:0, B:0, A:255)

---

## Step 3: Create Status Text

1. Right-click on `LoadingUI` → **UI** → **Text - TextMeshPro**
   - If prompted to import TMP Essentials, click **Import TMP Essentials**
2. Rename it to: `StatusText`
3. Position it:
   - **Rect Transform**:
     - Anchor: Middle Center
     - Pos X: 0, Pos Y: -50
     - Width: 600, Height: 100
4. Configure text:
   - **Text**: "Initializing..."
   - **Font Size**: 28
   - **Alignment**: Center (horizontal and vertical)
   - **Color**: White

---

## Step 4: Create Loading Spinner

1. Right-click on `LoadingUI` → **UI** → **Image**
2. Rename it to: `LoadingSpinner`
3. Position it:
   - **Rect Transform**:
     - Anchor: Middle Center
     - Pos X: 0, Pos Y: 50
     - Width: 80, Height: 80
4. Configure image:
   - **Color**: White
   - **Image Type**: Simple (for now - you can add a spinner sprite later)

---

## Step 5: Add BootstrapUI Component

1. Select `LoadingUI` in Hierarchy  
2. In Inspector, click **Add Component**
3. Search for: **BootstrapUI**
4. Click to add it
5. **Assign references**:
   - **Status Text**: Drag `StatusText` from Hierarchy → Status Text field
   - **Loading Spinner**: Drag `LoadingSpinner` from Hierarchy → Loading Spinner field
6. **Spin Speed**: 180 (default is fine)

---

## Step 6: Connect to BootstrapManager

1. Select `_Managers` GameObject in Hierarchy
2. In Inspector, find **BootstrapManager** component
3. In the **UI** section:
   - **Bootstrap UI**: Drag `LoadingUI` from Hierarchy → Bootstrap UI field

---

## Step 7: Test!

1. Press **Play** ▶️
2. You should see:
   - Black screen
   - "Initializing..." text
   - Loading spinner rotating
   - Text updates: "Connecting to servers..." → "Connected! Loading..."
   - Then tries to load MainMenu (will error - that's expected)

---

## ✅ Checklist:

- [ ] Canvas `LoadingUI` created
- [ ] Background panel (black)
- [ ] StatusText (TMP) centered
- [ ] LoadingSpinner image
- [ ] BootstrapUI component added to LoadingUI with references assigned
- [ ] BootstrapManager has Bootstrap UI reference set
- [ ] Tested and see loading screen with rotating spinner

---

**Once this is working, the Bootstrap scene is complete!** 🎉
