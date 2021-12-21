using System.Collections.Generic;
using UnityEngine;

namespace Spg
{
    /// <summary>
    /// 飞行棋数据
    /// </summary>
    public class GameData : MonoSingleton<GameData>
    {
        private Dictionary<int, int> MoveStep = new Dictionary<int, int>();
        private int CurrentGird;
        private int GirdCount;

        public List<Gird> Girds { get; set; }
        public Player player;

        public Dictionary<string, Effect> Effects;


        public void Init()
        {
            Effects = new Dictionary<string, Effect>();
            player = GameObject.Find("Player").GetComponent<Player>();
            CurrentGird = 0;

            InitGird();

            
        }

        /// <summary>
        /// 用于角色移动时获取下一格，同时会自动更新当前位置到移动后位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetNextGird()
        {
            if (CurrentGird < GirdCount)
            {
                CurrentGird++;
            }
            return Girds[CurrentGird].Pos;
        }

        /// <summary>
        /// 用于角色移动时获取上一格，同时会自动更新当前位置到移动后位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPreGird()
        {
            if (CurrentGird > 0)
            {
                CurrentGird--;
            }
            return Girds[CurrentGird].Pos;
        }

        public Vector3 GetStartGird()
        {
            CurrentGird = 0;
            return Girds[0].Pos;
        }

        /// <summary>
        /// 获取当前格子的event
        /// </summary>
        /// <returns></returns>
        public Event GetCurrentEvent()
        {
            return Girds[CurrentGird].GirdEvent;
        }

        private void InitGird()
        {
            MoveStep.Clear();
            int count = RuntimeData.Instance.Conf.GirdCount;
            GirdCount = count + 1;
            Girds = new List<Gird>(count + 2);
            Gird gird = new Gird
            {
                Msg = "起点",
                GirdEvent = new Event()
            };
            Girds.Add(gird);
            for (int i = 0; i < count; ++i)
            {
                gird = new Gird
                {
                    GirdEvent = GetEvent(i)
                };
                gird.Msg = gird.GirdEvent.Name;
                Girds.Add(gird);
            }
            gird = new Gird
            {
                Msg = "终点",
                GirdEvent = new Event()
            };
            gird.GirdEvent.Command = Consts.E_GmaeFinish;
            Girds.Add(gird);
        }

        /// <summary>
        /// 获取事件
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Event GetEvent(int i)
        {
            Event e = new Event(RuntimeData.Instance.Events[RandomGenerator.Instance.GainIndex(WeightType.Event)]);
            if (e.Step == 0)
            {
                return e;
            }
            while (CheckCircle(i, e.Step))
            {
                e = new Event(RuntimeData.Instance.Events[RandomGenerator.Instance.GainIndex(WeightType.Event)]);
            }
            if (e.Step != 0)
            {
                MoveStep.Add(i, e.Step);
            }
            return e;
        }

        /// <summary>
        /// 判断环路
        /// </summary>
        /// <param name="i"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool CheckCircle(int i, int s)
        {
            int ans = s;
            i += s;
            while (MoveStep.TryGetValue(i, out int tmp))
            {
                ans += tmp;
                i += tmp;
                if (ans == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Effect
    {
        public int Count { get; set; }
    }
}