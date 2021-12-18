using System.Collections.Generic;

namespace Spg
{
    public class SpTool
    {
        private string _name;
        private int? _minCount;
        private int? _maxCount;
        private int? _weight;
        private bool? _enable;
        private List<string> _tag;

        public string Name
        {
            get => _name ?? "戒尺";
            set => _name = value;
        }
        public string Desc { get; set; }
        public int MinCount 
        {
            get => _minCount ?? 1;
            set => _minCount = value;
        }
        public int MaxCount
        {
            get => _maxCount ?? 10;
            set => _maxCount = value;
        }
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

        public bool IsOtk
        {
            get => Tag.Contains("otk");
        }

        public bool IsDiy
        {
            get => Tag.Contains("diy");
        }

        public override string ToString() => string.IsNullOrWhiteSpace(Desc) ? Name : $"{Name}({Desc})";
    }
}