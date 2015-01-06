﻿using System;

namespace Cache
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = false,Inherited = false)]
    public class CachingAttribute:Attribute
    {

        /// <summary>
        /// 初始化一个新的<c>CachingAttribute</c>类型。
        /// </summary>
        /// <param name="method">缓存方式。</param>
        public CachingAttribute(CachingMethod method)
        {
            Method = method;
        }
        /// <summary>
        /// 初始化一个新的<c>CachingAttribute</c>类型。
        /// </summary>
        /// <param name="method">缓存方式。</param>
        /// <param name="correspondingMethodNames">
        /// 与当前缓存方式相关的方法名称。注：此参数仅在缓存方式为Remove时起作用。
        /// </param>
        public CachingAttribute(CachingMethod method, params string[] correspondingMethodNames)
            : this(method)
        {
            CorrespondingMethodNames = correspondingMethodNames;
        }
        #region Public Properties
        /// <summary>
        /// 获取或设置缓存方式。
        /// </summary>
        public CachingMethod Method { get; set; }
        /// <summary>
        /// 获取或设置一个<see cref="Boolean"/>值，该值表示当缓存方式为Put时，是否强制将值写入缓存中。
        /// </summary>
        public bool Force { get; set; }
        /// <summary>
        /// 获取或设置与当前缓存方式相关的方法名称。注：此参数仅在缓存方式为Remove时起作用。
        /// </summary>
        public string[] CorrespondingMethodNames { get; set; }
        #endregion
    }
}
