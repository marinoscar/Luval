using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public static class ObjectCacheProvider
    {
        private static Dictionary<string, IClearableCacheItems> _cacheCollection;

        public static CacheItems<TKey, TValue> GetProvider<TKey, TValue>(string bucketName)
        {
            CreateIfRequired();
            if (_cacheCollection.ContainsKey(bucketName))
                return (CacheItems<TKey, TValue>)_cacheCollection[bucketName];
            var result = new CacheItems<TKey, TValue>(bucketName);
            _cacheCollection.Add(bucketName, result);
            return result;
        }

        public static void ClearProvider(string name)
        {
            CreateIfRequired();
            if (!_cacheCollection.ContainsKey(name)) return;
            _cacheCollection[name].Clear();
        }

        public static void ClearAll()
        {
            _cacheCollection = null;
        }

        private static void CreateIfRequired()
        {
            if (_cacheCollection == null) _cacheCollection = new Dictionary<string, IClearableCacheItems>();
        }
    }

    public interface IClearableCacheItems
    {
        void Clear();
    }

    public class CacheItems<TKey, TValue> : IClearableCacheItems
    {
        public ICacheStorageProvider<TKey, CacheItemContainer<TValue>> Internal { get; private set; }

        public CacheItems(string bucketName)
        {
            Internal = new DictionaryCacheStorage<TKey, CacheItemContainer<TValue>>(bucketName);
            BucketName = bucketName;
        }

        public string BucketName { get; private set; }

        public void Clear()
        {
            Internal.Clear();
        }

        /// <summary>
        /// Gets the item requested from the cache
        /// </summary>
        /// <param name="key">The key of the cache item</param>
        /// <exception cref="http://msdn.microsoft.com/en-us/library/system.argumentnullexception(v=vs.110).aspx">ArgumentNullException </exception>
        public TValue GetCacheItem(TKey key)
        {
            return GetCacheItem(key, null);
        }

        /// <summary>
        /// Gets the item requested from the cache
        /// </summary>
        /// <param name="key">The key of the cache item</param>
        /// <param name="functionToGetValue">A function to use in case the item is not in cache and requires to be stored</param>
        public TValue GetCacheItem(TKey key, Func<TKey, TValue> functionToGetValue)
        {
            return GetCacheItem(key, functionToGetValue, DateTime.MaxValue);
        }

        /// <summary>
        /// Gets the item requested from the cache
        /// </summary>
        /// <param name="key">The key of the cache item</param>
        /// <param name="functionToGetValue">A function to use in case the item is not in cache and requires to be stored</param>
        /// <param name="expiration">The timespan when the item expire</param>
        public TValue GetCacheItem(TKey key, Func<TKey, TValue> functionToGetValue, TimeSpan expiration)
        {
            var utcExpiredBy = DateTime.MaxValue;
            if (expiration.TotalMilliseconds > 0) utcExpiredBy = DateTime.UtcNow.Add(expiration);
            return GetCacheItem(key, functionToGetValue, utcExpiredBy);
        }

        /// <summary>
        /// Gets the item requested from the cache
        /// </summary>
        /// <param name="key">The key of the cache item</param>
        /// <param name="functionToGetValue">A function to use in case the item is not in cache and requires to be stored</param>
        /// <param name="utcExpiredBy">The utc time in which the item expires</param>
        public TValue GetCacheItem(TKey key, Func<TKey, TValue> functionToGetValue, DateTime utcExpiredBy)
        {
            if (Internal.ContainsKey(key))
            {
                var itemContainer = Internal[key];
                itemContainer.LookedUpCount++;
                itemContainer.UtcLastAccesedOn = DateTime.UtcNow;
                if(itemContainer.UtcLastAccesedOn > itemContainer.UtcExpireOn)
                {
                    return GetValueFromFunction(key, functionToGetValue, utcExpiredBy);
                }
                return itemContainer.Item;
            }
            return GetValueFromFunction(key, functionToGetValue, utcExpiredBy);
        }

        private TValue GetValueFromFunction(TKey key, Func<TKey, TValue> func, DateTime utcExpiredBy)
        {
            if (func == null)
                throw new ArgumentNullException("The key {0} is not cached and no function was provided to properly store the value in cache".Fi(key));
            var value = func(key);
            SetCacheItem(key, value, utcExpiredBy);
            return value;
        }

        public void RemoveFromCache(TKey key)
        {
            Internal.Remove(key);
        }

        /// <summary>
        /// Sets the item in the cache
        /// </summary>
        /// <param name="key">Item Key</param>
        /// <param name="value">Item Value</param>
        public void SetCacheItem(TKey key, TValue value)
        {
            SetCacheItem(key, value, DateTime.MaxValue);
        }

        /// <summary>
        /// Sets the item in the cache
        /// </summary>
        /// <param name="key">Item Key</param>
        /// <param name="value">Item Value</param>
        /// <param name="expiration"></param>
        public void SetCacheItem(TKey key, TValue value, TimeSpan expiration)
        {
            var utcExpiration = DateTime.MaxValue;
            if (expiration.TotalMilliseconds > 0) utcExpiration = DateTime.UtcNow.Add(expiration);
            SetCacheItem(key,value, utcExpiration);
        }

        /// <summary>
        /// Sets the item in the cache
        /// </summary>
        /// <param name="key">Item Key</param>
        /// <param name="value">Item Value</param>
        /// <param name="utcExpiredBy">The expiration date of the item</param>
        public void SetCacheItem(TKey key, TValue value, DateTime utcExpiredBy)
        {
            Internal[key] = new CacheItemContainer<TValue>(value) { UtcExpireOn = utcExpiredBy };
        }

        public bool ContainsKey(TKey key)
        {
            return Internal.ContainsKey(key);
        }

    }

    public class CacheItemContainer<TValue>
    {

        public CacheItemContainer(TValue item)
        {
            Item = item;
            LookedUpCount = 0;
            Size = 0;
            UtcCreatedOn = DateTime.UtcNow;
            UtcLastAccesedOn = UtcCreatedOn;
            UtcExpireOn = DateTime.MaxValue;
        }

        public TValue Item { get; set; }
        public uint LookedUpCount { get; set; }
        public ulong Size { get; set; }
        public DateTime UtcCreatedOn { get; set; }
        public DateTime UtcLastAccesedOn { get; set; }
        public DateTime UtcExpireOn { get; set; }
    }

    public interface ICacheStorageProvider<TKey, TValue>
    {

        string BucketName { get; }
        bool ContainsKey(TKey key);
        void Add(TKey key, TValue value);
        TValue this[TKey key] { get; set; }
        void Remove(TKey key);
        void Clear();
    }

    public class DictionaryCacheStorage<TKey, TValue> : ICacheStorageProvider<TKey, TValue>
    {
        protected IDictionary<TKey, TValue> Internal { get; private set; }

        public DictionaryCacheStorage(string bucketName)
        {
            Internal = new Dictionary<TKey, TValue>();
            BucketName = bucketName;
        }

        public string BucketName { get; private set; }

        public bool ContainsKey(TKey key)
        {
            return Internal.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            Internal.Add(key, value);
        }

        public TValue this[TKey key]
        {
            get { return Internal[key]; }
            set { Internal[key] = value; }
        }

        public void Remove(TKey key)
        {
            Internal.Remove(key);
        }

        public void Clear()
        {
            Internal.Clear();
        }
    }
}
