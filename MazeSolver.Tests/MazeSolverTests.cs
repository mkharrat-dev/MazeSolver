using MazeSolver.Core.Api;
using MazeSolver.Core.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MazeSolver.Tests
{
    public class MazeSolverTests
    {
        [Fact]
        public async Task MazeSolver_ShouldFindPath()
        {
            var configData = new Dictionary<string, string?>
        {
            { "MazeApi:BaseUrl", "https://hire-game-maze.pertimm.dev/" }
        };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            var client = new MazeApiClient(config);
            await client.StartGame("UnitTestBot");

            var solver = new MazeSolverService(client);
            var path = await solver.Solve();

            Assert.NotNull(path);
            Assert.True(path!.Count >= 0);
        }
    }
}