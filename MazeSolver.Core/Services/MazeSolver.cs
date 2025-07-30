using MazeSolver.Core.Api;
using MazeSolver.Core.Models;

namespace MazeSolver.Core.Services
{
    public class MazeSolverService(MazeApiClient client)
    {
        private readonly MazeApiClient _client = client;
        private readonly HashSet<Position> visited = [];
        private readonly Dictionary<Position, Position> cameFrom = [];

        public async Task<List<Position>?> Solve()
        {
            var start = _client.State!.CurrentPosition;
            Queue<Position> queue = new();
            queue.Enqueue(start);
            visited.Add(start);
            cameFrom[start] = start;

            while (queue.Count != 0)
            {
                var pos = queue.Dequeue();
                await _client.Move(pos);
                var discoveries = await _client.Discover();

                foreach (var d in discoveries)
                {
                    var next = new Position(d.X, d.Y);
                    if (visited.Contains(next) || !d.Move || d.Value == "trap")
                        continue;

                    visited.Add(next);
                    cameFrom[next] = pos;

                    if (d.Value == "stop")
                        return ReconstructPath(start, next);

                    queue.Enqueue(next);
                }
            }

            return null;
        }

        private List<Position> ReconstructPath(Position start, Position end)
        {
            List<Position> path = [];
            var current = end;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }
    }
}