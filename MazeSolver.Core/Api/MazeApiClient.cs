using System.Net.Http;
using System.Text;
using System.Text.Json;
using MazeSolver.Core.Models;

namespace MazeSolver.Core.Api
{
    public class MazeApiClient
    {
        private readonly HttpClient _http = new();
        private const string BaseUrl = "https://hire-game-maze.pertimm.dev/";
        public GameState? State { get; private set; }

        public async Task<GameState> StartGame(string player)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { player }), Encoding.UTF8, "application/json");
            var res = await _http.PostAsync(BaseUrl + "start-game/", content);
            var body = await res.Content.ReadAsStringAsync();
            State = JsonSerializer.Deserialize<GameState>(body);
            return State!;
        }

        public async Task<List<Discovery>> Discover()
        {
            var res = await _http.GetAsync(State!.UrlDiscover);
            var body = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Discovery>>(body)!;
        }

        public async Task<GameState> Move(Position pos)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { position_x = pos.X, position_y = pos.Y }), Encoding.UTF8, "application/json");
            var res = await _http.PostAsync(State!.UrlMove, content);
            var body = await res.Content.ReadAsStringAsync();
            State = JsonSerializer.Deserialize<GameState>(body);
            return State!;
        }
    }
}