using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spg
{
    /// <summary>
    /// 游戏运行时数据
    /// </summary>
    public class GameData : MonoSingleton<GameData>
    {
        public List<Event> Events { get; set; }
        public Player player { get; set; }
        public int CurrentGird { get; set; }
        public int DiceCount { get; set; }
        public int CurrentEvent { get; set; }
        public Dictionary<string, Buff> buff { get; set; }

        public void Init()
        {
            int count = RuntimeData.Instance.Conf.GirdCount - 2;
            Events = new List<Event>(count);
            buff = new Dictionary<string, Buff>();
            for (int i = 0; i < count; i++)
            {
                Events.Add(GetEvent());
            }

            player = GameObject.Find("Player").GetComponent<Player>();
            CurrentGird = 0;
            DiceCount = 0;
            CurrentEvent = -1;
        }

        private int moveStep = 0;

        private Event GetEvent()
        {
            if (moveStep > 0)
            {
                --moveStep;
            }

            if (RandomGenerator.Instance.GainIndex(WeightType.Base) == 0)
            {
                Event ev = new Event(RuntimeData.Instance.EventList[RandomGenerator.Instance.GainIndex(WeightType.Event)]);
                while (ev.Method.Equals("Move") && moveStep != 0)
                {
                    ev = new Event(RuntimeData.Instance.EventList[RandomGenerator.Instance.GainIndex(WeightType.Event)]);
                }
                if (ev.Method.Equals("Move"))
                {
                    ev.Args.TryGetValue("step", out string s);
                    moveStep = Mathf.Abs(int.Parse(s)) + 1;
                }
                ev.ShowMsg = $"{ev.Name}\n{ev.Desc}";
                ev.needHandle = true;
                return ev;
            }

            Event e = new Event(RuntimeData.Instance.SpEventList[RandomGenerator.Instance.GainIndex(WeightType.SpEvent)]);
            if (e.Args is null)
            {
                e.Args = new Dictionary<string, string>();
            }
            EventHandler.Instance.Execute(e);
            return e;
        }

        
    }
}