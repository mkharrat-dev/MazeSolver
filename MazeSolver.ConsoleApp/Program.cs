using MazeSolver.Core.Api;
using MazeSolver.Core.Services;

using Microsoft.Extensions.Configuration;

var configData = new Dictionary<string, string?> { { "MazeApi:BaseUrl", "https://hire-game-maze.pertimm.dev/" } };
var config = new ConfigurationBuilder().AddInMemoryCollection(configData).Build();

var api = new MazeApiClient(config);
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