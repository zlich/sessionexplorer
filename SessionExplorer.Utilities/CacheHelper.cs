using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace SessionExplorer.Utilities
{
    /// <summary>
    /// Helper class which wraps the ent lib caching framework and provides a simplified 
    /// generic interface for using the ent lib caching application block
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public static class CacheHelper<TObject>
    {
        #region Private Static Fields

        /// <summary>
        /// default timeout for cached items, defined in web.config
        /// </summary>
        private static readonly int defaultTimeout = int.Parse(ConfigurationManager.AppSettings["CacheTimeout"] ?? "20");
        /// <summary>
        /// IEqualityComparer which uses the default EqualityComparer for the typeparam type to determine 
        /// if the object returned from the cache is the types default (null for reference types)
        /// </summary>
        private static readonly IEqualityComparer comparer = EqualityComparer<TObject>.Default;
        /// <summary>
        /// static instance of the CacheManager class (.NET guarantees thread safety for static initialisation)
        /// </summary>
        private static readonly CacheManager cacheManager = CacheFactory.GetCacheManager();

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Determines whether the specified obj is the types default without the overhead of boxing.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// 	<c>true</c> if the specified obj is default; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDefault(TObject obj)
        {
            return comparer.Equals(obj, default(TObject));
        }

        /// <summary>
        /// Adds the specified obj to the cache using CacheItemPriority.Normal 
        /// and the pre-configured cache timeout value
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheObject">The cache object.</param>
        public static void Add(string cacheKey, TObject cacheObject)
        {
            Add(cacheKey, cacheObject, CacheItemPriority.Normal, defaultTimeout, null);
        }

        /// <summary>
        /// Adds the specified obj to the cache using CacheItemPriority.Normal
        /// and the pre-configured cache timeout value, accepts a type which
        /// implements the ICacheItemRefreshAction interface to allow callbacks
        /// when the object is removed from the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheObject">The cache object.</param>
        /// <param name="refreshAction">The refresh action.</param>
        public static void Add(string cacheKey, TObject cacheObject, ICacheItemRefreshAction refreshAction)
        {
            Add(cacheKey, cacheObject, CacheItemPriority.Normal, defaultTimeout, refreshAction);
        }

        /// <summary>
        /// Adds the specified obj to the cache using CacheItemPriority.Normal 
        /// with the timeout specified in minites
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheObject">The cache object.</param>
        /// <param name="timeoutMinutes">The cache item timeout in minutes.</param>
        public static void Add(string cacheKey, TObject cacheObject, int timeoutMinutes)
        {
            Add(cacheKey, cacheObject, CacheItemPriority.Normal, timeoutMinutes, null);
        }

        /// <summary>
        /// Adds the specified obj to the cache with the CacheItemPriority option
        /// and the pre-configured cache timeout value 
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheObject">The cache object.</param>
        /// <param name="priority">The CacheItemPriority.</param>
        public static void Add(string cacheKey, TObject cacheObject, CacheItemPriority priority)
        {
            Add(cacheKey, cacheObject, priority, defaultTimeout, null);
        }

        /// <summary>
        /// Adds the specified obj to the cache with the timeout specified in minites
        /// and CacheItemPriority option
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheObject">The cache object.</param>
        /// <param name="priority">The CacheItemPriority.</param>
        /// <param name="timeoutMinutes">The cache item timeout in minutes.</param>
        /// <param name="refreshAction">The refresh action.</param>
        public static void Add(string cacheKey, TObject cacheObject, CacheItemPriority priority, int timeoutMinutes, ICacheItemRefreshAction refreshAction)
        {
            //set up absolute expiration on the cache item using the timeoutMinutes specified
            AbsoluteTime absoluteExpiration = new AbsoluteTime(DateTime.Now.AddMinutes(timeoutMinutes));

            //trace the add to cache call
            Trace.Write(String.Format("[CacheHelper] - Adding object of type: {0} to the cache with key: {1}", typeof(TObject), cacheKey));

            //add to the cache
            cacheManager.Add(cacheKey, cacheObject, priority, refreshAction, absoluteExpiration);
        }

        /// <summary>
        /// Gets the specified object from the cache, returns the types
        /// default value if the item was not found in the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>TObject from cache or the cached types default</returns>
        public static TObject Get(string cacheKey)
        {
            try
            {
                TObject cacheObj = (TObject)cacheManager.GetData(cacheKey);

                //trace the results of the GetData call
                TraceCacheEvent(cacheObj, cacheKey);

                return cacheObj;
            }
            catch (NullReferenceException)
            {
                Trace.Write(String.Format("[CacheHelper] - Failed to retrieve value type: {0} from cache with key: {1}", typeof(TObject), cacheKey));

                //A null reference exception will be thrown if we attempt to retrieve
                //and cast a value type from the cache which does not exist, 
                //so catch that exception here and return the types default value;
                return default(TObject);
            }
        }

        /// <summary>
        /// Determines whether the specified cache key exists in the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified cache key]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(string cacheKey)
        {
            return cacheManager.Contains(cacheKey);
        }

        /// <summary>
        /// Removes the specified item from the cache. Does not perform any action if
        /// the item does not exist in the cache. There is therefore no need to check
        /// if the item exists before removing it.
        /// </summary>
        /// <param name="cacheKey">The cache key of the item to remove.</param>
        public static void Remove(string cacheKey)
        {
            cacheManager.Remove(cacheKey);
        }

        /// <summary>
        /// Removes all items from the cache, if an error occurs the cache is left unchanged
        /// </summary>
        public static void Flush()
        {
            cacheManager.Flush();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Traces the cache event.
        /// </summary>
        /// <param name="cacheObj">The cache obj.</param>
        /// <param name="cacheKey">The cache key.</param>
        private static void TraceCacheEvent(TObject cacheObj, string cacheKey)
        {
            if (IsDefault(cacheObj))
            {
                //only trace failure if we get here and the type retrieved is not a value type
                if (!typeof(TObject).IsValueType)
                {
                    Trace.Write(String.Format("[CacheHelper] - Failed to retrieve reference type: {0} from cache with key: {1}", typeof(TObject), cacheKey));
                }
                else
                {
                    Trace.Write(String.Format("[CacheHelper] - Successfully retrieved value type: {0} from cache with key: {1}", typeof(TObject), cacheKey));
                }
            }
            else
            {
                Trace.Write(String.Format("[CacheHelper] - Successfully retrieved type: {0} from cache with key: {1}", typeof(TObject), cacheKey));
            }
        }

        #endregion
    }
}
