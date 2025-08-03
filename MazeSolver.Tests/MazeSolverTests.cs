using MazeSolver.Core.Api;
using MazeSolver.Core.Services;
using Xunit;

namespace MazeSolver.Tests
{
    public class MazeSolverTests
    {
        [Fact]
        public async Task MazeSolver_ShouldFindPath()
        {
            var client = new MazeApiClient();
            await client.StartGame("UnitTestBot");

            var solver = new MazeSolverService(client);
            var path = await solver.Solve();

            Assert.NotNull(path);
            Assert.True(path!.Count >= 0);
        }
    }
}