using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spg
{
    public class Events : MonoSingleton<Events>
    {
        public void Sp(object obj)
        {
            string s = obj is null ? "" : obj.ToString();
            Event e = GameData.Instance.GetCurrentEvent();
            
            if (!e.IsInit)
            {
                SpTool spTool = RuntimeData.Instance.SpTools[RandomGenerator.Instance.GainIndex(WeightType.SpTool)];
                SpPosture spPosture = RuntimeData.Instance.SpPostures[RandomGenerator.Instance.GainIndex(WeightType.SpPosture)];
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
                e.IsInit = true;
            }

            s += $"{e.Name}\n{e.Desc}";
            EventManager.Instance.SendMsg(Consts.E_ShowMsg, s);
        }

        public void Move(object obj)
        {
            Event e = GameData.Instance.GetCurrentEvent();
            if (e.Step != 0)
            {
                EventManager.Instance.SendMsg(Consts.E_PlayerRun, e.Step);
            }
        }
    }
}
