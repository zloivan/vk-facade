using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VKBridgeSDK.Runtime.components;
using VKBridgeSDK.Runtime.data;
using VKBridgeSDK.Runtime.managers;
using Object = UnityEngine.Object;

namespace VKBridgeSDK.Runtime
{
    /// <summary>
    /// В классе используются ананимные типы не просто так,
    /// не заменяйте ан множества, если в API VK явно не сказанно что там ожидают массив
    /// </summary>
    public static class VkBridgeFacade
    {
        

        private static VKResponseManager _vkResponseManager;

        private static VKEventManager _eventManager;

        private static GameObject _messageReceiverObject;

        private static VKMessageReceiver _vkMessageReceiver;

        public const bool IsDebug = true;

        public static void Initialize()
        {
            _vkResponseManager = new VKResponseManager();
            _eventManager = new VKEventManager();

            _messageReceiverObject = new GameObject("VKMessageReceiver");
            _vkMessageReceiver = _messageReceiverObject.AddComponent<VKMessageReceiver>();
            _vkMessageReceiver.Initialize(_vkResponseManager, _eventManager);

            Object.DontDestroyOnLoad(_messageReceiverObject);

            if (IsDebug)
            {
                SpawnDebugMenu();
            }
        }

        private static void SpawnDebugMenu()
        {
            var go = new GameObject("VKDebugMenu");
            go.AddComponent<VKBridgeDebugMenu>();
            Object.DontDestroyOnLoad(go);
        }

        public static async UniTask<VKRequestData> CallAPIMethod(string methodName, string parameters)
        {
            return await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCallAPIMethod",
                new { method = methodName, @params = parameters });
        }

        public static async UniTask<bool> VkBridgeInit()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppInit");
            return vkData.result;
        }

        public static async UniTask<bool> InviteFriend(string inviteRequestKey = null)
        {
            var methodParameters = string.IsNullOrEmpty(inviteRequestKey)
                ? null
                : new { requestKey = inviteRequestKey };

            var vkPromiseData =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowInviteBox", methodParameters);

            return vkPromiseData.result;
        }

        public static async UniTask<bool> CheckNativeInterstitialAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCheckNativeAds", new
            {
                ad_format = "interstitial",
                use_waterfall = useWaterfall
            });

            return vkPromise.result;
        }

        public static async UniTask<bool> CheckNativeRewardAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCheckNativeAds", new
            {
                ad_format = "reward",
                use_waterfall = useWaterfall
            });

            return vkPromise.result;
        }

        public static async UniTask<bool> ShowNativeRewardAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowNativeAds", new
            {
                ad_format = "reward",
                use_waterfall = useWaterfall
            });

            return vkPromise.result;
        }

        public static async UniTask<bool> ShowNativeInterstitialAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowNativeAds", new
            {
                ad_format = "interstitial",
                use_waterfall = useWaterfall
            });

            return vkPromise.result;
        }

        public static async UniTask<bool> ShowBannerAd(BannerLocation location = BannerLocation.bottom,
            BannerAlign align = BannerAlign.center,
            BannerOrientation orientation = BannerOrientation.horizontal,
            BannerLayout layout = BannerLayout.resize)
        {
            var parameters = new
            {
                banner_location = Enum.GetName(typeof(BannerLocation), location),
                banner_align = Enum.GetName(typeof(BannerAlign), align),
                banner_orientation = Enum.GetName(typeof(BannerOrientation), orientation),
                banner_layout = Enum.GetName(typeof(BannerLayout), layout)
            };

            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowBannerAd", parameters);
            return vkPromise.result;
        }

        public static async UniTask<bool> HideBannerAd()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppHideBannerAd");
            return vkData.result;
        }


        public static async UniTask<bool> CheckBannerAd()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCheckBannerAd");
            return vkData.result;
        }

        public static async UniTask<bool> RecommendApp()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppRecommend");
            return vkData.result;
        }

        public static async UniTask<bool> ShowLeaderBoard(int result = -1)
        {
            var methodParameters = result == -1
                ? null
                : new { user_result = result };

            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowLeaderBoardBox",
                    methodParameters);

            return vkPromise.result;
        }

        public static async UniTask<bool> PublishPostOnWall(string postMessage,
            string postAttachments = "",
            bool friendsOnly = true)
        {
            object methodParameters;
            
            if (!string.IsNullOrEmpty(postAttachments))
            {
                 methodParameters = new
                    { message = postMessage, attachments = postAttachments, friends_only = friendsOnly };
            }
            else
            {
                 methodParameters = new
                    { message = postMessage, friends_only = friendsOnly };
            }

            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowWallPostBox", methodParameters);
            return vkPromise.result;
        }

        /// <summary>
        /// Открывает окно покупки виртуальной ценности в мини-приложении или игре.
        /// </summary>
        /// <param name="itemType">Тип виртуальной ценности. Всегда имеет значение item.</param>
        /// <param name="itemId">Название виртуальной ценности. Будет передано в уведомлении на получение информации о виртуальной ценности. Длина строки: 64 символа.</param>
        /// <returns></returns>
        public static async UniTask<bool> ShowOrderBox(string itemType, string itemId)
        {
            var methodParameters = new { type = itemType, item = itemId };

            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKOrderData>("VKWebAppShowOrderBox", methodParameters);
            return vkPromise.success;
        }

        public static async UniTask<VKSubscriptionData> ShowSubscriptionBox(SubscriptionAction action,
            string subscriptionItem,
            string subscriptionId)
        {
            var methodParameters = new
            {
                action = Enum.GetName(typeof(SubscriptionAction), action),
                item = subscriptionItem,
                subscription_id = subscriptionId
            };

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKSubscriptionData>("VKWebAppShowSubscriptionBox",
                    methodParameters);
            return vkData;
        }

        /// <summary>
        /// Показывает окно выбора друзей из списка и получает информацию о них.
        /// </summary>
        /// <param name="isMultiSelect">Информация о том, выбрать ли нескольких друзей из списка или одного. </param>
        /// <returns></returns>
        public static async UniTask<VKFriendsData> GetFriendList(bool isMultiSelect)
        {
            var methodParameters = new
            {
                multi = isMultiSelect
            };

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKFriendsData>("VKWebAppGetFriends", methodParameters);
            return vkData;
        }

        public static async UniTask<VKUserData> GetUserData(int? userId = null)
        {
            var methodParameters = new Dictionary<string, object>();

            if (userId != null)
            {
                methodParameters.Add("user_id", userId);
            }

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKUserData>("VKWebAppGetUserInfo", methodParameters);

            return vkData;
        }

        public static async UniTask<VKUsersData> GetUsersData(params int[] usersId)
        {
            var methodParameters = new Dictionary<string, object>();

            if (usersId != null)
            {
                methodParameters.Add("users_id", usersId);
            }

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKUsersData>("VKWebAppGetUserInfo", methodParameters);

            return vkData;
        }

        public static void ShowAlert(string message)
        {
            _vkResponseManager.ShowAlert(message);
        }

        public static void AddEventListener(VKBridgeEventType eventType, Action<VKEventData> listener)
        {
            _eventManager.AddEventListener(eventType, listener);
        }

        public static void RemoveEventListener(VKBridgeEventType eventType, Action<VKEventData> listener)
        {
            _eventManager.RemoveEventListener(eventType, listener);
        }

        public static void Reset()
        {
            if (_messageReceiverObject != null)
            {
                Object.Destroy(_messageReceiverObject);
            }

            _vkResponseManager = null;
            _eventManager = null;
            _messageReceiverObject = null;
            _vkMessageReceiver = null;
        }
    }
}