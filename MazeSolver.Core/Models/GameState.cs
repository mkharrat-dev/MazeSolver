namespace MazeSolver.Core.Models
{
    public class GameState
    {
        public string Player { get; set; }
        public int Position_x { get; set; }
        public int Position_y { get; set; }
        public bool Win { get; set; }
        public bool Dead { get; set; }
        public string UrlMove { get; set; }
        public string UrlDiscover { get; set; }

        public Position CurrentPosition => new(Position_x, Position_y);
    }
}