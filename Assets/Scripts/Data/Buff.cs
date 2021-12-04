namespace Spg
{
    public class Buff
    {
        public string Effect { get; set; }
        public int Count { get; set; }

        public Buff() { }
        public Buff(string e, int c)
        {
            Effect = e;
            Count = c;
        }
    }
}