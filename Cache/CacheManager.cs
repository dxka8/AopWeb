using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class CacheManager : ICacheProvider
    {

        public ICacheProvider CacheProvider = new EntLibCacheProvider();
        // ReSharper disable once InconsistentNaming
        private CacheManager instance = new CacheManager();
        static CacheManager() { }

        #region 公共属性
        /// <summary>
        /// 获取<c>CacheManager</c>类型的单件（Singleton）实例。
        /// </summary>
        public  CacheManager Instance
        {
            get { return instance; }
        }
        #endregion


        public void Add(string key, string valKey, object value)
        {
            CacheProvider.Add(key, valKey, value);
        }

        public void Put(string key, string valKey, object value)
        {
            CacheProvider.Put(key, valKey, value);
        }

        public object Get(string key, string valKey)
        {
            return CacheProvider.Get(key, valKey);
        }

        public void Remove(string key)
        {
            CacheProvider.Remove(key);
        }

        public bool Exists(string key)
        {
            return CacheProvider.Exists(key);
        }

        public bool Exists(string key, string valKey)
        {
            return CacheProvider.Exists(key, valKey);
        }
    }
}
