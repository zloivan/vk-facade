# VKBridge Unity SDK Wrapper
## Overview
This package provides a Unity wrapper for the VKBridge JavaScript SDK, allowing seamless integration of VK functionalities within Unity projects. The wrapper is designed to handle VK API calls, manage responses, and provide a debug menu for easy testing.

## Features
Asynchronous VK API calls using UniTask
Event management system for VK events
Easy integration with Unity
Debug menu for quick access to VK functionalities

## Getting Started

### Prerequisites
* Unity 2021 LTS or later
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
* After importing the package, download samples for that package, it contains WEBGL template that is required for that SDK to work
* Copy the WEBGL template folder from the Samples folder.
* Paste the WEBGL template folder into the root directory of your Unity project.

## !!! IMPORTANT !!!
* To enable logic in VKBridgeFacade define `WEBGL_VK` in `Player / Other Settings / Scripting Define Symbols`
* To enable additional logs define `WEBGL_VK_DEBUG` in `Player / Other Settings / Scripting Define Symbols`

## Usage
### Initialization
To start using the VKBridge wrapper, initialize the VkBridgeFacade in your project's entry point:


```csharp
using VKBridgeSDK.Runtime;

public class GameInitializer : MonoBehaviour
{
void Start()
{
    VkBridgeFacade.Initialize();
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
### VkBridgeFacade
The main facade for interacting with VKBridge. It provides methods to initialize the SDK, make API calls, and manage events.

### VKMessageReceiver
A MonoBehaviour component that receives messages and events from VKBridge and delegates them to the appropriate handlers.

### VKResponseManager
Manages the responses from VKBridge API calls and resolves promises.

### VKEventManager
Handles VK events and manages event listeners.

### VKBridgeDebugMenu
Provides a debug menu for testing various VK functionalities directly within the Unity Editor.

## Example Code
VKBridgeDebugMenu

```csharp
using Cysharp.Threading.Tasks;
using UnityEngine;

public class VKBridgeDebugMenu : MonoBehaviour
{
private bool _showDebugMenu;

    private void OnGUI()
    {
#if WEBGL_VK
const float BUTTON_WIDTH = 100f;
const float BUTTON_HEIGHT = 50f;
var buttonX = Screen.width - BUTTON_WIDTH - 10f;
var buttonY = Screen.height - BUTTON_HEIGHT - 10f;

        if (GUI.Button(new Rect(buttonX, buttonY, BUTTON_WIDTH, BUTTON_HEIGHT), "VK Debug"))
        {
            OnDebugButtonClick();
        }

        if (_showDebugMenu)
        {
            DrawDebugMenu();
        }
#endif
}

    private void OnDebugButtonClick()
    {
        _showDebugMenu = !_showDebugMenu;
    }

    private void DrawDebugMenu()
    {
        // Drawing debug menu buttons
    }

    private async UniTaskVoid ShowLeaderboard()
    {
        try
        {
            if (await VkBridgeFacade.ShowLeaderBoard()) Debug.Log("Leaderboard shown");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
```

