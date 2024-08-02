using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace vk_facade.Runtime.data
{
    [Serializable]
    public class VKPromiseResponse
    {
        public string method;

        [JsonProperty("data")]
        public JObject data;

        [JsonProperty("error")]
        public JObject error;

        public override string ToString()
        {
            return $"{nameof(method)}: {method}, {nameof(data)}: {data}";
        }
    }
}