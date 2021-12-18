using UnityEngine;

namespace Spg
{
    public class Gird
    {
        private Vector3 _pos;
        private string _msg;
        private Event _girdEvent;

        public Vector3 Pos { get; set; }
        public string Msg { get; set; }
        public Event GirdEvent { get; set; }
    }
}