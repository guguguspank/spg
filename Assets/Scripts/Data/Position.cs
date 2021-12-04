using System.Collections.Generic;

namespace Spg
{
    public class Position
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Weight { get; set; }
        public List<string> Tag { get; set; }

        public bool IsOtk
        {
            get
            {
                return Tag.Contains("otk");
            }
        }

        public bool IsDiy
        {
            get
            {
                return Tag.Contains("diy");
            }
        }
    }
}