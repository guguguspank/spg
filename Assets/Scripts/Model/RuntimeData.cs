using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spg
{
    /// <summary>
    /// 全局游戏数据
    /// </summary>
    public class RuntimeData : MonoSingleton<RuntimeData>
    {
        public GameConfig Conf { get; set; }

        public List<SpTool> SpTools { get; set; }
        public List<SpTool> DiyTools { get; set; }
        public List<SpTool> OtkTools { get; set; }

        public List<SpPosture> SpPostures { get; set; }
        public List<SpPosture> DiyPostures { get; set; }
        public List<SpPosture> OtkPostures { get; set; }

        public List<Event> Events { get; set; }

        public IEnumerator Init()
        {
            SpTools = new List<SpTool>();
            DiyTools = new List<SpTool>();
            OtkTools = new List<SpTool>();
            SpPostures = new List<SpPosture>();
            DiyPostures = new List<SpPosture>();
            OtkPostures = new List<SpPosture>();
            Events = new List<Event>();

            Conf = Config.LoadYaml<GameConfig>(Path.Combine(Application.persistentDataPath, "Config", "GameConfig.yaml"));

            yield return StartCoroutine(LoadSpTool());
            yield return StartCoroutine(LoadSpPosture());
            yield return StartCoroutine(LoadEvent(Path.Combine(Application.persistentDataPath, "Config", "Event", "MoveEvent.yaml")));
            yield return StartCoroutine(LoadEvent(Path.Combine(Application.persistentDataPath, "Config", "Event", "SpEvent.yaml")));

            if (Conf.Extra)
            {
                yield return StartCoroutine(LoadEvent(Path.Combine(Application.persistentDataPath, "Config", "Event", "ExtraEvent.yaml")));
            }

        }

        public IEnumerator LoadSpTool()
        {
            SpTools.Clear();
            DiyTools.Clear();
            OtkTools.Clear();
            List<SpTool> spTools = Config.LoadYaml<List<SpTool>>(Path.Combine(Application.persistentDataPath, "Config", "SpTool.yaml"));

            foreach (var tool in spTools)
            {
                if (tool.Enable)
                {
                    SpTools.Add(tool);
                    if (tool.IsOtk)
                    {
                        OtkTools.Add(tool);
                    } 
                    if (tool.IsDiy)
                    {
                        DiyTools.Add(tool);
                    }
                }
            }

            yield return null;
        }

        public IEnumerator LoadSpPosture()
        {
            SpPostures.Clear();
            DiyPostures.Clear();
            OtkPostures.Clear();
            List<SpPosture> spPostures = Config.LoadYaml<List<SpPosture>>(Path.Combine(Application.persistentDataPath, "Config", "SpPosture.yaml"));

            foreach (var posture in spPostures)
            {
                if (posture.Enable)
                {
                    SpPostures.Add(posture);
                    if (posture.IsOtk)
                    {
                        OtkPostures.Add(posture);
                    }
                    if (posture.IsDiy)
                    {
                        DiyPostures.Add(posture);
                    }
                }
            }

            yield return null;
        }

        public IEnumerator LoadEvent(string path)
        {
            List<Event> temp = Config.LoadYaml<List<Event>>(path);

            foreach (var item in temp)
            {
                if (item.Args.TryGetValue("Step", out string step) && int.TryParse(step, out int tmp))
                {
                    item.Step = tmp;
                }
                Events.Add(item);
            }

            yield return null;
        }
    }
}
