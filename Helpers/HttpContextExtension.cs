using Newtonsoft.Json;
using ProjectAspEShop2024.BusinesLogic;

namespace ProjectAspEShop2024.Helpers
{
    public static class HttpContextExtension
    {
        /// <summary>
        /// Получить объект класса из сессии (метод расширения класса HttpContext)
        /// </summary>
        public static T? GetSubject<T>(this HttpContext httpContext)
            where T : class, new()
        {
            if (httpContext == null) 
                return null;

            T? subject = null;

            string key = typeof(T).FullName;
            // 1) ищем корзину в сессии
            string? subjectJson = httpContext.Session.GetString(key);
            // 2) если не нашли - создаём новую 
            if (String.IsNullOrEmpty(subjectJson))
                subject = new T();
            else
            {
                subject = JsonConvert.DeserializeObject<T>(subjectJson);
            }

            return subject;
        }

        public static void SaveSubject<T>(this HttpContext httpContext, T? subject)
            where T : class, new()
        {
            string subjectJson = JsonConvert.SerializeObject(subject);
            string key = typeof(T).FullName;
            httpContext.Session.SetString(key, subjectJson);
        }
    }
}
