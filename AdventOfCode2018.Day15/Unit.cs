namespace AdventOfCode2018.Day15
{
    public abstract class Unit
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; } = 200;
        public abstract char Type { get; }
    }
}
