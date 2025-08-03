using System;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Text.Json;
using MazeSolver.Core.Models;
using Microsoft.Extensions.Configuration;

namespace MazeSolver.Core.Api
{
    public class MazeApiClient(IConfiguration config)
    {
        private readonly HttpClient _http = new();
        private readonly string _baseUrl = config["MazeApi:BaseUrl"] ?? "https://hire-game-maze.pertimm.dev/";

        public GameState? State { get; private set; }

        public async Task<GameState> StartGame(string player)
        {
            string url = _baseUrl + "start-game/";
            MultipartFormDataContent form = new()
            {
                { new StringContent(player), "player" }
            };
            try
            {
                var response = await _http.PostAsync(url, form);
                string body = await response.Content.ReadAsStringAsync();
                State = JsonSerializer.Deserialize<GameState>(body);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
            return State!;
        }

        public async Task<List<Discovery>> Discover()
        {
            var res = await _http.GetAsync(State!.url_discover);
            var body = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Discovery>>(body)!;
        }

        public async Task<GameState> Move(Position pos)
        {
            MultipartFormDataContent form = new()
            {
                { new StringContent(pos.X.ToString()), "position_x" },
                { new StringContent(pos.Y.ToString()), "position_y" }
            };
            string url = State!.url_move;
            string body;
            // Requête POST avec le body JSON
            try
            {
                var response = await _http.PostAsync(url, form);
                body = await response.Content.ReadAsStringAsync();
                State = JsonSerializer.Deserialize<GameState>(body);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
            return State!;
        }
    }
}
