namespace VKBridgeSDK.Runtime.data
{
    public enum VKBridgeEventType // Имена инамов менять нельзя, они соответствуют именам ивентов в VK Bridge
    {
        Undefined,
        FocusChanged,
        VKWebAppAddToProfileResult,
        VKWebAppAddToProfileFailed,
        VKWebAppRecommendResult,
        VKWebAppRecommendFailed,
        VKWebAppShowWallPostBoxFailed,
        VKWebAppShowWallPostBoxResult,
        VKWebAppShowNativeAdsResult,
        VKWebAppShowNativeAdsFailed,
        VKWebAppShowBannerAdResult,
        VKWebAppShowBannerAdFailed,
        VKWebAppBannerAdUpdated,
        VKWebAppHideBannerAd,
        VKWebAppBannerAdClosedByUser,
        VKWebAppShowInviteBoxResult,
        VKWebAppShowInviteBoxFailed,
        VKWebAppShowLeaderBoardResult,
        VKWebAppShowLeaderBoardFailed,
        VKWebAppShowRequestBoxResult,
        VKWebAppShowRequestBoxFailed,
        VKWebAppGetFriendsResult,
        VKWebAppGetFriendsFailed,
        VKWebAppGetUserInfoResult,
        VKWebAppGetUserInfoFailed
    }
}