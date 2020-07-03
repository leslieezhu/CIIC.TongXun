using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Configuration;

namespace ZJ.App.Common
{
    public abstract class CacheBase
    {
          /// <summary>
        /// Cache名称
        /// </summary>
        protected string _cacheName = string.Empty;

        public CacheBase() { }

        protected bool IsCacheUsed
        {
            get
            {
                return  !string.IsNullOrEmpty(_cacheName);
            }
        }

        /// <summary>
        /// 当前Cache
        /// </summary>
        protected ICacheManager CurrentCache
        {
            get
            {
                return CacheFactory.GetCacheManager(this._cacheName);
            }
        }

        protected virtual void Flush()
        {
            if (IsCacheUsed)
            {
                CurrentCache.Flush();
            }
        }

        protected bool ExistsCacheData(string key)
        {
            return CurrentCache.Contains(key) && CurrentCache.GetData(key) != null;
        }

        protected object GetCacheData(string key)
        {
            return CurrentCache.GetData(key);
        }

        protected void AddCache(string key, object value)
        {
            CurrentCache.Add(key, value);
        }

        protected void RemoveCache(string key)
        {
            CurrentCache.Remove(key);
        }

        protected void AddCache(string key, object value, ExtendedFormatTime expirations)
        {
            CurrentCache.Add(key, value, CacheItemPriority.Normal, null, expirations);
        }

        protected ExtendedFormatTime CacheExtendedFormatTime
        {
            get { return new ExtendedFormatTime(ConfigurationManager.AppSettings["CacheExtendedFormatTime"]); }
        }
    }
}
