using System;

namespace VKBridgeSDK.Runtime.data
{
    [Serializable]
    public class VKLaunchParams : VKData
    {
        public string AccessToken { get; set; }
        public string AccessTokenSettings { get; set; }
        public string AdsAppId { get; set; }
        public int ApiId { get; set; }
        public string ApiResult { get; set; }
        public int ApiSettings { get; set; }
        public string ApiUrl { get; set; }
        public string AuthKey { get; set; }
        public int GroupId { get; set; }
        public string GroupWhitelistScope { get; set; }
        public string Hash { get; set; }
        public int IsAppUser { get; set; }
        public int IsFavorite { get; set; }
        public int IsPlayMachine { get; set; }
        public int IsSecure { get; set; }
        public int IsWidescreen { get; set; }
        public int Language { get; set; }
        public string LcName { get; set; }
        public int OdrEnabled { get; set; }
        public int ParentLanguage { get; set; }
        public string Platform { get; set; }
        public int RefNotificationId { get; set; }
        public string RefNotificationType { get; set; }
        public string Referrer { get; set; }
        public string RequestKey { get; set; }
        public string Secret { get; set; }
        public string Sid { get; set; }
        public string Sign { get; set; }
        public string SignKeys { get; set; }
        public string StatsHash { get; set; }
        public long Timestamp { get; set; }
        public int UserId { get; set; }
        public int ViewerId { get; set; }
        public int ViewerType { get; set; }
        public string WhitelistScopes { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(AccessToken)}: {AccessToken}, {nameof(AccessTokenSettings)}: {AccessTokenSettings}, {nameof(AdsAppId)}: {AdsAppId}, {nameof(ApiId)}: {ApiId}, {nameof(ApiResult)}: {ApiResult}, {nameof(ApiSettings)}: {ApiSettings}, {nameof(ApiUrl)}: {ApiUrl}, {nameof(AuthKey)}: {AuthKey}, {nameof(GroupId)}: {GroupId}, {nameof(GroupWhitelistScope)}: {GroupWhitelistScope}, {nameof(Hash)}: {Hash}, {nameof(IsAppUser)}: {IsAppUser}, {nameof(IsFavorite)}: {IsFavorite}, {nameof(IsPlayMachine)}: {IsPlayMachine}, {nameof(IsSecure)}: {IsSecure}, {nameof(IsWidescreen)}: {IsWidescreen}, {nameof(Language)}: {Language}, {nameof(LcName)}: {LcName}, {nameof(OdrEnabled)}: {OdrEnabled}, {nameof(ParentLanguage)}: {ParentLanguage}, {nameof(Platform)}: {Platform}, {nameof(RefNotificationId)}: {RefNotificationId}, {nameof(RefNotificationType)}: {RefNotificationType}, {nameof(Referrer)}: {Referrer}, {nameof(RequestKey)}: {RequestKey}, {nameof(Secret)}: {Secret}, {nameof(Sid)}: {Sid}, {nameof(Sign)}: {Sign}, {nameof(SignKeys)}: {SignKeys}, {nameof(StatsHash)}: {StatsHash}, {nameof(Timestamp)}: {Timestamp}, {nameof(UserId)}: {UserId}, {nameof(ViewerId)}: {ViewerId}, {nameof(ViewerType)}: {ViewerType}, {nameof(WhitelistScopes)}: {WhitelistScopes}";
        }
    }
}