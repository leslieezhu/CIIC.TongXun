using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace ZJ.App.Common.Cache
{
    public sealed class CacheHelper
    {
        /// <summary>
        /// 获取CacheManager实例
        /// <para>默认为Default</para>
        /// </summary>
        /// <returns></returns>
        public static CacheManager GetManagerInstance()
        {
            return GetManagerInstance(string.Empty);
        }
        /// <summary>
        /// 获取CacheManager实例
        /// </summary>
        /// <param name="CacheManagerName">缓存名称，存于CacheConst</param>
        /// <returns></returns>
        public static CacheManager GetManagerInstance(string CacheManagerName)
        {
            return new CacheManager(CacheManagerName);
        }
      
    }
}
