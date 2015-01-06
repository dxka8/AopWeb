using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Cache
{
    public class CachingBehavior : IInterceptionBehavior
    {
        /// <summary>
        /// 根据指定的<see cref="CachingAttribute"/>以及<see cref="IMethodInvocation"/>实例，
        /// 获取与某一特定参数值相关的键名。
        /// </summary>
        /// <param name="cachingAttribute"><see cref="CachingAttribute"/>实例。</param>
        /// <param name="input"><see cref="IMethodInvocation"/>实例。</param>
        /// <returns>与某一特定参数值相关的键名。
        ///   <remarks>
        ///    例如：<see cref="ICacheProvider.Add"/>
        ///   </remarks>
        /// </returns>
        private string GetValueKey(CachingAttribute cachingAttribute, IMethodInvocation input)
        {
            switch (cachingAttribute.Method)
            {
                case CachingMethod.Remove:
                    return null;
                case CachingMethod.Get:
                case CachingMethod.Put:
                    if (input.Arguments != null && input.Arguments.Count > 0)
                    {
                        var sb = new StringBuilder();
                        for (int i = 0; i < input.Arguments.Count; i++)
                        {
                            sb.Append(input.Arguments[i].ToString());
                            if (i != input.Arguments.Count - 1)
                                sb.Append("_");
                        }
                        return sb.ToString();
                    }
                    else
                    {
                        return "Null";
                    }
                default:
                    throw new InvalidOperationException("无效的缓存方式。");
            }
        }

        /// <summary>
        /// 获取当前行为需要拦截的对象类型接口。
        /// </summary>
        /// <returns>所有需要拦截的对象类型接口。</returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// 通过实现此方法来拦截调用并执行所需的拦截行为。
        /// </summary>
        /// <param name="input">调用拦截目标时的输入信息。</param>
        /// <param name="getNext">通过行为链来获取下一个拦截行为的委托。</param>
        /// <returns>从拦截目标获得的返回信息。</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var method = input.MethodBase;
            var key = method.Name;
            if (method.IsDefined(typeof(CachingAttribute), false))
            {
                var cachingAttribute = (CachingAttribute)method.GetCustomAttributes(typeof(CachingAttribute), false)[0];
                var valKey = GetValueKey(cachingAttribute, input);
                switch (cachingAttribute.Method)
                {
                    case CachingMethod.Get:
                        //try
                        {
                            if (new CacheManager().Instance.Exists(key, valKey))
                            {
                                var obj = new CacheManager().Instance.Get(key, valKey);
                                var arguments = new object[input.Arguments.Count];
                                input.Arguments.CopyTo(arguments, 0);
                                return new VirtualMethodReturn(input, obj, arguments);
                            }
                            var methodReturn = getNext().Invoke(input, getNext);
                            if (methodReturn.Exception == null)
                            {
                                new CacheManager().Instance.Add(key, valKey, methodReturn.ReturnValue);
                            }
                            return methodReturn;
                        }
                        //catch (Exception ex)
                        //{
                        //    return new VirtualMethodReturn(input, ex);
                        //}
                    case CachingMethod.Put:
                        try
                        {
                            var methodReturn = getNext().Invoke(input, getNext);
                            if (new CacheManager().Instance.Exists(key))
                            {
                                if (cachingAttribute.Force)
                                {
                                    new CacheManager().Instance.Remove(key);
                                    new CacheManager().Instance.Add(key, valKey, methodReturn.ReturnValue);
                                }
                                else
                                    new CacheManager().Instance.Put(key, valKey, methodReturn.ReturnValue);
                            }
                            else
                                new CacheManager().Instance.Add(key, valKey, methodReturn.ReturnValue);
                            return methodReturn;
                        }
                        catch (Exception ex)
                        {
                            return new VirtualMethodReturn(input, ex);
                        }
                    case CachingMethod.Remove:
                        try
                        {
                            var removeKeys = cachingAttribute.CorrespondingMethodNames;
                            foreach (var removeKey in removeKeys)
                            {
                                if (new CacheManager().Instance.Exists(removeKey))
                                    new CacheManager().Instance.Remove(removeKey);
                            }
                            var methodReturn = getNext().Invoke(input, getNext);
                            return methodReturn;
                        }
                        catch (Exception ex)
                        {
                            return new VirtualMethodReturn(input, ex);
                        }
                }
            }

            return getNext().Invoke(input, getNext);
        }

        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前拦截行为被调用时，是否真的需要执行
        /// 某些操作。
        /// </summary>
        public bool WillExecute
        {
            get { return true; }
        }
    }
}
