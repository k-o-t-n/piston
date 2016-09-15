namespace Piston.Storage
{
    using System;
    using System.Web;

    public static class CacheHelper
    {
        public static T GetOrCache<T>(string key, Func<T> accessor)
        {
            if (HttpRuntime.Cache[key] == null)
            {
                var value = accessor();
                HttpRuntime.Cache.Insert(key, value);
            }

            if (HttpRuntime.Cache[key] != null)
            {
                return (T)HttpRuntime.Cache[key];
            }

            return default(T);
        }
    }
}