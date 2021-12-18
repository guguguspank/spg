using System;
using System.Collections.Generic;

namespace Spg
{
    /// <summary>
    /// 事件处理
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class EventBase<K, V>
    {
        /// <summary>
        /// 事件表
        /// </summary>
        private Dictionary<K, Action<V>> EventDict = new Dictionary<K, Action<V>>();

        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventHandler">事件处理</param>
        public void AddListener(K eventType, Action<V> eventHandler)
        {
            if (EventDict.TryGetValue(eventType, out Action<V> callbacks))
            {
                EventDict[eventType] = callbacks + eventHandler;
            }
            else
            {
                EventDict.Add(eventType, eventHandler);
            }
        }

        /// <summary>
        /// 移除监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventHandler">事件处理</param>
        public void RemoveListener(K eventType, Action<V> eventHandler)
        {
            if (EventDict.TryGetValue(eventType, out Action<V> callbacks))
            {
                callbacks = (Action<V>)Delegate.RemoveAll(callbacks, eventHandler);
                if (callbacks == null)
                {
                    EventDict.Remove(eventType);
                }
                else
                {
                    EventDict[eventType] = callbacks;
                }
            }
        }

        /// <summary>
        /// 返回是否包含事件监听器
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public bool HasListener(K eventType)
        {
            return EventDict.ContainsKey(eventType);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventArg">参数</param>
        public void SendMessage(K eventType, V eventArg)
        {
            if (EventDict.TryGetValue(eventType, out Action<V> callbacks))
            {
                callbacks.Invoke(eventArg);
            }
        }

        /// <summary>
        /// 清空所有事件监听器
        /// </summary>
        public void Clear()
        {
            EventDict.Clear();
        }
    }
}