using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Spg
{
    public class SpPosture : IConfig
    {
        private string _id;
        private string _name;
        private int? _weight;
        private bool? _enable;
        private List<string> _tag;
        private bool? _ignore;

        public string Id
        {
            get => _id ?? Guid.NewGuid().ToString();
            set => _id = value;
        }
        public string Name
        {
            get => _name ?? "otk";
            set => _name = value;
        }
        public string Desc { get; set; }
        public int Weight
        {
            get => _weight ?? 50;
            set => _weight = value < 1 ? 1 : value > 100 ? 100 : value;
        }
        public bool Enable
        {
            get => _enable ?? true;
            set => _enable = value;
        }
        public List<string> Tag
        {
            get => _tag ?? new List<string> { "sp", "diy", "otk" };
            set => _tag = value;
        }
        public bool Ignore
        {
            get => _ignore ?? false;
            set => _ignore = value;
        }

        [YamlIgnore]
        public bool IsOtk
        {
            get => Tag.Contains("otk");
        }
        [YamlIgnore]
        public bool IsDiy
        {
            get => Tag.Contains("diy");
        }

        public override string ToString() => string.IsNullOrWhiteSpace(Desc) ? Name : $"{Name}({Desc})";
    }
}