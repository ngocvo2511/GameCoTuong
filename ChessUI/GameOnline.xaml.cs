using ChessLogic.GameStates.GameState;
using ChessLogic;
using ChessUI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR.Client;
using System.Data.Common;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for GameUserControl.xaml
    /// </summary>
    public partial class GameOnline : UserControl
    {
        private readonly Image[,] pieceImages = new Image[10, 9];
        private readonly Ellipse[,] highlights = new Ellipse[10, 9];
        private readonly Canvas[,] posMoved = new Canvas[10, 9];
        private Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private GameState gameState;
        private Position selectedPos = null;
        private HubConnection connection;
        private string roomName;
        private Player color;
        private int time;
        string username;
        string opponentUsername;
        private bool start = false;
        public GameOnline(string roomName, Player color, int time, string username, string opponentUsername = "")
        {
            InitializeComponent();
            InitializeBoard();
            this.color = color;
            gameState = new GameState2P(Player.Red, Board.InitialForOnline(color));
            this.roomName = roomName;
            this.time = time;
            this.username = username;
            this.opponentUsername = opponentUsername;
            ShowGameInformation();
            DrawBoard(gameState.Board);
            ConnectHub();
            //settingsMenu.BackButtonClicked += BackButtonClicked;
            //selectGameModeMenu.BackButtonClicked += BackButtonClicked;
        }
        //private void BackButtonClicked(object sender, EventArgs e)
        //{
        //    pauseMenu.Visibility = Visibility.Visible;
        //}
        private void ShowGameInformation()
        {
            redInfo.Text = username;
            blackInfo.Text = opponentUsername;
            TurnTextBlock.Text = gameState.CurrentPlayer == Player.Red ? "Đỏ" : "Đen";
            StartButton.Visibility = (color == Player.Red && opponentUsername != "") ? Visibility.Visible : Visibility.Collapsed;

        }
        private async void ConnectHub()
        {
            var connectionManager = SignalRConnectionManager.Instance;
            connection = connectionManager.Connection;

            connection.Remove("MoveTo");
            connection.Remove("PlayerJoined");
            connection.Remove("PlayerLeft");
            connection.Remove("Error");


            connection.On<int, int, int, int>("MoveTo", (x1, y1, x2, y2) =>
            {
                if (color == gameState.CurrentPlayer)
                {
                    Dispatcher.Invoke(() =>
                    {

                        Position from = new Position(x1, y1);
                        Position to = new Position(x2, y2);
                        Move move = new NormalMove(from, to);
                        HandleMove(move);
                    });
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        Position from = new Position(9 - x1, 8 - y1);
                        Position to = new Position(9 - x2, 8 - y2);
                        Move move = new NormalMove(from, to);
                        HandleMove(move);
                    });
                }
            });

            connection.On<string, string>("PlayerJoined", (creatorUsername, joinerUsername) =>
            {
                Dispatcher.Invoke(() =>
                {
                    opponentUsername = joinerUsername;
                    ShowGameInformation();
                });
            });

            connection.On<string, string, int>("GameStarted", (player1, player2, gameTime) =>
            {
                Dispatcher.Invoke(() =>
                {
                    opponentUsername = player2;
                    time = gameTime;
                    StartGame();
                });
            });

            connection.On<string>("PlayerLeft", (connectionId) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Player {connectionId} has left the room.");
                });
            });

            connection.On<string>("Error", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message);
                });
            });

            await connectionManager.StartConnectionAsync();

        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (connection != null && connection.State == HubConnectionState.Connected)
            {
                await connection.SendAsync("StartGame", roomName);

            }
            else
            {
                MessageBox.Show("Not connected to the server.");
            }
        }

        private void StartGame()
        {
            // Bắt đầu trò chơi và tính thời gian
            // Ví dụ: khởi tạo bộ đếm thời gian và bắt đầu trò chơi
            //gameState = new GameState2P(Player.Red, Board.InitialForOnline(color), time);
            //ShowGameInformation();
            //DrawBoard(gameState.Board);
            // Khởi động bộ đếm thời gian nếu cần
            StartButton.Visibility = Visibility.Collapsed;
            start = true;

        }

        private void InitializeBoard()
        {
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    Image image = new Image();
                    pieceImages[r, c] = image;
                    PieceGrid.Children.Add(image);

                    Ellipse highlight = new Ellipse
                    {
                        Width = 40,
                        Height = 40,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight);
                    Canvas canvas = new Canvas()
                    {
                        //Width = 80,
                        //Height = 80
                        //VerticalAlignment = VerticalAlignment.Center,
                        //HorizontalAlignment = HorizontalAlignment.Center
                    };
                    posMoved[r, c] = canvas;
                    PosMovedGrid.Children.Add(canvas);
                }
            }
        }

        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    Piece piece = board[r, c];
                    pieceImages[r, c].Source = Images.GetImage(piece);
                }
            }
        }


        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 25, 255, 125);
            foreach (Position to in moveCache.Keys)
            {
                if (gameState.Board[to] != null)
                {
                    highlights[to.Row, to.Column].Fill = new SolidColorBrush(Color.FromArgb(150, 255, 0, 0));
                }
                else highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if(!start)
            {
                return;
            }
            if (gameState.CurrentPlayer != color)
            {
                return;
            }
            Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);

            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }

        private Position ToSquarePosition(Point point)
        {
            double squareWidth = BoardGrid.ActualWidth / 9;    // 9 cột cho cờ Tướng
            double squareHeight = BoardGrid.ActualHeight / 10; // 10 hàng cho cờ Tướng

            int row = (int)(point.Y / squareHeight);
            int col = (int)(point.X / squareWidth);
            //MessageBox.Show(row + " " + col);

            return new Position(row, col);
        }

        private void OnFromPositionSelected(Position pos)
        {

            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);

            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private async void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if (pos == null) return;
            if (moveCache.TryGetValue(pos, out Move move))
            {

                await Task.Run(async () =>
                {
                    await connection.SendAsync("MakeMove", move.FromPos.Row, move.FromPos.Column, move.ToPos.Row, move.ToPos.Column);
                });
            }
        }
        private void DrawCapturedGrid(Piece piece)
        {
            if (piece == null) return;
            Image image = new Image();
            image.Source = Images.GetImage(piece);
            if (gameState.CurrentPlayer == Player.Red)
            {
                BlackCapturedGrid.Children.Add(image);
            }
            else
            {
                RedCapturedGrid.Children.Add(image);
            }
        }
        private void DrawNewPos(Canvas canvas)
        {
            Line topLeftArrow = new Line
            {
                X1 = 5,
                Y1 = 0,
                X2 = 25,
                Y2 = 0,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(topLeftArrow);
            Line sideTopLeftArrow = new Line
            {
                X1 = 5,
                Y1 = 0,
                X2 = 5,
                Y2 = 20,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideTopLeftArrow);
            Line topRightArrow = new Line
            {
                X1 = 55,
                Y1 = 0,
                X2 = 75,
                Y2 = 0,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(topRightArrow);
            Line sideTopRightArrow = new Line
            {
                X1 = 75,
                Y1 = 0,
                X2 = 75,
                Y2 = 20,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideTopRightArrow);
            Line bottomLeftArrow = new Line
            {
                X1 = 5,
                Y1 = 70,
                X2 = 25,
                Y2 = 70,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(bottomLeftArrow);
            Line sideBotLeftArrow = new Line
            {
                X1 = 5,
                Y1 = 70,
                X2 = 5,
                Y2 = 50,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideBotLeftArrow);
            Line bottomRightArrow = new Line
            {
                X1 = 55,
                Y1 = 70,
                X2 = 75,
                Y2 = 70,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(bottomRightArrow);
            Line sideBotRightArrow = new Line
            {
                X1 = 75,
                Y1 = 70,
                X2 = 75,
                Y2 = 50,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideBotRightArrow);
        }
        private void DrawOldPos(Canvas canvas)
        {
            Line topLeftArrow = new Line
            {
                X1 = 20,
                Y1 = 15,
                X2 = 30,
                Y2 = 15,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(topLeftArrow);
            Line sideTopLeftArrow = new Line
            {
                X1 = 20,
                Y1 = 15,
                X2 = 20,
                Y2 = 25,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideTopLeftArrow);
            Line topRightArrow = new Line
            {
                X1 = 48,
                Y1 = 15,
                X2 = 58,
                Y2 = 15,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(topRightArrow);
            Line sideTopRightArrow = new Line
            {
                X1 = 58,
                Y1 = 15,
                X2 = 58,
                Y2 = 25,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideTopRightArrow);
            Line bottomLeftArrow = new Line
            {
                X1 = 20,
                Y1 = 53,
                X2 = 30,
                Y2 = 53,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(bottomLeftArrow);
            Line sideBotLeftArrow = new Line
            {
                X1 = 20,
                Y1 = 53,
                X2 = 20,
                Y2 = 43,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideBotLeftArrow);
            Line bottomRightArrow = new Line
            {
                X1 = 58,
                Y1 = 53,
                X2 = 48,
                Y2 = 53,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(bottomRightArrow);
            Line sideBotRightArrow = new Line
            {
                X1 = 58,
                Y1 = 53,
                X2 = 58,
                Y2 = 43,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideBotRightArrow);
        }
        private void ShowPrevMove(Move move)
        {
            DrawOldPos(posMoved[move.FromPos.Row, move.FromPos.Column]);
            DrawNewPos(posMoved[move.ToPos.Row, move.ToPos.Column]);
        }
        private void HidePrevMove(Move move)
        {
            posMoved[move.FromPos.Row, move.FromPos.Column].Children.Clear();
            posMoved[move.ToPos.Row, move.ToPos.Column].Children.Clear();
        }
        private async void HandleMove(Move move)
        {

            // Khóa giao diện để tránh tác động ngoài ý muốn
            await Dispatcher.InvokeAsync(() => MainGame.IsHitTestVisible = false);

            if (gameState.Moved.Any())
            {
                await Dispatcher.InvokeAsync(() => HidePrevMove(gameState.Moved.First().Item1));
            }

            // Thực hiện logic trò chơi trong luồng nền
            await Task.Run(() =>
            {
                gameState.MakeMove(move);
            });

            // Cập nhật giao diện trên luồng chính
            await Dispatcher.InvokeAsync(() =>
            {
                Sound.PlayMoveSound();
                DrawBoard(gameState.Board);
                ShowPrevMove(move);
                DrawCapturedGrid(gameState.CapturedPiece);
                WarningTextBlock.Text = gameState.Board.IsInCheck(gameState.CurrentPlayer) ? "Chiếu tướng!" : null;
                TurnTextBlock.Text = gameState.CurrentPlayer == Player.Red ? "Đỏ" : "Đen";
            });
           

            // Mở khóa giao diện
            await Dispatcher.InvokeAsync(() => MainGame.IsHitTestVisible = true);

            if (gameState.IsGameOver())
            {
                HideHighlights();
                CellGrid.IsEnabled = false;
                //_mainWindow.CreateGameOverMenu(gameState);
            }
        }


        public event RoutedEventHandler SettingButtonClicked
        {
            add { AddHandler(SettingButtonClickedEvent, value); }
            remove { RemoveHandler(SettingButtonClickedEvent, value); }
        }
        public static readonly RoutedEvent SettingButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "SettingButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(GameOnline)
        );
        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SettingButtonClickedEvent));
        }

        public event RoutedEventHandler LeaveRoomButtonClicked
        {
            add { AddHandler(LeaveRoomButtonClickedEvent, value); }
            remove { RemoveHandler(LeaveRoomButtonClickedEvent, value); }
        }
        public static readonly RoutedEvent LeaveRoomButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "LeaveRoomButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(GameOnline)
        );

        private async void LeaveRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (connection != null && connection.State == HubConnectionState.Connected)
            {
                await connection.InvokeAsync("LeaveRoom", roomName);
            }
            else
            {
                MessageBox.Show("Not connected to the server.");
            }
            RaiseEvent(new RoutedEventArgs(LeaveRoomButtonClickedEvent));

        }
        private async void CloseAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (connection != null && connection.State == HubConnectionState.Connected)
            {
                await connection.InvokeAsync("LeaveRoom", roomName);
            }
            else
            {
                MessageBox.Show("Not connected to the server.");
            }
            Application.Current.Shutdown();
        }

        private void MinimizeAppButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Application.Current.MainWindow.DragMove();
            }
        }
    }
}
