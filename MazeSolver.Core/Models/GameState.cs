namespace MazeSolver.Core.Models
{
    public class GameState
    {
        public string player { get; set; }
        public string message { get; set; }
        public int position_x { get; set; }
        public int position_y { get; set; }
        public bool win { get; set; }
        public bool dead { get; set; }
        public string url_move { get; set; }
        public string url_discover { get; set; }

        public Position CurrentPosition => new(position_x, position_y);
    }
}