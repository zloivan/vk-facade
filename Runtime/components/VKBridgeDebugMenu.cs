using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using vk_facade.Runtime.data;
using vk_facade.Runtime.helpers;
using ILogger = UnityEngine.ILogger;

namespace vk_facade.Runtime.components
{
    public class VKBridgeDebugMenu : MonoBehaviour
    {
        private bool _showDebugMenu;
        private readonly ILogger _logger = new VKBridgeLogger();

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

            //ShowOrderBox();
            if (GUILayout.Button("Save data"))
            {
                SaveApple().Forget();
            }

            //ShowOrderBox();
            if (GUILayout.Button("Save days of week"))
            {
                SaveDaysInWeek().Forget();
            }

            //ShowOrderBox();
            if (GUILayout.Button("Save flat earth"))
            {
                SaveEarthIsFlat().Forget();
            }

            //ShowOrderBox();
            if (GUILayout.Button("Save person"))
            {
                SavePerson().Forget();
            }
            
            //ShowOrderBox();
            if (GUILayout.Button("Get Key Names"))
            {
                GetKeyNames().Forget();
            }

            //ShowOrderBox();
            if (GUILayout.Button("Load data"))
            {
                LoadDataFromStorage().Forget();
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
                if (await VkBridgeFacade.ShowLeaderBoard()) _logger.Log("DEBUG_MENU", "Leaderboard shown");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid ShowLeaderboardWith100Points()
        {
            try
            {
                if (await VkBridgeFacade.ShowLeaderBoard(100)) _logger.Log("DEBUG_MENU", "Leaderboard shown");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid CheckBannerAd()
        {
            try
            {
                if (await VkBridgeFacade.CheckBannerAd()) _logger.Log("DEBUG_MENU", "Banner ad is shown");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid HideBannerAd()
        {
            try
            {
                if (await VkBridgeFacade.HideBannerAd()) _logger.Log("DEBUG_MENU", "Banner ad is hidden");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid ShowBannerAd()
        {
            try
            {
                if (await VkBridgeFacade.ShowBannerAd()) _logger.Log("DEBUG_MENU", "Banner ad is shown");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid ShowInterstitial()
        {
            try
            {
                if (await VkBridgeFacade.ShowNativeInterstitialAd())
                    _logger.Log("DEBUG_MENU", "Interstitial ad is shown");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid ShowReward()
        {
            try
            {
                if (await VkBridgeFacade.ShowNativeRewardAd()) _logger.Log("DEBUG_MENU", "Reward ad is shown");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid CheckNativeRewardAd()
        {
            try
            {
                if (await VkBridgeFacade.CheckNativeRewardAd()) _logger.Log("DEBUG_MENU", "Reward ad is ready");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid CheckNativeInterstitialAd()
        {
            try
            {
                if (await VkBridgeFacade.CheckNativeInterstitialAd())
                    _logger.Log("DEBUG_MENU", "Interstitial ad is ready");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid InviteFriend()
        {
            try
            {
                if (await VkBridgeFacade.InviteFriend()) _logger.Log("DEBUG_MENU", "InviteFriend shown");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
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
                    _logger.Log("DEBUG_MENU", "App was recomended");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid PublishPostOnWall()
        {
            try
            {
                await VkBridgeFacade.PublishPostOnWall($"Думаете, вы лучше меня? Попробуйте побить мой рекорд: " +
                                                       $"{1500}?! \nЖду вас здесь: ",
                    $"https://vk.com/app52010090");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
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
                    _logger.Log("DEBUG_MENU",
                        $"FriendFirstName: {friend.first_name}, LastName: {friend.last_name}, ID: {friend.id}");
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
                var userData = await VkBridgeFacade.GetUserData();
                _logger.Log("DEBUG_MENU", userData);
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
            }
        }

        private async UniTaskVoid ShowOrderBox()
        {
            try
            {
                var orderId = await VkBridgeFacade.ShowOrderBox("Товар 1", "100 рублей");
                _logger.Log("DEBUG_MENU", $"Order ID: {orderId}");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
                throw;
            }
        }

        private async UniTaskVoid SaveApple()
        {
            try
            {
                _logger.Log("DEBUG_MENU",
                    "Setting data:[apple, green]");

                if (await VkBridgeFacade.StorageSet("apple", "green"))
                    _logger.Log("DEBUG_MENU", "Apple is saved");

                _logger.Log("DEBUG_MENU", "Data saved to storage");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
                throw;
            }
        }

        private async UniTaskVoid SaveDaysInWeek()
        {
            try
            {
                _logger.Log("DEBUG_MENU",
                    "Setting data:[daysInWeek, 7]");

                if (await VkBridgeFacade.StorageSet("daysInWeek", 7.ToString()))
                    _logger.Log("DEBUG_MENU", "Days in week is saved");

                _logger.Log("DEBUG_MENU", "Data saved to storage");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
                throw;
            }
        }

        private async UniTaskVoid SaveEarthIsFlat()
        {
            try
            {
                _logger.Log("DEBUG_MENU",
                    "Setting data:[earthIsFlat, false]");


                if (await VkBridgeFacade.StorageSet("earthIsFlat", false.ToString()))
                    _logger.Log("DEBUG_MENU", "Earth is flat is saved");


                _logger.Log("DEBUG_MENU", "Data saved to storage");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
                throw;
            }
        }

        private async UniTaskVoid SavePerson()
        {
            try
            {
                _logger.Log("DEBUG_MENU",
                    "Setting data:[person, {name=john, age=5, gender=male}]");

                var jObject = JsonConvert.SerializeObject(new { name = "john", age = 5, gender = "male" });

                _logger.Log("DEBUG_MENU", $"Person as JSON before save: {jObject}");

                if (await VkBridgeFacade.StorageSet("person", jObject))
                    _logger.Log("DEBUG_MENU", "Person is saved");

                _logger.Log("DEBUG_MENU", "Data saved to storage");
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
                throw;
            }
        }

        public async UniTaskVoid GetKeyNames()
        {
            try
            {
                var keyNamesContainer = await VkBridgeFacade.StorageGetKeys(100,0);

                if (keyNamesContainer != null && keyNamesContainer.keys != null && keyNamesContainer.keys.Length > 0)
                {
                    _logger.Log("DEBUG_MENU",$"Keys: {string.Join(", ", keyNamesContainer)}");
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
                throw;
            }
        }

        private async UniTaskVoid LoadDataFromStorage()
        {
            try
            {
                var vkStorage =
                    await VkBridgeFacade.StorageGet("apple", "daysInWeek", "earthIsFlat", "person");


                _logger.Log("DEBUG_MENU", $"Apple is: {vkStorage["apple"]}");
                _logger.Log("DEBUG_MENU", $"Days in week: {vkStorage["daysInWeek"]}");
                _logger.Log("DEBUG_MENU", $"Earth is flat: {vkStorage["earthIsFlat"]}");

                try
                {
                    var person = JsonConvert.DeserializeObject<JObject>(vkStorage["person"]);

                    _logger.Log("DEBUG_MENU",
                        $"Person is: Name: {person["name"]}, Age: {person["age"]?.ToObject<int>()}, Gender: {person["gender"]}");
                }
                catch (Exception e)
                {
                    _logger.LogError("DEBUG_MENU", e);
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("DEBUG_MENU", e);
                throw;
            }
        }
    }
}