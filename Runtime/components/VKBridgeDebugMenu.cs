using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace vk_facade.Runtime.components
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
            const float MENU_WIDTH = 250f;
            const float MENU_HEIGHT = 600;
            
            var menuX = (Screen.width - MENU_WIDTH) / 2f;
            var menuY = (Screen.height - MENU_HEIGHT) / 2f;

            GUILayout.BeginArea(new Rect(menuX, menuY, MENU_WIDTH + 10, MENU_HEIGHT), "VK Debug Menu", GUI.skin.window);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(MENU_WIDTH - 10),
                GUILayout.Height(MENU_HEIGHT));

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

            if (GUILayout.Button("Get Friends List"))
            {
                GetFriendsList().Forget();
            }

            if (GUILayout.Button("Publish post"))
            {
                PublishPostOnWall().Forget();
            }
            
            //GetUserData();
            if (GUILayout.Button("Get user data"))
            {
                GetUserData().Forget();
            }
            
            //ShowOrderBox();
            if (GUILayout.Button("Show Order Box"))
            {
                ShowOrderBox().Forget();
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

        private async UniTaskVoid PublishPostOnWall()//TODO Missing require param
        {
            try
            {
                await VkBridgeFacade.PublishPostOnWall($"Думаете, вы лучше меня? Попробуйте побить мой рекорд: " +
                                                           $"{1500}?! \nЖду вас здесь: ",
                        $"https://vk.com/app52010090");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        
        private async UniTaskVoid GetFriendsList()
        {
            try
            {
                var friends = await VkBridgeFacade.GetFriendList(true);
                foreach (var friend in friends.users)
                {
                    Debug.Log($"FriendFirstName: {friend.first_name}, LastName: {friend.last_name}, ID: {friend.id}");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        private async UniTaskVoid GetUserData()
        {
            try
            {
                var userData= await VkBridgeFacade.GetUserData();
                Debug.Log(userData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async UniTaskVoid ShowOrderBox()
        {
            try
            {
                var orderId = await VkBridgeFacade.ShowOrderBox("Товар 1", "100 рублей");//TODO incorrect params
                Debug.Log($"Order ID: {orderId}");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

    }
}