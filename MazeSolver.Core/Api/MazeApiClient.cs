using System;
using System.Net.Http;
using System.Numerics;
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
            //var payload = new { player };

            //// Sérialiser en JSON
            //var json = JsonSerializer.Serialize(payload);

            //// Contenu HTTP avec Content-Type
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            var form = new MultipartFormDataContent
            {
                { new StringContent(player), "player" }
            };
            string url = BaseUrl + "start-game/";
            // Requête POST avec le body JSON
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
            var uri = State!.url_discover;

            var res = await _http.GetAsync(State!.url_discover);
            var body = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Discovery>>(body)!;
        }

        public async Task<GameState> Move(Position pos)
        {
            var payload = new { position_x = pos.X, position_y = pos.Y };

            //// Sérialiser en JSON
            //var json = JsonSerializer.Serialize(payload);

            //// Contenu HTTP avec Content-Type
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            var form = new MultipartFormDataContent
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
