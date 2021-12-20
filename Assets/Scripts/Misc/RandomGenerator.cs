using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spg
{
    public class RandomGenerator : MonoSingleton<RandomGenerator>
    {
        private Dictionary<WeightType, (float, int)[]> WeightDict { get; set; } = new Dictionary<WeightType, (float, int)[]>();

        public IEnumerator Init()
        {
            yield return StartCoroutine(UpdateTool());
            yield return StartCoroutine(UpdatePosture());
            yield return StartCoroutine(UpdateEvent());
        }

        public IEnumerator UpdateTool()
        {
            List<int> wl = new List<int>();
            foreach (var item in RuntimeData.Instance.SpTools)
            {
                wl.Add(item.Weight);
            }
            InitWeight(WeightType.SpTool, wl);
            wl.Clear();
            foreach (var item in RuntimeData.Instance.OtkTools)
            {
                wl.Add(item.Weight);
            }
            InitWeight(WeightType.OtkTool, wl);
            wl.Clear();
            foreach (var item in RuntimeData.Instance.DiyTools)
            {
                wl.Add(item.Weight);
            }
            InitWeight(WeightType.DiyTool, wl);
            yield return null;
        }

        public IEnumerator UpdatePosture()
        {
            List<int> wl = new List<int>();
            foreach (var item in RuntimeData.Instance.SpPostures)
            {
                wl.Add(item.Weight);
            }
            InitWeight(WeightType.SpPosture, wl);
            wl.Clear();
            foreach (var item in RuntimeData.Instance.OtkPostures)
            {
                wl.Add(item.Weight);
            }
            InitWeight(WeightType.OtkPosture, wl);
            wl.Clear();
            foreach (var item in RuntimeData.Instance.DiyPostures)
            {
                wl.Add(item.Weight);
            }
            InitWeight(WeightType.DiyPosture, wl);
            yield return null;
        }

        public IEnumerator UpdateEvent()
        {
            List<int> wl = new List<int>();
            foreach (var item in RuntimeData.Instance.Events)
            {
                wl.Add(item.Weight);
            }
            InitWeight(WeightType.Event, wl);
            yield return null;
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

            WeightDict[type] = weights;
        }
    }

    public enum WeightType
    {
        Event,
        SpPosture,
        DiyPosture,
        OtkPosture,
        SpTool,
        DiyTool,
        OtkTool
    }
}