using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VKBridgeSDK.Runtime.components
{
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
            var menuWidth = 200f;
            var menuHeight = 300f;
            var menuX = (Screen.width - menuWidth) / 2f;
            var menuY = (Screen.height - menuHeight) / 2f;

            GUILayout.BeginArea(new Rect(menuX, menuY, menuWidth, menuHeight), "VK Debug Menu", GUI.skin.window);

            if (GUILayout.Button("Test alert"))
            {
                TestAlert();
            }

            if (GUILayout.Button("Show leaderboard"))
            {
                ShowLeaderboard().Forget();
            }
            //
            // if (GUILayout.Button("Show invite box"))
            // {
            //     ShowInviteBox();
            // }
            //
            // if (GUILayout.Button("Show Alert"))
            // {
            //     ShowAlertMessageBox();
            // }
            //
            // if (GUILayout.Button("Check Reward Ads"))
            // {
            //     CheckRewards();
            // }
            //
            // if (GUILayout.Button("Check Interstitial Ads"))
            // {
            //     CheckInterstitials();
            // }
            //
            // if (GUILayout.Button("Show Reward Ads"))
            // {
            //     ShowReward();
            // }
            //
            // if (GUILayout.Button("Show Interstitial Ads"))
            // {
            //     ShowInterstitial();
            // }
            //
            // if (GUILayout.Button("Show Banner Ad"))
            // {
            //     ShowBannerAd();
            // }
            //
            // if (GUILayout.Button("Check Banner Ad"))
            // {
            //     CheckBannerAd();
            // }
            //
            // if (GUILayout.Button("Hide Banner Ad"))
            // {
            //     HideBannerAd();
            // }
            //
            // if (GUILayout.Button("Track test event in MyTarget"))
            // {
            //     TrackTestEventInMyTarget();
            // }

            GUILayout.EndArea();
        }

        public void TestAlert()
        {
            VkBridgeFacade.ShowAlert("Alert works");
        }

        public async UniTaskVoid ShowLeaderboard()
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
        //
        // public void ShowAlertMessageBox()
        // {
        //     VKBridgeHandler.Alert("Alert Works!");
        // }
        //
        // public void CheckRewards()
        // {
        //     VKBridgeHandler.CheckRewardsAvailable(status =>
        //     {
        //         VKBridgeHandler.Alert($"Reward {status}");
        //     });
        // }
        //
        // public void CheckInterstitials()
        // {
        //     VKBridgeHandler.CheckInterstitialAvailable(status =>
        //     {
        //         VKBridgeHandler.Alert($"Reward {status}");
        //     });
        // }
        //
        //
        // public void ShowReward()
        // {
        //     VKBridgeHandler.ShowReward();
        // }
        //
        // public void ShowInterstitial()
        // {
        //     VKBridgeHandler.ShowInterstitial();
        // }
        //
        // public void ShowBannerAd()
        // {
        //     VKBridgeHandler.ShowBannerAd();
        // }
        //
        // public void CheckBannerAd()
        // {
        //     VKBridgeHandler.CheckBannerAd();
        // }
        //
        // public void HideBannerAd()
        // {
        //     VKBridgeHandler.HideBannerAd();
        // }
        //
        // public void ShowInviteBox()
        // {
        //     VKBridgeHandler.ShowInviteBox();
        // }
        //
        // public void TrackTestEventInMyTarget()
        // {
        //     VKBridgeHandler.TrackMyTrackerEvent("test_event");
        // }
    }
}