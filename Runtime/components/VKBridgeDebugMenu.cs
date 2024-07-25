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
            if (GUILayout.Button("Show leaderboard with 100 points"))
            {
                ShowLeaderboardWith100Points().Forget();
            }

            //
            if (GUILayout.Button("Invite Friend"))
            {
                InviteFriend().Forget();
            }

            //
            if (GUILayout.Button("Recommend app"))
            {
                RecomendApp().Forget();
            }
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

        public async UniTaskVoid ShowLeaderboardWith100Points()
        {
            try
            {
                if (await VkBridgeFacade.ShowLeaderBoard(100)) Debug.Log("Leaderboard shown");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        //check banner ad
        public async UniTaskVoid CheckBannerAd()
        {
            try
            {
                if (await VkBridgeFacade.CheckBannerAd()) Debug.Log("Banner ad is shown");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        //hide banner ad
        public async UniTaskVoid HideBannerAd()
        {
            try
            {
                if (await VkBridgeFacade.HideBannerAd()) Debug.Log("Banner ad is hidden");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        //show the banner
        public async UniTaskVoid ShowBannerAd()
        {
            try
            {
                if (await VkBridgeFacade.ShowBannerAd()) Debug.Log("Banner ad is shown");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        //show native ad interstitial
        public async UniTaskVoid ShowInterstitial()
        {
            try
            {
                if (await VkBridgeFacade.ShowNativeInterstitialAd()) Debug.Log("Interstitial ad is shown");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        //show native ad rewarded video
        public async UniTaskVoid ShowReward()
        {
            try
            {
                if (await VkBridgeFacade.ShowNativeRewardAd()) Debug.Log("Reward ad is shown");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public async UniTaskVoid CheckNativeRewardAd()
        {
            try
            {
                if (await VkBridgeFacade.CheckNativeRewardAd()) Debug.Log("Reward ad is shown");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public async UniTaskVoid CheckNativeInterstitialAd()
        {
            try
            {
                if (await VkBridgeFacade.CheckNativeInterstitialAd()) Debug.Log("Interstitial ad is shown");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        //
        public async UniTaskVoid InviteFriend()
        {
            try
            {
                if (await VkBridgeFacade.InviteFriend()) Debug.Log("InviteFriend shown");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        //
        public async UniTaskVoid RecomendApp()
        {
            try
            {
                if (await VkBridgeFacade.RecommendApp())
                {
                    Debug.Log("App was recomended");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
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