using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Vnn88.Common.Infrastructure
{
    /// <summary>
    /// Set and get an strongly type from session
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Set a object to session
        /// </summary>
        /// <typeparam name="T">Any object class</typeparam>
        /// <param name="session"></param>
        /// <param name="key">key use for indentity</param>
        /// <param name="value">value store to session</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Get an object of specific type from the session throught the key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
