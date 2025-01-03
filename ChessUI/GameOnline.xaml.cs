using ChessLogic.GameStates.GameState;
using ChessLogic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Threading;

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
        public HubConnection connection;
        private string roomName;
        private Player color;
        private int time;
        string username;
        string opponentUsername;
        private bool start = false;
        private DispatcherTimer redTimer;
        private DispatcherTimer blackTimer;
        private Brush redBrush = new SolidColorBrush(Colors.Red);
        private Brush blackBrush = new SolidColorBrush(Colors.Black);
        private Stack<Tuple<Move, Piece>> moveList;


        public GameOnline(string roomName, Player color, int time, string username, string opponentUsername = "")
        {
            InitializeComponent();
            InitializeBoard();
            this.color = color;
            gameState = new GameState2P(Player.Red, Board.InitialForOnline(color), time);
            this.roomName = roomName;
            this.time = time;
            this.username = username;
            this.opponentUsername = opponentUsername;
            ShowGameInformation();
            DrawBoard(gameState.Board);
            ConnectHub();
        }

        private void ShowGameInformation()
        {
            redInfo.Text = username;
            blackInfo.Text = opponentUsername;
            TurnTextBlock.Text = gameState.CurrentPlayer == Player.Red ? "Đỏ" : "Đen";
        }
        private async void ConnectHub()
        {
            var connectionManager = SignalRConnectionManager.Instance;
            connection = connectionManager.Connection;

            connection.Remove("MoveTo");
            connection.Remove("PlayerJoined");
            connection.Remove("PlayerLeft");
            connection.Remove("GameStarted");
            connection.Remove("CreateGameOver");
            connection.Remove("Countdown");




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
                Dispatcher.Invoke(async () =>
                {
                    opponentUsername = joinerUsername;
                    ShowGameInformation();
                    await connection.SendAsync("StartGame", roomName);
                });
            });

            connection.On<int>("Countdown", (count) =>
            {
                Dispatcher.Invoke(() =>
                {
                    UnableClick();
                    CountdownText.Text = "Trận đấu sẽ bắt đầu sau: " + count.ToString();
                    CountdownPopup.IsOpen = true;
                });
            });

            connection.On<int>("GameStarted", (gameTime) =>
            {
                Dispatcher.Invoke(() =>
                {
                    CountdownPopup.IsOpen = false;
                    AbleClick();
                    StartGame();
                });
            });

            connection.On<Result, Player>("CreateGameOver", (result, current) =>
            {
                Dispatcher.Invoke(() =>
                {
                    start = false;
                    RaiseGameOverEvent(result, current);
                });
            });

            connection.On<string>("PlayerLeft", (connectionId) =>
            {
                Dispatcher.Invoke(async () =>
                {
                    if (!start)
                    {
                        opponentUsername = "";
                    }
                    else
                    {
                        gameState.Result = Result.Win(color, EndReason.PlayerDisconnected);
                        moveList = new Stack<Tuple<Move, Piece>>(gameState.Moved.ToArray());
                        await connection.SendAsync("GameOver", roomName, gameState.Result, color == Player.Red ? Player.Black : Player.Red);
                    }
                });
            });


            await connectionManager.StartConnectionAsync();

        }


        private void StartGame()
        {
            start = true;
            ResetGameState();
            InitializeTimer();
            SwitchTurn();
        }

        private void ResetGameState()
        {
            gameState = new GameState2P(Player.Red, Board.InitialForOnline(color), time);
            selectedPos = null;
            moveCache.Clear();
            DrawBoard(gameState.Board);
            HideHighlights();
            ResetTimer();
            InitializeTimer();
        }

        private async Task RedTimer_TickAsync(object sender, EventArgs e)
        {
            gameState.timeRemainingRed--;
            int minutes = gameState.timeRemainingRed / 60;
            int seconds = gameState.timeRemainingRed % 60;
            if (color == Player.Red)
            {
                bottomClock.Text = $"{minutes:D2}:{seconds:D2}";
            }
            else
            {
                topClock.Text = $"{minutes:D2}:{seconds:D2}";
            }
            if (gameState.timeRemainingRed <= 0)
            {
                StopTimer();
                HideHighlights();
                CellGrid.IsEnabled = false;
                gameState.TimeForfeit();
                await connection.SendAsync("GameOver", roomName, gameState.Result, gameState.CurrentPlayer);
                return;
            }
            if (gameState.timeRemainingRed < 60)
            {
                if (color == Player.Red)
                {
                    bottomClock.Foreground = redBrush;
                }
                else
                {
                    topClock.Foreground = redBrush;
                }
            }
        }
        private async Task BlackTimer_TickAsync(object sender, EventArgs e)
        {
            gameState.timeRemainingBlack--;
            int minutes = gameState.timeRemainingBlack / 60;
            int seconds = gameState.timeRemainingBlack % 60;
            if (color == Player.Black)
            {
                bottomClock.Text = $"{minutes:D2}:{seconds:D2}";
            }
            else
            {
                topClock.Text = $"{minutes:D2}:{seconds:D2}";
            }
            if (gameState.timeRemainingBlack <= 0)
            {
                StopTimer();
                HideHighlights();
                CellGrid.IsEnabled = false;
                gameState.TimeForfeit();
                await connection.SendAsync("GameOver", roomName, gameState.Result, gameState.CurrentPlayer);
                return;
            }
            if (gameState.timeRemainingBlack < 60)
            {
                if (color == Player.Black)
                {
                    bottomClock.Foreground = blackBrush;
                }
                else
                {
                    topClock.Foreground = blackBrush;
                }
            }
        }

        internal void StopTimer()
        {
            redTimer.Stop();
            blackTimer.Stop();
        }
        internal void ContinueTimer()
        {
            if (!gameState.IsGameOver())
            {
                if (gameState.CurrentPlayer == Player.Red)
                {
                    redTimer.Start();
                }
                else
                {
                    blackTimer.Start();
                }
            }
        }
        private void SwitchTurn()
        {
            redTimer.Stop();
            blackTimer.Stop();

            if (gameState.CurrentPlayer == Player.Red)
            {
                redTimer.Start();
            }
            else
            {
                blackTimer.Start();
            }
        }

        private void InitializeTimer()
        {
            int minutes = gameState.timeRemainingRed / 60;
            int seconds = gameState.timeRemainingRed % 60;
            bottomClock.Text = $"{minutes:D2}:{seconds:D2}";
            topClock.Text = $"{minutes:D2}:{seconds:D2}";

            redTimer = new DispatcherTimer();
            redTimer.Interval = TimeSpan.FromSeconds(1);
            redTimer.Tick += RedTimer_Tick;
            blackTimer = new DispatcherTimer();
            blackTimer.Interval = TimeSpan.FromSeconds(1);
            blackTimer.Tick += BlackTimer_Tick;
        }

        public void ResetTimer()
        {
            if (redTimer != null)
            {
                redTimer.Stop();
                redTimer.Tick -= RedTimer_Tick;
                redTimer = null;
            }
            if (blackTimer != null)
            {
                blackTimer.Stop();
                blackTimer.Tick -= BlackTimer_Tick;
                blackTimer = null;
            }
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
            if (!start)
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
                SwitchTurn();
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
                moveList = new Stack<Tuple<Move, Piece>>(gameState.Moved.ToArray());
                HideHighlights();
                CellGrid.IsEnabled = false;
                if (redTimer != null) StopTimer();
                await connection.SendAsync("GameOver", roomName, gameState.Result, gameState.CurrentPlayer);
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

        private void LeaveRoomButton_Click(object sender, RoutedEventArgs e)
        {
            //if (connection != null && connection.State == HubConnectionState.Connected && start)
            //{
            //    await connection.InvokeAsync("LeaveRoom", roomName);
            //}
            RaiseEvent(new RoutedEventArgs(LeaveRoomButtonClickedEvent));

        }

        public async Task LeaveRoomAsync()
        {
            bool IsConnected = connection != null && connection.State == HubConnectionState.Connected;

            if (IsConnected)
            {
                await connection.InvokeAsync("LeaveRoom", roomName);
            }
        }


        public static readonly RoutedEvent CloseAppButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "CloseAppButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(GameOnline)
        );
        public event RoutedEventHandler CloseAppButtonClicked
        {
            add { AddHandler(CloseAppButtonClickedEvent, value); }
            remove { RemoveHandler(CloseAppButtonClickedEvent, value); }
        }

        private void CloseAppButton_Click(object sender, RoutedEventArgs e)
        {
            //if (connection != null && connection.State == HubConnectionState.Connected && start)
            //{
            //    await connection.InvokeAsync("LeaveRoom", roomName);
            //}
            RaiseEvent(new RoutedEventArgs(CloseAppButtonClickedEvent));
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

        public static readonly RoutedEvent GameOverEvent = EventManager.RegisterRoutedEvent(
       "GameOver", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GameOnline));

        public event RoutedEventHandler GameOver
        {
            add { AddHandler(GameOverEvent, value); }
            remove { RemoveHandler(GameOverEvent, value); }
        }

        protected void RaiseGameOverEvent(Result result, Player currentPlayer)
        {
            RaiseEvent(new GameOverEventArgs(GameOverEvent, result, currentPlayer));
        }
        private void RedTimer_Tick(object sender, EventArgs e)
        {
            _ = RedTimer_TickAsync(sender, e);
        }

        private void BlackTimer_Tick(object sender, EventArgs e)
        {
            _ = BlackTimer_TickAsync(sender, e);
        }

        public void Review()
        {
            if(gameState.Moved.Count != 0)
                HidePrevMove(gameState.Moved.Peek().Item1);
            gameState = new GameState2P(Player.Red, Board.InitialForOnline(color));
            TurnTextBlock.Text = "Đỏ";
            BlackCapturedGrid.Children.Clear();
            RedCapturedGrid.Children.Clear();
            bottomClock.Text = null;
            topClock.Text = null;
            WarningTextBlock.Text = null;
            ResetTimer();
            DrawBoard(gameState.Board);
            AbleClick();
            CellGrid.IsHitTestVisible = false;
            DoButton.Visibility = Visibility.Visible;
            UndoButton.Visibility = Visibility.Visible;
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            if (gameState.Moved.Count != 0) HidePrevMove(gameState.Moved.First().Item1);
            OnToPositionSelected(selectedPos);
            if (gameState.Moved.Count == 0) return;
            var move = gameState.Moved.Pop();
            Move doMove = new NormalMove(move.Item1.ToPos, move.Item1.FromPos);
            doMove.Execute(gameState.Board);
            gameState.Board[doMove.FromPos] = move.Item2;
            DrawBoard(gameState.Board);
            if (gameState.Moved.Count != 0)
            {
                ShowPrevMove(gameState.Moved.First().Item1);
            }
            gameState.CapturedPiece = move.Item2;
            moveList.Push(move);
            gameState.CurrentPlayer = gameState.CurrentPlayer.Opponent();
            UndoCapturedGrid(gameState.CapturedPiece);
            TurnTextBlock.Text = gameState.CurrentPlayer == Player.Red ? "Đỏ" : "Đen";
            WarningTextBlock.Text = gameState.Board.IsInCheck(gameState.CurrentPlayer) ? "Chiếu tướng!" : null;
            gameState.noCapture.Pop();
        }

        private void DoButton_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            if (moveList.Count == 0) return;
            if (gameState.Moved.Count != 0) HidePrevMove(gameState.Moved.First().Item1);
            bool capture = moveList.Peek().Item1.Execute(gameState.Board);

            if (capture)
            {
                gameState.noCapture.Push(0);
                //stateHistory.Clear();
            }
            else
            {
                if (gameState.noCapture.Count == 0) gameState.noCapture.Push(1);
                else gameState.noCapture.Push(gameState.noCapture.Peek() + 1);
            }
            gameState.Moved.Push(moveList.Pop());
            DrawBoard(gameState.Board);
            if (gameState.Moved.Count != 0)
            {
                ShowPrevMove(gameState.Moved.First().Item1);
            }
            DrawCapturedGrid(gameState.Moved.Peek().Item2);
            gameState.CurrentPlayer = gameState.CurrentPlayer.Opponent();
            WarningTextBlock.Text = gameState.Board.IsInCheck(gameState.CurrentPlayer) ? "Chiếu tướng!" : null;
            TurnTextBlock.Text = gameState.CurrentPlayer == Player.Red ? "Đỏ" : "Đen";
        }

        private void UndoCapturedGrid(Piece piece)
        {
            if (piece == null) return;
            if (gameState.CurrentPlayer == Player.Black)
            {
                int count = BlackCapturedGrid.Children.Count;
                if (count > 0)
                    BlackCapturedGrid.Children.RemoveAt(count - 1);
            }
            else
            {
                int count = RedCapturedGrid.Children.Count;
                if (count > 0)
                    RedCapturedGrid.Children.RemoveAt(count - 1);
            }
        }

        private void AbleClick()
        {
            CellGrid.IsHitTestVisible = true;
            LeaveButton.IsEnabled = true;
            SettingButton.IsEnabled = true;
            CloseAppButton.IsEnabled = true;
        }

        private void UnableClick()
        {
            CellGrid.IsHitTestVisible = false;
            LeaveButton.IsEnabled = false;
            SettingButton.IsEnabled = false;
            CloseAppButton.IsEnabled = false;
        }
    }
}
