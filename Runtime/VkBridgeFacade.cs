using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using vk_facade.Runtime.components;
using vk_facade.Runtime.data;
using vk_facade.Runtime.helpers;
using vk_facade.Runtime.managers;
using Object = UnityEngine.Object;

namespace vk_facade.Runtime
{
    public static class VkBridgeFacade
    {
        private static VKResponseManager _vkResponseManager;
        private static VKEventManager _eventManager;
        private static GameObject _messageReceiverObject;
        private static GameObject _vKMenuGameObject;
        private static VKMessageReceiver _vkMessageReceiver;
        private static VKUrlManager _urlManager;
        private static VKStorageManager _storageManager;
        private static ILogger _logger;

        public static VKStorageManager Storage => _storageManager;

        /// <summary>
        /// Инициализация Фасада, вызвать в самом начале, до использования любого из методов
        /// </summary>
        public static void Initialize()
        {
            InitializeAsync().Forget();
        }

        /// <summary>
        /// Инициализация Фасада, вызвать в самом начале, до использования любого из методов
        /// </summary>
        public static async UniTask InitializeAsync()
        {
            _logger = new VKBridgeLogger();
            _logger.Log("BRIDGE_FACADE", "Initializing VkBridgeFacade...");
            _vkResponseManager = new VKResponseManager();
            _logger.Log("BRIDGE_FACADE", "Created: VKResponseManager...");
            _eventManager = new VKEventManager();
            _logger.Log("BRIDGE_FACADE", "Created: VKEventManager...");
            _messageReceiverObject = new GameObject("VKMessageReceiver");
            _vkMessageReceiver = _messageReceiverObject.AddComponent<VKMessageReceiver>();
            _logger.Log("BRIDGE_FACADE", "Created: VKMessageReceiver...");
            _vkMessageReceiver.Initialize(_vkResponseManager, _eventManager);
            _urlManager = new VKUrlManager();
            _logger.Log("BRIDGE_FACADE", "Created: VKUrlManager...");
            _storageManager = new VKStorageManager();
            _logger.Log("BRIDGE_FACADE", "Created: VKStorageManager");

            Object.DontDestroyOnLoad(_messageReceiverObject);

            SpawnDebugMenu();

            await VkBridgeInit();
            await _storageManager.Load();
        }

        private static void SpawnDebugMenu()
        {
            _vKMenuGameObject = new GameObject("VKDebugMenu");
            _vKMenuGameObject.AddComponent<VKBridgeDebugMenu>();
            Object.DontDestroyOnLoad(_vKMenuGameObject);
            _vKMenuGameObject.SetActive(false);
        }

        /// <summary>
        /// Call custom request to VKBridge
        /// </summary>
        /// <param name="methodName">Method name of VKBridge sdk </param>
        /// <param name="parameters">This is must be object type, follow strictly to VkBridge documentaion and params that are descirbed there for each method. Check parsing is correct</param>
        /// <typeparam name="T">Pick the child of VkData that is applicable to method call</typeparam>
        /// <returns></returns>
        /// /// <remarks>
        /// To learn more about how to use this method, please refer to the <a href="https://dev.vk.com/ru/bridge/overview">VKBridge Documentation</a>.
        /// </remarks>
        public static async UniTask<T> CallCustomRequest<T>(string methodName, VKParams parameters) where T : VKData
        {
            return await _vkResponseManager.CallVkMethodAsync<T>(methodName, parameters);
        }

        public static async UniTask<VKRequestData> CallAPIMethod(string methodName, string parameters)
        {
            return await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCallAPIMethod",
                new VKParams
                {
                    { "method", methodName },
                    { "params", parameters }
                });
        }

        /// <summary>
        /// Инициализация VKBridge. <b>!Обязательно вызвать!</b>
        /// </summary>
        /// <returns>Успешно ли прошел запрос</returns>
        public static async UniTask<bool> VkBridgeInit()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppInit");
            return vkData.result;
        }

        /// <summary>
        /// Показать панель приглашения друзей
        /// </summary>
        /// <param name="inviteRequestKey"></param>
        /// <returns>Успешно ли прошел запрос</returns>
        public static async UniTask<bool> InviteFriend(string inviteRequestKey = null)
        {
            var vkPromiseData =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowInviteBox",
                    new VKParams
                    {
                        { "requestKey", inviteRequestKey }
                    });

            return vkPromiseData.result;
        }

        /// <summary>
        /// Проверить готов ли интер к показу, и запустить загрузку если не готов
        /// </summary>
        /// <param name="useWaterfall">Если интер не готов, стоил ли использовать реворд</param>
        /// <returns>True если готов к показу</returns>
        public static async UniTask<bool> CheckNativeInterstitialAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCheckNativeAds",
                new VKParams
                {
                    { "ad_format", "interstitial" },
                    { "use_waterfall", useWaterfall }
                });

            _logger.Log("BRIDGE_FACADE", $"CheckNativeInterstitialAd got result: {vkPromise.result}");
            return vkPromise.result;
        }

        /// <summary>
        /// Проверить готов ли реворд к показу, и запустить загрузку если не готов
        /// </summary>
        /// <returns>True если готов к показу</returns>
        public static async UniTask<bool> CheckNativeRewardAd()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCheckNativeAds",
                new VKParams
                {
                    { "ad_format", "reward" }
                });

            _logger.Log("BRIDGE_FACADE", $"CheckNativeRewardAd got result: {vkPromise.result}");
            return vkPromise.result;
        }

        /// <summary>
        /// Показать реворд, если он готов. <b>!!ВОЗВРАТ ИЗ МЕТОДА ПРОИЗОЙДЕТ ПО ОКОНЧАНИЮ РЕВОРДА!!</b> 
        /// </summary>
        /// <returns>Досмотрел ли до конца</returns>
        public static async UniTask<bool> ShowNativeRewardAd()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowNativeAds",
                new VKParams
                {
                    { "ad_format", "reward" }
                });

            _logger.Log("BRIDGE_FACADE", $"ShowNativeRewardAd got result: {vkPromise.result}");
            return vkPromise.result;
        }

        public static async UniTask<bool> ShowNativeInterstitialAd(bool useWaterfall = true)
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowNativeAds",
                new VKParams
                {
                    { "ad_format", "interstitial" },
                    { "use_waterfall", useWaterfall }
                });

            _logger.Log("BRIDGE_FACADE", $"ShowNativeInterstitialAd got result: {vkPromise.result}");
            return vkPromise.result;
        }

        /// <summary>
        /// Показать баннер
        /// </summary>
        /// <param name="location"></param>
        /// <param name="align"></param>
        /// <param name="orientation"></param>
        /// <param name="layout"></param>
        /// <returns>Успешно ли прошел запрос</returns>
        public static async UniTask<bool> ShowBannerAd(BannerLocation location = BannerLocation.bottom,
            BannerAlign align = BannerAlign.center,
            BannerOrientation orientation = BannerOrientation.horizontal,
            BannerLayout layout = BannerLayout.resize)
        {
            var parameters = new VKParams
            {
                { "banner_location", Enum.GetName(typeof(BannerLocation), location) },
                { "banner_align", Enum.GetName(typeof(BannerAlign), align) },
                { "banner_orientation", Enum.GetName(typeof(BannerOrientation), orientation) },
                { "banner_layout", Enum.GetName(typeof(BannerLayout), layout) }
            };

            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowBannerAd", parameters);

            _logger.Log("BRIDGE_FACADE", $"ShowBannerAd got result: {vkPromise.result}");
            return vkPromise.result;
        }

        /// <summary>
        /// Спрятать банер
        /// </summary>
        /// <returns>Успешно ли прошел запрос</returns>
        public static async UniTask<bool> HideBannerAd()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppHideBannerAd");

            _logger.Log("BRIDGE_FACADE", $"HideBannerAd got result: {vkData.result}");
            return vkData.result;
        }

        /// <summary>
        /// Проверить показывается ли банер
        /// </summary>
        /// <returns>Показываем или нет</returns>
        public static async UniTask<bool> CheckBannerAd()
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppCheckBannerAd");

            _logger.Log("BRIDGE_FACADE", $"CheckBannerAd got result: {vkData.result}");
            return vkData.result;
        }

        /// <summary>
        /// Порекомендовать приложние (пока не видел успешно что бы отрабатывало, может быть есть ограничения для приложений
        /// со стороны ВК по колличеству друзей, или статусу приложений, но надежнее использовать PublishPostOnWall)
        /// </summary>
        /// <returns>Успешно ли прошел запрос</returns>
        public static async UniTask<bool> RecommendApp()
        {
            VKRequestData vkData = await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppRecommend");

            _logger.Log("BRIDGE_FACADE", $"RecommendApp got result: {vkData.result}");
            return vkData.result;
        }

        /// <summary>
        /// Показать окно лидерборда
        /// </summary>
        /// <param name="result">Если не отправлять результат, покажет тещий счет игрока, иначе поменяет на новый</param>
        /// <returns>Успешно ли прошел запрос</returns>
        public static async UniTask<bool> ShowLeaderBoard(int? result = null)
        {
            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowLeaderBoardBox",
                    new VKParams
                    {
                        { "user_result", result }
                    });

            _logger.Log("BRIDGE_FACADE", $"ShowLeaderBoard got result: {vkPromise.result}");
            return vkPromise.result;
        }


        /// <summary>
        /// Показать окно публикации на стену ВК
        /// </summary>
        /// <param name="postMessage">Сообщение публикации</param>
        /// <param name="postAttachments">Приложение к сообщению</param>
        /// <param name="friendsOnly">Опубликовать только для друзей</param>
        /// <returns>Успешно ли прошел запрос</returns>
        public static async UniTask<bool> PublishPostOnWall(string postMessage,
            string postAttachments = null,
            bool friendsOnly = true)
        {
            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppShowWallPostBox", new VKParams
                {
                    { "message", postMessage },
                    { "friends_only", friendsOnly },
                    { "attachments", postAttachments }
                });

            _logger.Log("BRIDGE_FACADE", $"ShowPostOnWall got result: {vkPromise.result}");
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
            var vkPromise =
                await _vkResponseManager.CallVkMethodAsync<VKOrderData>("VKWebAppShowOrderBox", new VKParams
                {
                    { "type", itemType },
                    { "item", itemId }
                });

            _logger.Log("BRIDGE_FACADE", $"ShowOrderBox got result: {vkPromise.success}");
            return vkPromise.success;
        }

        public static async UniTask<VKSubscriptionData> ShowSubscriptionBox(SubscriptionAction action,
            string subscriptionItem,
            string subscriptionId)
        {
            var vkData = await _vkResponseManager.CallVkMethodAsync<VKSubscriptionData>("VKWebAppShowSubscriptionBox",
                    new VKParams
                    {
                        { "action", Enum.GetName(typeof(SubscriptionAction), action) },
                        { "item", subscriptionItem },
                        { "subscription_id", subscriptionId }
                    });

            _logger.Log("BRIDGE_FACADE", $"ShowSubscriptionBox got result: {vkData}");
            return vkData;
        }

        /// <summary>
        /// Показывает окно выбора друзей из списка и получает информацию о них.
        /// </summary>
        /// <param name="isMultiSelect">Информация о том, выбрать ли нескольких друзей из списка или одного. </param>
        /// <returns></returns>
        public static async UniTask<VKFriendsData> GetFriendList(bool isMultiSelect)
        {
            var methodParameters = new VKParams
            {
                { "multi", isMultiSelect }
            };

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKFriendsData>("VKWebAppGetFriends", methodParameters);

            _logger.Log("BRIDGE_FACADE", $"GetFriendList got result: {vkData}");
            return vkData;
        }

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Если ID пусто, вернет информацию по текущему пользователю</returns>
        public static async UniTask<VKUserData> GetUserData(int? userId = null)
        {
            var methodParameters = new VKParams();


            if (userId != null) methodParameters.Add("user_id", userId);

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKUserData>("VKWebAppGetUserInfo", methodParameters);

            _logger.Log("BRIDGE_FACADE", $"GetUserData got result: {vkData}");
            return vkData;
        }

        public static void ShowAlert(string message)
        {
            _vkResponseManager.ShowAlert(message);
        }

        public static void AddEventListener(VKBridgeEventType eventType, Action<VKEventData> listener)
        {
            _eventManager?.AddEventListener(eventType, listener);
        }

        public static void RemoveEventListener(VKBridgeEventType eventType, Action<VKEventData> listener)
        {
            _eventManager?.RemoveEventListener(eventType, listener);
        }

        /// <summary>
        /// Этот метод недоступен в играх, только в мини-приложениях
        /// </summary>
        /// <returns></returns>
        public static async UniTask<VKLaunchParams> GetLaunchParamsViaVkBridge()
        {
            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKLaunchParams>("VKWebAppGetLaunchParams");

            _logger.Log("BRIDGE_FACADE", $"GetLaunchParams got result: {vkData}");
            return vkData;
        }

        public static VKLaunchParams GetLaunchParams()
        {
            var vkLaunchParams = _urlManager.GetLaunchParams();

            _logger.Log("BRIDGE_FACADE", $"GetLaunchParams got result: {vkLaunchParams}");
            return vkLaunchParams;
        }

        /// <summary>
        /// Получить код языка пользователя <a href="https://dev.vk.com/ru/api/api-requests#%D0%9E%D0%B1%D1%89%D0%B8%D0%B5%20%D0%BF%D0%B0%D1%80%D0%B0%D0%BC%D0%B5%D1%82%D1%80%D1%8B">Общие параметры</a>
        /// </summary>
        /// <returns>0=>"ru", 1 =>"uk", 2 => "be", 3 =>"en", 4 =>"es", 5 =>"fi", 6 =>"de",7 =>"it", _ =>"en"</returns>
        public static string GetLanguageCode()
        {
            var result = GetLaunchParams();

            _logger.Log("BRIDGE_FACADE", $"GetLanguageCode got result: {result}");
            return LaunchParamsHelper.ConvertToLanguage(result.Language);
        }

        /// <summary>
        /// Получить набор значений по ключам
        /// </summary>
        /// <param name="parameters">Список ключей по которым нужно вернуть значения</param>
        /// <returns>Список ключ - значение, обернутые в <see cref="VKStorageData"/></returns>
        internal static async UniTask<VKStorageData> StorageGet(params string[] parameters)
        {
            var vkParams = new VKParams
            {
                { "keys", parameters }
            };

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKStorageData>("VKWebAppStorageGet", vkParams);

            _logger.Log("BRIDGE_FACADE", $"StorageGet got result: {vkData}");
            return vkData;
        }

        /// <summary>
        /// Записывает новое значение по ключу.
        /// </summary>
        /// <param name="key">Ключ по которому забирается значение</param>
        /// <param name="value">Если тут пустая строка, тогда значение удаляется из хранилища</param>
        /// <returns>Успешный ли запрос</returns>
        internal static async UniTask<bool> StorageSet(string key, string value)
        {
            _logger.Log("BRIDGE_FACADE", $"StorageSet method called with [{key},{value}]...");

            var vkParams = new VKParams
            {
                { "key", key },
                { "value", value }
            };

            _logger.Log("BRIDGE_FACADE", $"StorageSet params: {vkParams.GetParams()}");

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKRequestData>("VKWebAppStorageSet", vkParams);

            _logger.Log("BRIDGE_FACADE", $"StorageSet got result: {vkData.result}");
            return vkData.result;
        }

        internal static async UniTask<VKStorageKeys> StorageGetKeys(int numKeyNamesToGet, int offset)
        {
            var vkParams = new VKParams
            {
                { "count", numKeyNamesToGet },
                { "offset", offset }
            };

            var vkData =
                await _vkResponseManager.CallVkMethodAsync<VKStorageKeys>("VKWebAppStorageGetKeys", vkParams);

            _logger.Log("BRIDGE_FACADE", $"StorageGetKeys got result: {vkData}");
            return vkData;
        }

        /// <summary>
        /// Used to reset static fields in Editor
        /// </summary>
        public static void ResetStaticFields()
        {
            if (_messageReceiverObject != null)
            {
                Object.DestroyImmediate(_messageReceiverObject);
            }

            if (_vKMenuGameObject != null)
            {
                Object.DestroyImmediate(_vKMenuGameObject);
            }

            _vkResponseManager = null;
            _eventManager = null;
            _messageReceiverObject = null;
            _vKMenuGameObject = null;
            _vkMessageReceiver = null;
            _urlManager = null;
            _storageManager = null;
            _logger = new VKBridgeLogger();
        }

        /// <summary>
        /// Method enable debug menu
        /// </summary>
        public static void ShowDebug()
        {
            _vKMenuGameObject.SetActive(true);
        }
    }
}