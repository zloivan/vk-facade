using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using vk_facade.Runtime.data;
using vk_facade.Runtime.helpers;
using ILogger = vk_facade.Runtime.helpers.ILogger;

namespace vk_facade.Runtime.managers
{
    public class VKUrlManager
    {
        [DllImport("__Internal")]
        private static extern IntPtr UnityVKBridge_GetWindowLocationHref();

        private readonly ILogger _logger = new VKBridgeLogger();

        public VKLaunchParams GetLaunchParams()
        {
            if (Application.isEditor)
                return new VKLaunchParams();
            
            _logger.Log("GetLaunchParams called...");
            var url = Marshal.PtrToStringAnsi(UnityVKBridge_GetWindowLocationHref());
            Assert.IsNotNull(url, "URL, could not be null!");

            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            _logger.Log($"Got url: {url}");

            var uri = new Uri(url);
            _logger.Log($"Resulting uri: {uri}");

            var queryString = uri.Query;
            var queryDictionary = ParseQueryString(queryString);
            return PopulateLaunchParams(queryDictionary);
        }

        private Dictionary<string, string> ParseQueryString(string queryString)
        {
            _logger.Log("ParseQueryString called...");
            var queryDictionary = new Dictionary<string, string>();
            _logger.Log($"Query string: {queryString}");
            if (string.IsNullOrEmpty(queryString))
                return queryDictionary;

            var querySegments = queryString.TrimStart('?').Split('&');

            foreach (var segment in querySegments)
            {
                _logger.Log($"Segment: {segment}");
                var parts = segment.Split('=');
                var partsNotEmpty = parts.Length > 1;
                _logger.Log($"Parts not empty: {partsNotEmpty}");

                if (partsNotEmpty)
                {
                    queryDictionary[parts[0]] = Uri.UnescapeDataString(parts[1]);

                    _logger.Log($"Query dictionary[{parts[0]}]: {queryDictionary[parts[0]]}");
                }
                else
                {
                    queryDictionary[parts[0]] = string.Empty;
                }
            }

            return queryDictionary;
        }

        private VKLaunchParams PopulateLaunchParams(Dictionary<string, string> queryDictionary)
        {
            var launchParams = new VKLaunchParams();

            foreach (var kvp in queryDictionary)
            {
                switch (kvp.Key)
                {
                    case "access_token":
                        launchParams.AccessToken = kvp.Value;
                        break;
                    case "access_token_settings":
                        launchParams.AccessTokenSettings = kvp.Value;
                        break;
                    case "ads_app_id":
                        launchParams.AdsAppId = kvp.Value;
                        break;
                    case "api_id":
                        launchParams.ApiId = int.Parse(kvp.Value);
                        break;
                    case "api_result":
                        launchParams.ApiResult = kvp.Value;
                        break;
                    case "api_settings":
                        launchParams.ApiSettings = int.Parse(kvp.Value);
                        break;
                    case "api_url":
                        launchParams.ApiUrl = kvp.Value;
                        break;
                    case "auth_key":
                        launchParams.AuthKey = kvp.Value;
                        break;
                    case "group_id":
                        launchParams.GroupId = int.Parse(kvp.Value);
                        break;
                    case "group_whitelist_scope":
                        launchParams.GroupWhitelistScope = kvp.Value;
                        break;
                    case "hash":
                        launchParams.Hash = kvp.Value;
                        break;
                    case "is_app_user":
                        launchParams.IsAppUser = int.Parse(kvp.Value);
                        break;
                    case "is_favorite":
                        launchParams.IsFavorite = int.Parse(kvp.Value);
                        break;
                    case "is_play_machine":
                        launchParams.IsPlayMachine = int.Parse(kvp.Value);
                        break;
                    case "is_secure":
                        launchParams.IsSecure = int.Parse(kvp.Value);
                        break;
                    case "is_widescreen":
                        launchParams.IsWidescreen = int.Parse(kvp.Value);
                        break;
                    case "language":
                        launchParams.Language = int.Parse(kvp.Value);
                        break;
                    case "lc_name":
                        launchParams.LcName = kvp.Value;
                        break;
                    case "odr_enabled":
                        launchParams.OdrEnabled = int.Parse(kvp.Value);
                        break;
                    case "parent_language":
                        launchParams.ParentLanguage = int.Parse(kvp.Value);
                        break;
                    case "platform":
                        launchParams.Platform = kvp.Value;
                        break;
                    case "ref_notification_id":
                        launchParams.RefNotificationId = int.Parse(kvp.Value);
                        break;
                    case "ref_notification_type":
                        launchParams.RefNotificationType = kvp.Value;
                        break;
                    case "referrer":
                        launchParams.Referrer = kvp.Value;
                        break;
                    case "request_key":
                        launchParams.RequestKey = kvp.Value;
                        break;
                    case "secret":
                        launchParams.Secret = kvp.Value;
                        break;
                    case "sid":
                        launchParams.Sid = kvp.Value;
                        break;
                    case "sign":
                        launchParams.Sign = kvp.Value;
                        break;
                    case "sign_keys":
                        launchParams.SignKeys = kvp.Value;
                        break;
                    case "stats_hash":
                        launchParams.StatsHash = kvp.Value;
                        break;
                    case "timestamp":
                        launchParams.Timestamp = long.Parse(kvp.Value);
                        break;
                    case "user_id":
                        launchParams.UserId = int.Parse(kvp.Value);
                        break;
                    case "viewer_id":
                        launchParams.ViewerId = int.Parse(kvp.Value);
                        break;
                    case "viewer_type":
                        launchParams.ViewerType = int.Parse(kvp.Value);
                        break;
                    case "whitelist_scopes":
                        launchParams.WhitelistScopes = kvp.Value;
                        break;
                }
            }

            Debug.Assert(launchParams.IsValid, "Invalid launch parameters, not parsed correctly!");
            _logger.Log($"Populated launch params: {launchParams}");

            return launchParams;
        }
    }
}