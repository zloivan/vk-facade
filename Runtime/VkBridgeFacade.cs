using System;
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

        public static async UniTask<VKPromise> GetUserInfo()
        {
            return await _vkResponseManager.CallVkMethodAsync("VKWebAppGetUserInfo");
        }

        public static async UniTask<VKPromise> CallAPIMethod(string methodName, string parameters)
        {
            return await _vkResponseManager.CallVkMethodAsync("VKWebAppCallAPIMethod",
                new { method = methodName, @params = parameters });
        }

        public static async UniTask<bool> VkBridgeInit()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppInit");
            return vkPromise.data.result;
        }

        public static async UniTask<bool> InviteFriend(string inviteRequestKey = null)
        {
            var methodParameters = string.IsNullOrEmpty(inviteRequestKey)
                ? null
                : new { requestKey = inviteRequestKey };

            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowInviteBox", methodParameters);

            return vkPromise.data.result;
        }


        public static async UniTask<bool> CheckNativeInterstitialAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppCheckNativeAds", new
            {
                ad_format = "interstitial",
                use_waterfall = useWaterfall
            });

            return vkPromise.data.result;
        }

        public static async UniTask<bool> CheckNativeRewardAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppCheckNativeAds", new
            {
                ad_format = "reward",
                use_waterfall = useWaterfall
            });

            return vkPromise.data.result;
        }

        public static async UniTask<bool> ShowNativeRewardAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowNativeAds", new
            {
                ad_format = "reward",
                use_waterfall = useWaterfall
            });

            return vkPromise.data.result;
        }

        public static async UniTask<bool> ShowNativeInterstitialAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowNativeAds", new
            {
                ad_format = "interstitial",
                use_waterfall = useWaterfall
            });

            return vkPromise.data.result;
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
                
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowBannerAd", parameters);
            return vkPromise.data.result;
        }
        
        public static async UniTask<bool> HideBannerAd()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppHideBannerAd");
            return vkPromise.data.result;
        }

        public static async UniTask<bool> CheckBannerAd()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppCheckBannerAd");
            return vkPromise.data.result;
        }

        public static async UniTask<bool> RecommendApp()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppRecommend");
            return vkPromise.data.result;
        }

        public static async UniTask<bool> ShowLeaderBoard(int result = -1)
        {
            var methodParameters = result == -1
                ? null
                : new { user_result = result };

            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowLeaderBoardBox", methodParameters);

            return vkPromise.data.result;
        }

        public static void ShowAlert(string message)
        {
            _vkResponseManager.ShowAlert(message);
        }

        public static void AddEventListener(VKBridgeEventType eventType, Action<VKPromiseData> listener)
        {
            _eventManager.AddEventListener(eventType, listener);
        }

        public static void RemoveEventListener(VKBridgeEventType eventType, Action<VKPromiseData> listener)
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