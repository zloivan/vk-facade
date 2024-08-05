using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace vk_facade.Runtime.data
{
    public class VKParams : IEnumerable<KeyValuePair<string, object>>
    {
        private Dictionary<string, object> Params { get; } = new Dictionary<string, object>();

        public void Add(string paramName, object paramValue)
        {
            Params[paramName] = paramValue;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Params.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Params.GetEnumerator();
        }

        public string GetParams()
        {
            if (Params.Count == 0) return "{}";
            
            
            var result = new JObject();
            
            foreach (var param in Params)
            {
                result[param.Key] = JToken.FromObject(param.Value);
            }

            return result.ToString();
        }
    }
}