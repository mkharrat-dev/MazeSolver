using MazeSolver.Core.Api;
using MazeSolver.Core.Services;

var api = new MazeApiClient();
await api.StartGame("BotFromConsole");

var solver = new MazeSolverService(api);
var path = await solver.Solve();

if (path != null)
{
    foreach (var pos in path)
    {
        var state = await api.Move(pos);
        Console.WriteLine($"> Moved to ({state.position_x}, {state.position_y})");

        if (state.dead)
        {
            Console.WriteLine("☠️ You died.");
            break;
        }

        if (state.win)
        {
            Console.WriteLine("🎉 You won!");
            break;
        }
    }
}
else
{
    Console.WriteLine("❌ No path found.");
}