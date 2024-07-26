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

        private Vector2 scrollPosition = Vector2.zero;

        private void DrawDebugMenu()
        {
            var menuWidth = 200f;
            var menuHeight = 300f;
            var menuX = (Screen.width - menuWidth) / 2f;
            var menuY = (Screen.height - menuHeight) / 2f;

            GUILayout.BeginArea(new Rect(menuX, menuY, menuWidth, menuHeight), "VK Debug Menu", GUI.skin.window);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(menuWidth),
                GUILayout.Height(menuHeight));

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
                RecommendApp().Forget();
            }

            if (GUILayout.Button("Check banner ad"))
            {
                CheckBannerAd().Forget();
            }

            if (GUILayout.Button("Hide banner ad"))
            {
                HideBannerAd().Forget();
            }

            if (GUILayout.Button("Check interstitial ad"))
            {
                CheckNativeInterstitialAd().Forget();
            }

            if (GUILayout.Button("Show interstitial ad"))
            {
                ShowInterstitial().Forget();
            }

            if (GUILayout.Button("Show interstitial ad"))
            {
                ShowReward().Forget();
            }

            if (GUILayout.Button("Show Banner Ad"))
            {
                ShowBannerAd().Forget();
            }

            if (GUILayout.Button("Check Native Reward Ad"))
            {
                CheckNativeRewardAd().Forget();
            }

            // End the scroll view
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void TestAlert()
        {
            VkBridgeFacade.ShowAlert("Alert works");
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

        private async UniTaskVoid ShowLeaderboardWith100Points()
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

        private async UniTaskVoid CheckBannerAd()
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

        private async UniTaskVoid HideBannerAd()
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

        private async UniTaskVoid ShowBannerAd()
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

        private async UniTaskVoid ShowInterstitial()
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

        private async UniTaskVoid ShowReward()
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

        private async UniTaskVoid CheckNativeRewardAd()
        {
            try
            {
                if (await VkBridgeFacade.CheckNativeRewardAd()) Debug.Log("Reward ad is ready");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async UniTaskVoid CheckNativeInterstitialAd()
        {
            try
            {
                if (await VkBridgeFacade.CheckNativeInterstitialAd()) Debug.Log("Interstitial ad is ready");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async UniTaskVoid InviteFriend()
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
        private async UniTaskVoid RecommendApp()
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

        private async UniTaskVoid PublishPostOnWall()
        {
            try
            {
                if (await VkBridgeFacade.PublishPostOnWall("Побьете мой рекорд?", "https://vk.com/app52010090", true))
                    Debug.Log("Post was published on wall");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}