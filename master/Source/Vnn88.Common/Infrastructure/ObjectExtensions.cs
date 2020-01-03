using System.Collections.Generic;
using System.Linq;

namespace Vnn88.Common.Infrastructure
{
    /// <summary>
    /// Object extendsions
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Convert object value to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString<T>(this T obj)
        {
            return string.Join(" ", obj.GetType().GetProperties()
                .Select(x => x.GetValue(obj)?.ToString() ?? ""));
        }

        /// <summary>
        /// Convert object to dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary<T>(this T obj)
        {
            return obj.GetType().GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(obj)?.ToString() ?? "");
        }
    }
}
