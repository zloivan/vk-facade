using System;

namespace VKBridgeSDK.Runtime.data
{
    public abstract class VKData
    {
    }

    [Serializable]
    public class VKRequestData : VKData
    {
        public bool result;

        public override string ToString()
        {
            return $"{nameof(result)}: {result}";
        }
    }

    [Serializable]
    public class VKOrderData : VKData
    {
        public bool success;

        public override string ToString()
        {
            return $"{nameof(success)}: {success}";
        }
    }

    [Serializable]
    public class VKLaunchParams : VKData
    {
        public string sign;// = "Hb67aIL4cElWINenspCpKu3tUgacikw541NCXX8zWL4",
        public string vk_access_token_settings;//= "",
        public int vk_app_id;//: 8142709,
        public int vk_are_notifications_enabled;//: 0,
        public int vk_is_app_user;//: 1,
        public int vk_is_favorite;//: 0,
        public string vk_language;//= "ru",
        public string vk_platform;//= "desktop_web",
        public string vk_ref;// "other",
        public int vk_ts;//: 1664886146,
        public int vk_user_id; //: 82156740
    }

    [Serializable]
    public class VKSubscriptionData : VKData
    {
        public bool success;
        public int subscriptionId;

        public override string ToString()
        {
            return $"{nameof(success)}: {success}, {nameof(subscriptionId)}: {subscriptionId}";
        }
    }

    [Serializable]
    public class VKFriendsData : VKData // Ето не я ебанутый, это в вк так сделали
    {
        public VKUserData[] users;

        public override string ToString()
        {
            return $"{nameof(users)}: {users}";
        }
    }

    [Serializable]
    public class VKUsersData : VKData // Ето не я ебанутый, это в вк так сделали
    {
        public VKUserData[] result;
    }

    [Serializable]
    public class VKEvent : VKData
    {
        public Detail detail;

        public override string ToString()
        {
            return $"{nameof(detail)}: {detail}";
        }
    }

    [Serializable]
    public class Detail : VKData
    {
        public string type;
        public VKEventData data;

        public override string ToString()
        {
            return $"{nameof(type)}: {type}, {nameof(data)}: {data}";
        }
    }

    [Serializable]
    public class VKEventData : VKData
    {
        public bool result;
        public string reason;
        public int banner_width;
        public int banner_height;
        public string banner_location;
        public string banner_align;
        public string orientation;
        public string layout_type;
        public bool success;
        public string[] notSentIds;
        public string requestKey;
        public string app_id;
        public string api_host;
        public int post_id;

        public string error_type;
        public VKError.ErrorData error_data;
        public VKUserData[] users;


        public override string ToString()
        {
            return
                $"{nameof(result)}: {result}, {nameof(reason)}: {reason}, {nameof(banner_width)}: {banner_width}, {nameof(banner_height)}: {banner_height}, {nameof(banner_location)}: {banner_location}, {nameof(banner_align)}: {banner_align}, {nameof(orientation)}: {orientation}, {nameof(layout_type)}: {layout_type}, {nameof(success)}: {success}, {nameof(notSentIds)}: {notSentIds}, {nameof(requestKey)}: {requestKey}, {nameof(app_id)}: {app_id}, {nameof(api_host)}: {api_host}, {nameof(post_id)}: {post_id}, {nameof(error_type)}: {error_type}, {nameof(error_data)}: {error_data}, {nameof(users)}: {users}";
        }
    }

    [Serializable]
    public class VKUserData : VKData
    {
        public int id;
        public string first_name;
        public string last_name;
        public int sex;
        public string photo_200;
        public VKCityData city;
        public VKCountryData country;

        public override string ToString()
        {
            return
                $"{nameof(id)}: {id}, {nameof(first_name)}: {first_name}, {nameof(last_name)}: {last_name}, {nameof(sex)}: {sex}, {nameof(photo_200)}: {photo_200}, {nameof(city)}: {city}, {nameof(country)}: {country}";
        }
    }

    [Serializable]
    public class VKCityData : VKData
    {
        public int id;
        public string title;

        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(title)}: {title}";
        }
    }

    [Serializable]
    public class VKCountryData : VKData
    {
        public int id;
        public string title;

        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(title)}: {title}";
        }
    }

    [Serializable]
    public class VKError : VKData
    {
        public string type;
        public Data data;

        public override string ToString()
        {
            return $"{nameof(type)}: {type}, {nameof(data)}: {data}";
        }
        
        [Serializable]
        public class Data
        {
            public string error_type;
            public ErrorData error_data;
            public int request_id;

            public override string ToString()
            {
                return
                    $"{nameof(error_type)}: {error_type}, {nameof(error_data)}: {error_data}, {nameof(request_id)}: {request_id}";
            }
        }

        [Serializable]
        public class ErrorData
        {
            public int error_code;
            public string error_reason;
            public string error_description;
            public string error_msg;
            public int error;

            public override string ToString()
            {
                return
                    $"{nameof(error_code)}: {error_code}, {nameof(error_reason)}: {error_reason}, {nameof(error_description)}: {error_description}, {nameof(error_msg)}: {error_msg}, {nameof(error)}: {error}";
            }
        }
    }

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

    public enum SubscriptionAction
    {
        create,
        cancel,
        resume
    }
}