using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace vk_facade.Runtime.data
{
    [Serializable]
    public class VKPromiseResponse
    {
        [JsonProperty("method")]
        public string method;
        
        [JsonProperty("requestId")]
        public string requestId ;

        [JsonProperty("data")]
        public JObject data;

        [JsonProperty("error")]
        public JObject error;
        
        public override string ToString()
        {
            return
                $"{nameof(method)}: {method}, {nameof(requestId)}: {requestId}, {nameof(data)}: {data}, {nameof(error)}: {error}";
        }
    }
}