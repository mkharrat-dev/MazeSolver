using MazeSolver.Core.Api;
using MazeSolver.Core.Models;
using MazeSolver.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MazeSolver.WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly MazeApiClient _api = new();
        private MazeSolver.Core.Services.MazeSolverService _solver;
        private readonly Dictionary<Position, string> _map = [];

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            MazeGrid.Children.Clear();
            _map.Clear();
            await _api.StartGame("WpfBot");
            _solver = new MazeSolver.Core.Services.MazeSolverService(_api);
            var path = await _solver.Solve();
            if (path != null && path.Count > 0)
            {
                foreach (var pos in path)
                {
                    var state = await _api.Move(pos);
                    _map[pos] = state.win ? "stop" : "path";
                    DrawMaze(state.position_x, state.position_y);
                    await Task.Delay(150);
                    if (state.dead || state.win) break;
                }
            }
            else
            {
                MessageBox.Show("❌ Aucun chemin trouvé.");
            }
        }

        private void DrawMaze(int currentX, int currentY)
        {
            MazeGrid.Children.Clear();
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    var pos = new Position(x, y);
                    var cell = new Border
                    {
                        Width = 25,
                        Height = 25,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0.5),
                        Background = Brushes.White
                    };

                    if (x == currentX && y == currentY)
                        cell.Background = Brushes.Blue;
                    else if (_map.TryGetValue(pos, out var type))
                    {
                        cell.Background = type switch
                        {
                            "wall" => Brushes.Gray,
                            "trap" => Brushes.Red,
                            "stop" => Brushes.Green,
                            _ => Brushes.LightGray
                        };
                    }

                    MazeGrid.Children.Add(cell);
                }
            }
        }
    }
}