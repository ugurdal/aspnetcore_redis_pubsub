using System;
using Newtonsoft.Json;

namespace redis_common.Common
{
    public static class Extensions
    {
        public static string FromJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T ToObject<T>(this string value) where T : class
        {
            try
            {
                return string.IsNullOrEmpty(value)
                    ? null
                    : JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}