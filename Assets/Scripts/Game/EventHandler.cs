using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Spg
{
    /// <summary>
    /// 事件处理类
    /// </summary>
    public class EventHandler : MonoSingleton<EventHandler>
    {
        private Dictionary<string, MethodInfo> MethodDict { get; set; }

        public void Init()
        {
            MethodDict = new Dictionary<string, MethodInfo>();
            Type t = Instance.GetType();
            MethodDict.Add("Sp", t.GetMethod(nameof(Sp)));
            MethodDict.Add("GainNumber", t.GetMethod(nameof(GainNumber)));
            MethodDict.Add("NoHandler", t.GetMethod(nameof(NoHandler)));
            MethodDict.Add("Move", t.GetMethod(nameof(Move)));
            MethodDict.Add("BackStart", t.GetMethod(nameof(BackStart)));
            MethodDict.Add("AddBuff", t.GetMethod(nameof(AddBuff)));
            MethodDict.Add("SpWithMethod", t.GetMethod(nameof(SpWithMethod)));
        }

        public void Execute(Event e)
        {
            MethodDict.TryGetValue(e.Method, out MethodInfo m);
            m?.Invoke(Instance, new object[] { e });

        }

        public void Sp(Event e)
        {
            if (!e.Args.TryGetValue("mode", out string s))
            {
                s = "sp";
            }
            SpTool tool;
            Position position;
            switch (s.ToLower())
            {
                case "diy":
                    tool = RuntimeData.Instance.DiyToolList[RandomGenerator.Instance.GainIndex(WeightType.DiyTool)];
                    position = RuntimeData.Instance.DiyPositionList[RandomGenerator.Instance.GainIndex(WeightType.DiyPosition)];
                    break;
                case "otk":
                    tool = RuntimeData.Instance.OtkToolList[RandomGenerator.Instance.GainIndex(WeightType.OtkTool)];
                    position = RuntimeData.Instance.OtkPositionList[RandomGenerator.Instance.GainIndex(WeightType.OtkPosition)];
                    break;
                default:
                    tool = RuntimeData.Instance.SpToolList[RandomGenerator.Instance.GainIndex(WeightType.SpTool)];
                    position = RuntimeData.Instance.SpPositionList[RandomGenerator.Instance.GainIndex(WeightType.SpPosition)];
                    break;
            }
            var random = new System.Random(Guid.NewGuid().GetHashCode());
            int num = random.Next(tool.MinCount, tool.MaxCount + 1);
            string toolMsg = tool.Name;
            string positionMsg = position.Name;
            if (tool.Desc != null && !tool.Desc.Equals(""))
            {
                toolMsg += $"({tool.Desc})";
            }
            if(position.Desc != null && !position.Desc.Equals(""))
            {
                positionMsg += $"({position.Desc})";
            }
            e.ShowMsg = $"{e.Name}\n{e.Desc.Replace("${tool}", toolMsg).Replace("${position}", positionMsg).Replace("${num}", $"{num}")}";
        }

        public void GainNumber(Event e)
        {
            Dictionary<string, string> value = e.Args;
            int min = value.ContainsKey("min") ? int.Parse(value["min"]) : 1;
            int max = value.ContainsKey("max") ? int.Parse(value["max"]) + 1 : 51;
            var random = new System.Random(Guid.NewGuid().GetHashCode());
            int count = random.Next(min, max);
            e.ShowMsg = $"{e.Name}\n{e.Desc.Replace("${num}", count.ToString())}";
        }

        public void NoHandler(Event e)
        {
            e.ShowMsg = $"{e.Name}\n{e.Desc}";
        }

        public void Move(Event e)
        {
            if (!e.Args.TryGetValue("step", out string s))
            {
                return;
            }
            int step = int.Parse(s);
            GameData.Instance.player.Move(step);
        }

        public void BackStart(Event e)
        {
            GameData.Instance.player.BackStart();
        }

        public void AddBuff(Event e)
        {
            if (!(e.Args.TryGetValue("count", out string c) && e.Args.TryGetValue("effect", out string s)))
            {
                return;
            }

            if (GameData.Instance.buff.ContainsKey(s))
            {
                GameData.Instance.buff[s].Count += int.Parse(c);
            }
            else
            {
                GameData.Instance.buff.Add(s, new Buff(s, int.Parse(c)));
            }
        }

        public void SpWithMethod(Event e)
        {
            Sp(e);
            PlayMethod method = RuntimeData.Instance.PlayMethodList[RandomGenerator.Instance.GainIndex(WeightType.PlayMethod)];
            e.ShowMsg += $"\n同时，本次惩罚你需要附加玩法{method.Name}";
            if (method.Desc != null && !method.Desc.Equals(""))
            {
                e.ShowMsg += $"({method.Desc})";
            }
        }
    }
}
