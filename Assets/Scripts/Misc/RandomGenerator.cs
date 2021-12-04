using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spg
{
    public class RandomGenerator : Singleton<RandomGenerator>
    {
        private Dictionary<WeightType, (float, int)[]> WeightDict { get; set; }

        public void Init()
        {
            WeightDict = new Dictionary<WeightType, (float, int)[]>();
            List<int> l = new List<int>();
            l.Add(RuntimeData.Instance.Conf.Event);
            l.Add(RuntimeData.Instance.Conf.SpEvent);
            InitWeight(WeightType.Base, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.EventList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.Event, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.SpEventList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.SpEvent, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.PlayMethodList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.PlayMethod, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.SpPositionList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.SpPosition, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.DiyPositionList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.DiyPosition, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.OtkPositionList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.OtkPosition, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.SpToolList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.SpTool, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.DiyToolList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.DiyTool, l);
            l.Clear();

            foreach (var item in RuntimeData.Instance.OtkToolList)
            {
                l.Add(item.Weight);
            }
            InitWeight(WeightType.OtkTool, l);
            l.Clear();
        }

        public int GainIndex(WeightType type)
        {
            var weights = WeightDict[type];
            var random = new System.Random(Guid.NewGuid().GetHashCode());
            var randomNum = random.NextDouble() * weights.Length;
            int intRan = (int)Math.Floor(randomNum);
            var p = weights[intRan];
            return p.Item1 > randomNum - intRan ? intRan : p.Item2;
        }

        public T GainItem<T>()
        {
            return default(T);
        }

        private void InitWeight(WeightType type, List<int> weightList)
        {
            int total = weightList.Sum();          
            int length = weightList.Count();
            var avg = 1f * total / length;
            List<(float, int)> smallAvg = new List<(float, int)>();
            List<(float, int)> bigAvg = new List<(float, int)>();
            (float, int)[] weights = new (float, int)[length];
            
            for (int i = 0; i < length; ++i)
            {
                (weightList[i] > avg ? bigAvg : smallAvg).Add((weightList[i], i));
            }

            for (int i = 0; i < length; ++i)
            {
                if (smallAvg.Count > 0)
                {
                    if (bigAvg.Count > 0)
                    {
                        weights[smallAvg[0].Item2] = (smallAvg[0].Item1 / avg, bigAvg[0].Item2);
                        bigAvg[0] = (bigAvg[0].Item1 - avg + smallAvg[0].Item1, bigAvg[0].Item2);
                        if (avg - bigAvg[0].Item1 > 0.0000001f)
                        {
                            smallAvg.Add(bigAvg[0]);
                            bigAvg.RemoveAt(0);
                        }
                    }
                    else
                    {
                        weights[smallAvg[0].Item2] = (smallAvg[0].Item1 / avg, smallAvg[0].Item2);
                    }
                    smallAvg.RemoveAt(0);
                }
                else
                {
                    weights[bigAvg[0].Item2] = (bigAvg[0].Item1 / avg, bigAvg[0].Item2);
                    bigAvg.RemoveAt(0);
                }
            }

            WeightDict.Add(type, weights);
        }
    }

    public enum WeightType
    {
        Base,
        Event,
        SpEvent,
        ExtraEvent,
        PlayMethod,
        SpPosition,
        DiyPosition,
        OtkPosition,
        SpTool,
        DiyTool,
        OtkTool
    }
}