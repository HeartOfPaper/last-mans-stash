# Photon PUN 2 Setup Guide

Follow these steps to set up Photon PUN 2 for multiplayer networking.

---

## Step 1: Install Photon PUN 2

1. Open Unity
2. Go to **Window > Asset Store**
3. Search for **"PUN 2 FREE"**
4. Download and Import **PUN 2 - FREE** by Exit Games
5. Click **Import** when the import dialog appears
6. Wait for import to complete

---

## Step 2: Create Photon Account & Get AppId

1. Go to [https://www.photonengine.com/](https://www.photonengine.com/)
2. Click **Sign Up** (or **Log In** if you have an account)
3. After logging in, go to **Dashboard**
4. Click **Create a New App**
5. Fill in the form:
   - **Photon Type**: Select **Photon PUN**
   - **Name**: "Last Man's Stash" (or any name you prefer)
   - **Description**: (optional)
6. Click **Create**
7. Your **App ID** will be displayed - **copy this ID**

---

## Step 3: Configure Photon in Unity

### Option A: Using PUN Wizard (Recommended)
1. In Unity, go to **Window > Photon Unity Networking > PUN Wizard**
2. Click **Setup Project**
3. Paste your **App ID** in the field
4. Click **Setup**

### Option B: Manual Setup
1. In Unity, go to **Window > Photon Unity Networking > Highlight Server Settings**
2. In the Inspector, paste your **App ID** in the **AppIdRealtime** field
3. Set the following settings:
   - **Fixed Region**: Leave blank (auto-select best region)
   - **Use Name Server**: Checked
   - **Server**: Leave as default

---

## Step 4: Verify Installation

1. Check that the following folders exist in your Assets:
   - `Photon/`
   - `PhotonLibs/`
   
2. In Unity, go to **Window > Photon Unity Networking > Highlight Server Settings**
3. Verify that your **App ID** is shown

---

## Step 5: Test Basic Connection

Create a simple test script to verify Photon works:

```csharp
using UnityEngine;
using Photon.Pun;

public class PhotonConnectionTest : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✓ Connected to Photon Master Server!");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError($"✗ Disconnected from Photon: {cause}");
    }
}
```

1. Create an empty GameObject in a test scene
2. Attach this script to it
3. Press Play
4. Check the Console - you should see "✓ Connected to Photon Master Server!"

---

## Important Settings

### Photon Server Settings
Location: `Assets/Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset`

**Recommended Settings**:
- **App Id Realtime**: Your App ID
- **Use Name Server**: ✓ Checked
- **Fixed Region**: (blank)
- **Protocol**: UDP
- **Send Rate**: 20
- **Serialization Rate**: 10

### PUN Settings
- **Auto-Join Lobby**: Unchecked (we'll join manually)
- **Enable Lobby Statistics**: Checked
- **Network Logging**: Set to "Informational" during development, "Errors Only" for production

---

## Troubleshooting

### "Could not connect to Photon"
- Check your internet connection
- Verify App ID is correct
- Make sure you selected **PUN** (not other Photon products) when creating the app

### "Invalid App ID"
- App ID must be exactly as shown in Photon Dashboard
- No extra spaces or characters

### Import Errors
- Delete the `Photon/` and `PhotonLibs/` folders
- Re-import PUN 2 from Asset Store

---

## Next Steps

Once Photon is set up, you're ready to:
1. Create the Bootstrap scene
2. Implement connection logic
3. Build the room/lobby system

---

**You're all set!** ✓
