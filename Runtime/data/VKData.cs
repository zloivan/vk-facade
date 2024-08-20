using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace vk_facade.Runtime.data
{
    public abstract class VKData
    {
        public abstract bool IsValid { get; }
    }

    [Serializable]
    public class VKRequestData : VKData
    {
        public bool result;

        public override string ToString()
        {
            return $"{nameof(result)}: {result}";
        }

        public override bool IsValid => true;
    }

    [Serializable]
    public class VKOrderData : VKData
    {
        public bool success;

        public override string ToString()
        {
            return $"{nameof(success)}: {success}";
        }

        public override bool IsValid => true;
    }

    [Serializable]
    public class VKSubscriptionData : VKData
    {
        public bool success;
        public int subscriptionId = -1;

        public override string ToString()
        {
            return $"{nameof(success)}: {success}, {nameof(subscriptionId)}: {subscriptionId}";
        }

        public override bool IsValid => subscriptionId != -1;
    }

    [Serializable]
    public class VKBannerData : VKData
    {
        [JsonProperty("result")]
        public bool result;
        [JsonProperty("banner_width")]        
        public int banner_width;
        [JsonProperty("banner_height")]   
        public int banner_height;
        [JsonProperty("banner_location")]
        public string banner_location;
        [JsonProperty("banner_align")]
        public string banner_align;
        [JsonProperty("height_type")]
        public string height_type;
        [JsonProperty("orientation")]
        public string orientation;
        [JsonProperty("layout_type")]
        public string layout_type;
        
        [JsonIgnore]
        public override bool IsValid => banner_width != 0 && banner_height != 0;
    }

    [Serializable]
    public class VKFriendsData : VKData // Ето не я ебанутый, это в вк так сделали
    {
        public VKUserData[] users;

        public override string ToString()
        {
            return $"{nameof(users)}: {users}";
        }

        public override bool IsValid => users != null && users.Length > 0;
    }

    [Serializable]
    public class VKUsersData : VKData // Ето не я ебанутый, это в вк так сделали
    {
        public VKUserData[] result;
        public override bool IsValid => result != null && result.Length > 0;
    }

    [Serializable]
    public class VKEvent : VKData
    {
        public Detail detail;

        public override string ToString()
        {
            return $"{nameof(detail)}: {detail}";
        }

        public override bool IsValid => detail != null && detail.IsValid;
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

        public override bool IsValid => !string.IsNullOrEmpty(type) && data != null && data.IsValid;
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

        public override bool IsValid => true; // Any field of that class can be filled or be empty
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

        public override bool IsValid =>
            id != 0
            && !string.IsNullOrEmpty(first_name)
            && !string.IsNullOrEmpty(last_name)
            && city != null
            && country != null;
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

        public override bool IsValid => !string.IsNullOrEmpty(title);
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

        public override bool IsValid => !string.IsNullOrEmpty(title);
    }

    [Serializable]
    public class VKError : VKData
    {
        [JsonProperty("error_type")]public string error_type;
        [JsonProperty("error_data")]public ErrorData error_data;
        [JsonProperty("request_id")]public int request_id;

        public override bool IsValid => error_data != null && !string.IsNullOrEmpty(error_type);
        public override string ToString()
        {
            return
                $"{nameof(error_type)}: {error_type}, {nameof(error_data)}: {error_data}, {nameof(request_id)}: {request_id}";
        }
            
        [Serializable]
        public class ErrorData
        {
            [JsonProperty("error_code")]public int error_code;
            [JsonProperty("error_reason")]public string error_reason;
            [JsonProperty("error_description")]public string error_description;
            [JsonProperty("error_msg")]public string error_msg;
            [JsonProperty("error")]public int error;

            public override string ToString()
            {
                return
                    $"{nameof(error_code)}: {error_code}, {nameof(error_reason)}: {error_reason}, {nameof(error_description)}: {error_description}, {nameof(error_msg)}: {error_msg}, {nameof(error)}: {error}";
            }
        }
    }

    [Serializable]
    public class VKStorageData : VKData
    {
        [JsonProperty("keys")]
        public List<VKStorageKeyValue> keys;

        public override bool IsValid => keys != null && keys.Count > 0;
        private Dictionary<string, string> _keysDictionary;

        private Dictionary<string, string> InitializeDictionary()
        {
            var result = new Dictionary<string, string>();
            foreach (var kv in keys)
            {
                result[kv.key] = kv.value;
            }

            return result;
        }

        public string this[string key]
        {
            get
            {
                if (_keysDictionary == null)
                {
                    _keysDictionary =  InitializeDictionary();
                }

                return _keysDictionary.TryGetValue(key, out var value) ? value : null;
            }
        }

        [Serializable]
        public class VKStorageKeyValue
        {
            public string key;
            public string value;
        }
    }

    [Serializable]
    public class VKStorageKeys : VKData
    {
        [JsonProperty("keys")]
        public string[] keys;

        public override bool IsValid => keys != null;
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