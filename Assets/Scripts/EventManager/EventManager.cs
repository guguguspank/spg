using System;

namespace Spg
{
    public class EventManager : MonoSingleton<EventManager>
    {
        private EventBase<string, object> Handler = new EventBase<string, object>();

        public void AddListener(string enentType, Action<object> eventHandler)
        {
            Handler.AddListener(enentType, eventHandler);
        }

        public void RemoveListener(string enentType, Action<object> eventHandler)
        {
            Handler.RemoveListener(enentType, eventHandler);
        }

        public bool HasListener(string eventType)
        {
            return Handler.HasListener(eventType);
        }

        public void SendMsg(string eventType)
        {
            Handler.SendMessage(eventType, null);
        }

        public void SendMsg(string eventType, object eventArgs)
        {
            Handler.SendMessage(eventType, eventArgs);
        }

        public void Clear()
        {
            Handler.Clear();
        }
    }
}