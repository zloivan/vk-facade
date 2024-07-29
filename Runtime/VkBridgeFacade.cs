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
    public static class VkBridgeFacade
    {
        public enum BannerLocation
        {
            bottom,
            top
        }

        public enum BannerAlign
        {
            left,
            right,
            center
        }

        public enum BannerLayout
        {
            resize,
            overlay
        }

        public enum BannerOrientation
        {
            vertical,
            horizontal
        }

        private static VKResponseManager _vkResponseManager;
        private static VKEventManager _eventManager;
        private static GameObject _messageReceiverObject;
        private static VKMessageReceiver _vkMessageReceiver;
        public const bool IsDebug = true;
        public static bool _useOld = true;

        public static bool UseOld
        {
            get => _useOld;
            set
            {
                _useOld = value;
                _vkResponseManager.useold = value;
                Debug.Log($"Using old: {value}");
            }
        }


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

        [Obsolete]
        public static async UniTask<VKPromise> CallAPIMethodOld(string methodName, string parameters)
        {
            return await _vkResponseManager.CallVkMethodAsync("VKWebAppCallAPIMethod",
                new { method = methodName, @params = parameters });
        }

        public static async UniTask<VKRequestData> CallAPIMethodNew(string methodName, string parameters)
        {
            return await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCallAPIMethod",
                new { method = methodName, @params = parameters });
        }

        public static async UniTask<bool> VkBridgeInit()
        {
            if (UseOld)
            {
                return await VkBridgeInitOld();
            }
            else
            {
                return await VkBridgeInitNew();
            }
        }

        [Obsolete]
        public static async UniTask<bool> VkBridgeInitOld()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppInit");
            return vkPromise.data.result;
        }

        public static async UniTask<bool> VkBridgeInitNew()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppInit");
            return vkData.result;
        }

        public static async UniTask<bool> InviteFriend(string inviteRequestKey = null)
        {
            if (UseOld)
            {
                return await InviteFriendOld(inviteRequestKey);
            }
            else
            {
                return await InviteFriendNew(inviteRequestKey);
            }
        }

        public static async UniTask<bool> InviteFriendOld(string inviteRequestKey = null)
        {
            var methodParameters = string.IsNullOrEmpty(inviteRequestKey)
                ? null
                : new { requestKey = inviteRequestKey };

            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowInviteBox", methodParameters);

            return vkPromise.data.result;
        }

        public static async UniTask<bool> InviteFriendNew(string inviteRequestKey = null)
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
            if (UseOld)
            {
                return await CheckNativeInterstitialAdOld(useWaterfall);
            }
            else
            {
                return await CheckNativeInterstitialAdNew(useWaterfall);
            }
        }

        public static async UniTask<bool> CheckNativeInterstitialAdOld(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppCheckNativeAds", new
            {
                ad_format = "interstitial",
                use_waterfall = useWaterfall
            });

            return vkPromise.data.result;
        }

        public static async UniTask<bool> CheckNativeInterstitialAdNew(bool useWaterfall = true)
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
            if (UseOld)
            {
                return await CheckNativeRewardAdOld(useWaterfall);
            }
            else
            {
                return await CheckNativeRewardAdNew(useWaterfall);
            }
        }

        public static async UniTask<bool> CheckNativeRewardAdOld(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppCheckNativeAds", new
            {
                ad_format = "reward",
                use_waterfall = useWaterfall
            });

            return vkPromise.data.result;
        }

        public static async UniTask<bool> CheckNativeRewardAdNew(bool useWaterfall = true)
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
            if (UseOld)
            {
                return await ShowNativeRewardAdOld(useWaterfall);
            }
            else
            {
                return await ShowNativeRewardAdNew(useWaterfall);
            }
        }

        public static async UniTask<bool> ShowNativeRewardAdOld(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowNativeAds", new
            {
                ad_format = "reward",
                use_waterfall = useWaterfall
            });

            return vkPromise.data.result;
        }

        public static async UniTask<bool> ShowNativeRewardAdNew(bool useWaterfall = true)
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
            if (UseOld)
            {
                return await ShowNativeInterstitialAdOld(useWaterfall);
            }
            else
            {
                return await ShowNativeInterstitialAdNew(useWaterfall);
            }
        }

        public static async UniTask<bool> ShowNativeInterstitialAdOld(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowNativeAds", new
            {
                ad_format = "interstitial",
                use_waterfall = useWaterfall
            });

            return vkPromise.data.result;
        }

        public static async UniTask<bool> ShowNativeInterstitialAdNew(bool useWaterfall = true)
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
            if (UseOld)
            {
                return await ShowBannerAdOld(location, align, orientation, layout);
            }
            else
            {
                return await ShowBannerAdNew(location, align, orientation, layout);
            }
        }

        public static async UniTask<bool> ShowBannerAdOld(BannerLocation location = BannerLocation.bottom,
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

            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowBannerAd", parameters);
            return vkPromise.data.result;
        }

        public static async UniTask<bool> ShowBannerAdNew(BannerLocation location = BannerLocation.bottom,
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
            if (UseOld)
            {
                return await HideBannerAdOld();
            }
            else
            {
                return await HideBannerAdNew();
            }
        }

        public static async UniTask<bool> HideBannerAdOld()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppHideBannerAd");
            return vkPromise.data.result;
        }

        public static async UniTask<bool> HideBannerAdNew()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppHideBannerAd");
            return vkData.result;
        }


        public static async UniTask<bool> CheckBannerAd()
        {
            if (UseOld)
            {
                return await CheckBannerAdOld();
            }
            else
            {
                return await CheckBannerAdNew();
            }
        }

        [Obsolete]
        public static async UniTask<bool> CheckBannerAdOld()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppCheckBannerAd");
            return vkPromise.data.result;
        }

        public static async UniTask<bool> CheckBannerAdNew()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCheckBannerAd");
            return vkData.result;
        }

        public static async UniTask<bool> RecommendApp()
        {
            if (UseOld)
            {
                return await RecommendAppOld();
            }
            else
            {
                return await RecommendAppNew();
            }
        }

        [Obsolete]
        public static async UniTask<bool> RecommendAppOld()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppRecommend");
            return vkPromise.data.result;
        }

        public static async UniTask<bool> RecommendAppNew()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppRecommend");
            return vkData.result;
        }

        public static async UniTask<bool> ShowLeaderBoard(int result = -1)
        {
            if (UseOld)
            {
                return await ShowLeaderBoardOld(result);
            }
            else
            {
                return await ShowLeaderBoardNew(result);
            }
        }


        public static async UniTask<bool> ShowLeaderBoardOld(int result = -1)
        {
            var methodParameters = result == -1
                ? null
                : new { user_result = result };

            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowLeaderBoardBox", methodParameters);

            return vkPromise.data.result;
        }

        public static async UniTask<bool> ShowLeaderBoardNew(int result = -1)
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
            string postAttachments,
            bool friendsOnly)
        {
            if (UseOld)
            {
                return await PublishPostOnWallOld(postMessage, postAttachments, friendsOnly);
            }
            else
            {
                return await PublishPostOnWallNew(postMessage, postAttachments, friendsOnly);
            }
        }

        public static async UniTask<bool> PublishPostOnWallOld(string postMessage,
            string postAttachments,
            bool friendsOnly)
        {
            var methodParameters = new
                { message = postMessage, attachments = postAttachments, friends_only = friendsOnly };

            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowWallPostBox", methodParameters);
            return vkPromise.data.result;
        }

        public static async UniTask<bool> PublishPostOnWallNew(string postMessage,
            string postAttachments,
            bool friendsOnly)
        {
            var methodParameters = new
                { message = postMessage, attachments = postAttachments, friends_only = friendsOnly };

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
            if (UseOld)
            {
                return await ShowOrderBoxOld(itemType, itemId);
            }
            else
            {
                return await ShowOrderBoxNew(itemType, itemId);
            }
        }

        [Obsolete]
        public static async UniTask<bool> ShowOrderBoxOld(string itemType, string itemId)
        {
            var methodParameters = new { type = itemType, item = itemId };

            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowOrderBox", methodParameters);
            return vkPromise.data.success;
        }

        public static async UniTask<bool> ShowOrderBoxNew(string itemType, string itemId)
        {
            var methodParameters = new { type = itemType, item = itemId };

            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKOrderData>("VKWebAppShowOrderBox", methodParameters);
            return vkPromise.success;
        }

        public enum SubscriptionAction
        {
            create,
            cancel,
            resume
        }

        public static async UniTask<bool> ShowSubscriptionBox(SubscriptionAction action,
            string item,
            string subscriptionId)
        {
            if (UseOld)
            {
                return await ShowSubscriptionBoxOld(action, item, subscriptionId);
            }
            else
            {
                var vkPromiseSubscriptionData = await ShowSubscriptionBoxNew(action, item, subscriptionId);
                return vkPromiseSubscriptionData.success;
            }
        }

        [Obsolete]
        public static async UniTask<bool> ShowSubscriptionBoxOld(SubscriptionAction action,
            string item,
            string subscriptionId)
        {
            var methodParameters = new
            {
                action = Enum.GetName(typeof(SubscriptionAction), action),
                item = item,
                subscription_id = subscriptionId
            };

            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowSubscriptionBox", methodParameters);
            return vkPromise.data.success;
        }

        public static async UniTask<VKSubscriptionData> ShowSubscriptionBoxNew(SubscriptionAction action,
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

        public static void AddEventListener(VKBridgeEventType eventType, Action<VKPromiseDataOLD> listener)
        {
            _eventManager.AddEventListener(eventType, listener);
        }

        public static void RemoveEventListener(VKBridgeEventType eventType, Action<VKPromiseDataOLD> listener)
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