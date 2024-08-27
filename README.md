Certainly! I'll update the README.md file with more relevant examples, including information about the storage module and the new index.html template. Here's an updated version of the README:

```markdown
# VKBridge Unity SDK Wrapper

## Overview
This package provides a Unity wrapper for the VKBridge JavaScript SDK, allowing seamless integration of VK functionalities within Unity projects. The wrapper is designed to handle VK API calls, manage responses, and provide a debug menu for easy testing.

## Features
- Asynchronous VK API calls using UniTask
- Event management system for VK events
- Easy integration with Unity
- Debug menu for quick access to VK functionalities
- Storage system for saving and loading data
- Dynamic game container for handling banner ads without changing in-game logic

## Getting Started

### Prerequisites
* Unity 2020 LTS or later
* cysharp/UniTask package
* Newtonsoft.Json package

## Installation

#### Importing the Package via Package Manager
* Open Unity and go to Window > Package Manager.
* Click on the + button in the top left corner and select Add package from git URL...
* Enter the following URL: `https://github.com/zloivan/vk-facade.git`
* Click Add to import the package.

#### Importing the Package via Git
* Open your Unity project.
* Navigate to Packages/manifest.json.
* Add the following line to the dependencies section:

```json
"com.zloivan.vk-facade": "https://github.com/zloivan/vk-facade.git"
```
Save the manifest.json file. Unity will automatically download and import the package.

### Adding WebGL Template
* After importing the package, download samples for that package, it contains WEBGL templates that are required for this SDK to work
* Copy the WEBGL template folders from the Samples folder.
* Paste the WEBGL template folders into the root directory of your Unity project.

## !!! IMPORTANT !!!
* To enable logic in VKBridgeFacade define `WEBGL_VK` in `Player / Other Settings / Scripting Define Symbols`
* To enable additional logs define `WEBGL_VK_DEBUG` in `Player / Other Settings / Scripting Define Symbols`

## Usage

### Initialization
To start using the VKBridge wrapper, initialize the VkBridgeFacade in your project's entry point:

```csharp
    public static class VKInitialization
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static async void Initialize()
        {
            //Recommended way to initialize, this will also get all storage data from VK, after this call you can use Storage
            await VkBridgeFacade.InitializeAsync();
            
            
            //VkBridgeFacade.Initialize(); //identical to VkBridgeFacade.InitializeAsync().Forget();
            
            //Optional, if you want to save data on focus change
            VkBridgeFacade.AddEventListener(VKBridgeEventType.FocusChanged, HandleFocusChanged);
        }

        private static void HandleFocusChanged(VKEventData obj)
        {
            VkBridgeFacade.Storage.Save();
        }
    }
```

### Making API Calls
You can make VK API calls asynchronously using the VkBridgeFacade methods. For example, to show a leaderboard:

```csharp
private async void ShowLeaderboard()
{
    bool success = await VkBridgeFacade.ShowLeaderBoard();
    Debug.Log($"Leaderboard shown: {success}");
}
```

### Storage System
The VKBridge SDK now includes a storage system for saving and loading data. Here's an example of how to use it:

```csharp
// Save data
VkBridgeFacade.Storage.SetString("key", "value");
VkBridgeFacade.Storage.SetInt("score", 100);
VkBridgeFacade.Storage.SetFloat("health", 95.5f);

// Load data
string value = VkBridgeFacade.Storage.GetString("key");
int score = VkBridgeFacade.Storage.GetInt("score");
float health = VkBridgeFacade.Storage.GetFloat("health");

// Save changes
await VkBridgeFacade.Storage.Save();
```

### Dynamic Game Container
The new index.html template provides a dynamic game container that automatically adjusts to banner ads without requiring changes to your in-game logic. To use this template:

1. Select the new template in your Unity WebGL build settings.
2. Use the VKBridge SDK to show and hide banner ads as usual.

The game container will automatically resize when banners are shown or hidden, maintaining your game's aspect ratio and layout.

### Event Handling
To handle VK events, add event listeners using the VkBridgeFacade:

```csharp
VkBridgeFacade.AddEventListener(VKBridgeEventType.FocusChanged, OnFocusChanged);

private void OnFocusChanged(VKEventData eventData)
{
    Debug.Log("Focus changed event received");
}
```

## Components
- VkBridgeFacade: The main facade for interacting with VKBridge.
- VKMessageReceiver: Receives messages and events from VKBridge.
- VKResponseManager: Manages responses from VKBridge API calls.
- VKEventManager: Handles VK events and manages event listeners.
- VKBridgeDebugMenu: Provides a debug menu for testing VK functionalities.
- VKStorageManager: Manages data storage and retrieval.

## Example Code
Here's an example of using various VKBridge SDK features:

```csharp
using Cysharp.Threading.Tasks;
using UnityEngine;
using VKBridgeSDK.Runtime;

public class VKBridgeExample : MonoBehaviour
{
    private async void Start()
    {
        VkBridgeFacade.Initialize();
        await ShowLeaderboard();
        await SaveAndLoadData();
        ShowBannerAd();
    }

    private async UniTask ShowLeaderboard()
    {
        bool success = await VkBridgeFacade.ShowLeaderBoard();
        Debug.Log($"Leaderboard shown: {success}");
    }

    private async UniTask SaveAndLoadData()
    {
        VkBridgeFacade.Storage.SetInt("score", 100);
        await VkBridgeFacade.Storage.Save();

        int score = VkBridgeFacade.Storage.GetInt("score");
        Debug.Log($"Loaded score: {score}");
    }

    private async void ShowBannerAd()
    {
        var bannerData = await VkBridgeFacade.ShowBannerAd();
        Debug.Log($"Banner shown: {bannerData.result}");
    }
}
```

This updated README provides more comprehensive information about the VKBridge SDK, including examples of using the storage system and mentions the new dynamic game container feature. It also maintains the existing structure and important notes from the original README.