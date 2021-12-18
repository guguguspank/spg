using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Spg
{
    public class Event
    {
        private string _name;
        private int? _weight;
        private string _command;
        private Dictionary<string, string> _args;
        
        private int? _step;
        private bool? _isInit;

        public string Name
        {
            get => _name ?? "测试事件";
            set => _name = value;
        }
        public string Desc { get; set; }
        public int Weight
        {
            get => _weight ?? 50;
            set => _weight = value < 1 ? 1 : value > 100 ? 100 : value;
        }
        public string Command
        {
            get => _command ?? "null";
            set => _command = value;
        }
        public Dictionary<string, string> Args
        {
            get => _args ?? new Dictionary<string, string>();
            set => _args = value;
        }

        [YamlIgnore]
        public int Step
        {
            get => _step ?? 0;
            set => _step = value;
        }
        [YamlIgnore]
        public bool IsInit
        {
            get => _isInit ?? false;
            set => _isInit = value;
        }

        public Event() { }

        public Event(Event e)
        {
            Name = e.Name;
            Desc = e.Desc;
            Weight = e.Weight;
            Command = e.Command;
            Args = new Dictionary<string, string>(e.Args);
            Step = e.Step;
        }
    }
}