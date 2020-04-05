using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nib.Career.Core.Extensions
{
    public static class ObjectConversionExtension
    {
        /// <summary>
        /// Serializes object to json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToString<T>(this T entity)
        {
            return JsonSerializer.Serialize(entity);
        }

        /// <summary>
        /// Deserialize json to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
