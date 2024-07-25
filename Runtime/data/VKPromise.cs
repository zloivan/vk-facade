using System;

namespace VKBridgeSDK.Runtime.data
{
    [Serializable]
    public class VKPromise
    {
        public string method;
        public VKPromiseData _vkPromiseData;

        public override string ToString()
        {
            return $"{nameof(method)}: {method}, {nameof(_vkPromiseData)}: {_vkPromiseData}";
        }
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
        public VKPromiseData _vkPromiseData;
    }

    [Serializable]
    public class VKPromiseData
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

        public override string ToString()
        {
            return
                $"{nameof(result)}: {result}, {nameof(reason)}: {reason}, {nameof(banner_width)}: {banner_width}, {nameof(banner_height)}: {banner_height}, {nameof(banner_location)}: {banner_location}, {nameof(banner_align)}: {banner_align}, {nameof(orientation)}: {orientation}, {nameof(layout_type)}: {layout_type}, {nameof(success)}: {success}, {nameof(notSentIds)}: {notSentIds}, {nameof(requestKey)}: {requestKey}";
        }
    }

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