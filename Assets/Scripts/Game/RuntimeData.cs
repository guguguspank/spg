using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spg
{
    public class RuntimeData : MonoSingleton<RuntimeData>
    {
        public GameConfig Conf { get; set; }

        public List<Event> EventList { get; set; }
        public List<Event> SpEventList { get; set; }
        public List<PlayMethod> PlayMethodList { get; set; }
        public List<Position> SpPositionList { get; set; }
        public List<Position> OtkPositionList { get; set; } = new List<Position>();
        public List<Position> DiyPositionList { get; set; } = new List<Position>();
        public List<SpTool> SpToolList { get; set; }
        public List<SpTool> OtkToolList { get; set; } = new List<SpTool>();
        public List<SpTool> DiyToolList { get; set; } = new List<SpTool>();

        public void Init()
        {
            string path = Application.persistentDataPath;

            Conf = Config.LoadYaml<GameConfig>(Path.Combine(path, "Config", "GameConfig.yaml"));

            EventList = Config.LoadYaml<List<Event>>(Path.Combine(path, "Config", "Event.yaml"));
            SpEventList = Config.LoadYaml<List<Event>>(Path.Combine(path, "Config", "SpEvent.yaml"));
            if (Conf.Extra)
            {
                List<Event> l = Config.LoadYaml<List<Event>>(Path.Combine(path, "Config", "ExtraEvent.yaml"));
                foreach (var item in l)
                {
                    SpEventList.Add(item);
                }
            }
            PlayMethodList = Config.LoadYaml<List<PlayMethod>>(Path.Combine(path, "Config", "PlayMethod.yaml"));
            SpPositionList = Config.LoadYaml<List<Position>>(Path.Combine(path, "Config", "Position.yaml"));
            foreach (var item in SpPositionList)
            {
                if (item.IsDiy)
                {
                    DiyPositionList.Add(item);
                }
                if (item.IsOtk)
                {
                    OtkPositionList.Add(item);
                }
            }
            SpToolList = Config.LoadYaml<List<SpTool>>(Path.Combine(path, "Config", "SpTool.yaml"));
            foreach (var item in SpToolList)
            {
                if (item.IsDiy)
                {
                    DiyToolList.Add(item);
                }
                if (item.IsOtk)
                {
                    OtkToolList.Add(item);
                }
            }
        }
    }
}
