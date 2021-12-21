using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spg
{
    public class Events : MonoSingleton<Events>
    {
        private void FinishHandle(string s)
        {
            foreach (var item in GameData.Instance.Effects)
            {
                if (item.Value.Count > 0)
                {
                    s += $"\n{item.Key}";
                    item.Value.Count--;
                }
            }

            EventManager.Instance.SendMsg(Consts.E_ShowMsg, s);
        }

        public void Sp(object obj)
        {
            string s = obj is null ? "" : obj.ToString();
            Event e = GameData.Instance.GetCurrentEvent();
            
            if (!e.IsInit)
            {
                SpPosture spPosture;
                SpTool spTool;
                if (e.Command.Equals(Consts.E_Otk))
                {
                    spPosture = RuntimeData.Instance.OtkPostures[RandomGenerator.Instance.GainIndex(WeightType.OtkPosture)];
                    spTool = RuntimeData.Instance.OtkTools[RandomGenerator.Instance.GainIndex(WeightType.OtkTool)];
                }
                else if (e.Command.Equals(Consts.E_Diy))
                {
                    spPosture = RuntimeData.Instance.DiyPostures[RandomGenerator.Instance.GainIndex(WeightType.DiyPosture)];
                    spTool = RuntimeData.Instance.DiyTools[RandomGenerator.Instance.GainIndex(WeightType.DiyTool)];
                }
                else
                {
                    spPosture = RuntimeData.Instance.SpPostures[RandomGenerator.Instance.GainIndex(WeightType.SpPosture)];
                    if (spPosture.IsOtk)
                    {
                        spTool = RuntimeData.Instance.OtkTools[RandomGenerator.Instance.GainIndex(WeightType.OtkTool)];
                    }
                    else
                    {
                        spTool = RuntimeData.Instance.SpTools[RandomGenerator.Instance.GainIndex(WeightType.SpTool)];
                    }
                }
                
                int num;
                if (e.Args.TryGetValue("Num", out string value) && int.TryParse(value, out int tmp))
                {
                    num = tmp;
                }
                else
                {
                    var random = new System.Random(Guid.NewGuid().GetHashCode());
                    num = random.Next(spTool.MinCount, spTool.MaxCount + 1);
                }
                e.Desc = e.Desc.Replace("_tool", spTool.ToString());
                e.Desc = e.Desc.Replace("_posture", spPosture.ToString());
                e.Desc = e.Desc.Replace("_num", num.ToString());
                e.ShowMsg = $"{e.Name}\n{e.Desc}";
                e.IsInit = true;
            }

            s += e.ShowMsg;
            FinishHandle(s);
        }

        public void Otk(object obj)
        {
            Sp(obj);
        }

        public void Diy(object obj)
        {
            Sp(obj);
        }

        public void Num(object obj)
        {
            string s = obj is null ? "" : obj.ToString();
            Event e = GameData.Instance.GetCurrentEvent();

            if (!e.IsInit)
            {
                int minNum = 1, maxNum = 50;
                if (e.Args.ContainsKey("Min"))
                {
                    int.TryParse(e.Args["Min"], out minNum);
                }
                if (e.Args.ContainsKey("Max"))
                {
                    int.TryParse(e.Args["Max"], out maxNum);
                }
                var random = new System.Random(Guid.NewGuid().GetHashCode());
                e.Desc = e.Desc.Replace("_num", random.Next(minNum, maxNum).ToString());
                e.ShowMsg = $"{e.Name}\n{e.Desc}";
                e.IsInit = true;
            }

            s += $"{e.Name}\n{e.Desc}";
            FinishHandle(s);
        }

        public void Move(object obj)
        {
            Event e = GameData.Instance.GetCurrentEvent();
            if (!e.IsInit)
            {
                e.ShowMsg = $"{e.Name}\n{e.Desc}";
                e.IsInit = true;
            }

            EventManager.Instance.SendMsg(Consts.E_ShowMsgAndMove, e);
        }

        public void BackStart(object obj)
        {
            Move(obj);
        }

        public void NoHandle(object obj)
        {
            Event e = GameData.Instance.GetCurrentEvent();
            if (!e.IsInit)
            {
                e.ShowMsg = $"{e.Name}\n{e.Desc}";
                e.IsInit = true;
            }

            FinishHandle(e.ShowMsg);
        }

        public void AddEffect(object obj)
        {
            Event e = GameData.Instance.GetCurrentEvent();
            if (!e.IsInit)
            {
                e.ShowMsg = $"{e.Name}\n{e.Desc}";
                if (e.Args.TryGetValue("Count", out string c) && e.Args.TryGetValue("Effect", out string s))
                {
                    if (GameData.Instance.Effects.ContainsKey(s))
                    {
                        GameData.Instance.Effects[s].Count += int.Parse(c);
                    }
                    else
                    {
                        GameData.Instance.Effects[s] = new Effect() { Count = int.Parse(c) };
                    }
                }
                else
                {
                    e.ShowMsg += "\n本事件参数有误！";
                }
            }
            EventManager.Instance.SendMsg(Consts.E_ShowMsg, e.ShowMsg);
        }
    }
}
