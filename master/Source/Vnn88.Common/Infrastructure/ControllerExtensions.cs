using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Vnn88.Common.Infrastructure
{
    /// <summary>
    /// Extend controller functional
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Puts an object into the TempData by first serializing it to JSON.
        /// </summary>
        /// <typeparam name="T">Class for Serialize</typeparam>
        /// <param name="tempData"></param>
        /// <param name="key">key used for identidy</param>
        /// <param name="value">Value which want to store</param>
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Gets an object from the TempData by deserializing it from JSON.
        /// </summary>
        /// <typeparam name="T">Class for Deserialize</typeparam>
        /// <param name="tempData"></param>
        /// <param name="key">key for indentify</param>
        /// <returns>Object store in tempData</returns>
        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            tempData.TryGetValue(key, out var o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }

        /// <summary>
        /// Utilities for add message throught views
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message">Message for sending</param>
        /// <param name="type">Enum for indetify type of message</param>
        public static void AddMessage(this Controller controller, string message, NotifyType type = NotifyType.Info)
        {
            string key = nameof(NotifyMessage);
            NotifyMessage notifyMessage = new NotifyMessage
            {
                Message = message,
                NotifyType = type
            };       
            controller.TempData.Put(key, notifyMessage);  
        }

        public static IActionResult SuccessResult(this Controller controller, string message)
        {
            return controller.Json(new JsonMessage
            {
                Result = true,
                Message = message
            });
        }

        public static IActionResult ErrorResult(this Controller controller, string message)
        {
            return controller.Json(new JsonMessage
            {
                Result = false,
                Message = message
            });
        }
    }

    public class JsonMessage
    {
        [JsonProperty(PropertyName = "result")]
        public bool Result { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
