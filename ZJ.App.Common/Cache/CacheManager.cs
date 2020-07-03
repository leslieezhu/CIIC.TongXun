using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Configuration;

namespace ZJ.App.Common.Cache
{
    public class CacheManager
    {
        /// <summary>
        /// Cache名称
        /// </summary>
        protected string _cacheName = string.Empty;

        public CacheManager(string cacheName) 
        {
            _cacheName = cacheName;
        }

        /// <summary>
        /// 当前Cache
        /// </summary>
        protected ICacheManager CurrentCache
        {
            get
            {
                if (!string.IsNullOrEmpty(_cacheName))
                {
                    return CacheFactory.GetCacheManager(this._cacheName);
                }
                else
                {
                    return CacheFactory.GetCacheManager();
                }
            }
        }

        public virtual void Flush()
        {
                CurrentCache.Flush();
        }

        public bool ExistsCacheData(string key)
        {
            return CurrentCache.Contains(key) && CurrentCache.GetData(key) != null;
        }

        public object GetCacheData(string key)
        {
            return CurrentCache.GetData(key);
        }

        public void AddCache(string key, object value)
        {
            CurrentCache.Add(key, value, CacheItemPriority.Normal, null, CacheExtendedFormatTime);
        }

        //public void AddCache<T>(string key, T TEntity)
        //{
        //    CurrentCache.Add(key, TEntity);
        //}

        public void RemoveCache(string key)
        {
            CurrentCache.Remove(key);
        }

        public void AddCache(string key, object value, ExtendedFormatTime expirations)
        {
            CurrentCache.Add(key, value, CacheItemPriority.Normal, null, expirations);
        }

        public void AddCache<T>(string key, T TEntity, ExtendedFormatTime expirations)
        {
            CurrentCache.Add(key, TEntity, CacheItemPriority.Normal, null, expirations);
        }

        public ExtendedFormatTime CacheExtendedFormatTime
        {
            get { return new ExtendedFormatTime(ConfigurationManager.AppSettings["CacheExtendedFormatTime"]); }
        }
    }
}
