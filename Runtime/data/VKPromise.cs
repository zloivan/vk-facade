using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VKBridgeSDK.Runtime.data
{
    [Serializable]
    public class VKPromise
    {
        public string method;
        public VKPromiseDataOLD data;

        public override string ToString()
        {
            return $"{nameof(method)}: {method}, {nameof(data)}: {data}";
        }
    }

    public class VKPromiseResponse
    {
        public string method;
        public string data;
    }

    public class VKPromise<T> : IVKPromise where T : VKData
    {
        public UniTaskCompletionSource<T> CompletionSource { get; set; } = new UniTaskCompletionSource<T>();

        public void SetResult(string jsonData)
        {
            var data = JsonUtility.FromJson<T>(jsonData);
            CompletionSource.TrySetResult(data);
        }
        
        public void SetException(string jsonExceptionData)
        {
            var error = JsonUtility.FromJson<VKError>(jsonExceptionData);
            
            var data = error.data.error_data;
            var errorMessage = !string.IsNullOrEmpty(data.error_description) ? data.error_description :
                !string.IsNullOrEmpty(data.error_msg) ? data.error_msg : data.error_reason;
            
            CompletionSource.TrySetException(new Exception(errorMessage));
        }
    }

    public interface IVKPromise
    {
        void SetResult(string jsonData);
        void SetException(string jsonExceptionData);
    }


    public abstract class VKData
    {
    }

    [Serializable]
    public class VKRequestData : VKData
    {
        public bool result;
    }
    
    [Serializable]
    public class VKOrderData : VKData
    {
        public bool success;
    }
    [Serializable]
    public class VKSubscriptionData : VKData
    {
        public bool success;
        public int subscriptionId;
    }
    
    [Serializable]
    public class VKFriendsData : VKData// Ето не я ебанутый, это в вк так сделали
    {
        public VKUserData[] users;
    }
    
    [Serializable]
    public class VKUsersData : VKData // Ето не я ебанутый, это в вк так сделали
    {
        public VKUserData[] result;
    }

    [Serializable]
    public class VKEvent
    {
        public Detail detail;
    }

    [Serializable]
    public class Detail
    {
        public string type;
        public VKPromiseDataOLD _vkPromiseData;
    }

    [Serializable][Obsolete]
    public class VKPromiseDataOLD : VKData
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
        public int post_id;
        public VKUserData[] users;
        

        public override string ToString()
        {
            return
                $"{nameof(result)}: {result}, {nameof(reason)}: {reason}, {nameof(banner_width)}: {banner_width}, {nameof(banner_height)}: {banner_height}, {nameof(banner_location)}: {banner_location}, {nameof(banner_align)}: {banner_align}, {nameof(orientation)}: {orientation}, {nameof(layout_type)}: {layout_type}, {nameof(success)}: {success}, {nameof(notSentIds)}: {notSentIds}, {nameof(requestKey)}: {requestKey}";
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
            return $"{nameof(id)}: {id}, {nameof(first_name)}: {first_name}, {nameof(last_name)}: {last_name}, {nameof(sex)}: {sex}, {nameof(photo_200)}: {photo_200}, {nameof(city)}: {city}, {nameof(country)}: {country}";
        }
    }

    [Serializable]
    public class VKCityData
    {
        public int id;
        public string title;
        
        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(title)}: {title}";
        }
    }

    [Serializable]
    public class VKCountryData
    {
        public int id;
        public string title;
        
        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(title)}: {title}";
        }
    }

    [Obsolete]
    public class VkPromiseError
    {
        public string method;
        public VKError error;
    }

    [Serializable]
    public class VKError
    {
        public string type;
        public Data data;

        [Serializable]
        public class Data
        {
            public string error_type;
            public ErrorData error_data;
            public int request_id;
        }

        [Serializable]
        public class ErrorData
        {
            public int error_code;
            public string error_reason;
            public string error_description;
            public string error_msg;
            public int error;
        }
    }
}