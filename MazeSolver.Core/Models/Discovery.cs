namespace MazeSolver.Core.Models
{
    public class Discovery
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Move { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}