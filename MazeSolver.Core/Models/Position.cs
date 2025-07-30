namespace MazeSolver.Core.Models
{
    public record Position(int X, int Y)
    {
        public IEnumerable<Position> Neighbors() => new[]
        {
            new Position(X + 1, Y),
            new Position(X - 1, Y),
            new Position(X, Y + 1),
            new Position(X, Y - 1),
        };
    }
}