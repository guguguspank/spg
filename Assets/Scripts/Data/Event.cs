using System.Collections.Generic;

namespace Spg
{
    public class Event
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> Args { get; set; }
        public int Weight { get; set; }

        public string ShowMsg { get; set; }
        public bool needHandle { get; set; } = false;

        public Event() { }
        public Event(Event e)
        {
            Name = e.Name;
            Desc = e.Desc;
            Method = e.Method;
            Args = e.Args;
            Weight = e.Weight;
            ShowMsg = e.ShowMsg;
        }
    }
}