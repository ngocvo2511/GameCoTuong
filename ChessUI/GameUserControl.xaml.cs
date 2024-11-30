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

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for GameUserControl.xaml
    /// </summary>
    public partial class GameUserControl : UserControl
    {
        private readonly Image[,] pieceImages = new Image[10, 9];
        private readonly Ellipse[,] highlights = new Ellipse[10, 9];
        private readonly Canvas[,] posMoved = new Canvas[10, 9];
        private Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private GameState gameState;
        private Position selectedPos = null;
        private MainWindow _mainWindow;
        public GameUserControl(MainWindow mainWindow, bool isAI, int difficult = 1)
        {
            InitializeComponent();
            InitializeBoard();
            if (isAI == true) gameState = new GameStateAI(Player.Red, Board.Initial(), difficult);
            else gameState = new GameState2P(Player.Red, Board.Initial());
            DrawBoard(gameState.Board);
            _mainWindow = mainWindow;
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

        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if (pos == null) return;
            if (moveCache.TryGetValue(pos, out Move move))
            {
                HandleMove(move);
            }
        }
        private void DrawNewPos(Canvas canvas,int row,int col)
        {
            SolidColorBrush solidColorBrush = (gameState.Board[row, col].Color == Player.Black) ? Brushes.DarkBlue : Brushes.Red;
            Line topLeftArrow = new Line
            {
                X1 = 5,
                Y1 = 0,
                X2 = 25,
                Y2 = 0,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(topLeftArrow);
            Line sideTopLeftArrow = new Line
            {
                X1 = 5,
                Y1 = 0,
                X2 = 5,
                Y2 = 20,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideTopLeftArrow);
            Line topRightArrow = new Line
            {
                X1 = 55,
                Y1 = 0,
                X2 = 75,
                Y2 = 0,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(topRightArrow);
            Line sideTopRightArrow = new Line
            {
                X1 = 75,
                Y1 = 0,
                X2 = 75,
                Y2 = 20,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideTopRightArrow);
            Line bottomLeftArrow = new Line
            {
                X1 = 5,
                Y1 = 70,
                X2 = 25,
                Y2 = 70,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(bottomLeftArrow);
            Line sideBotLeftArrow = new Line
            {
                X1 = 5,
                Y1 = 70,
                X2 = 5,
                Y2 = 50,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideBotLeftArrow);
            Line bottomRightArrow = new Line
            {
                X1 = 55,
                Y1 = 70,
                X2 = 75,
                Y2 = 70,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(bottomRightArrow);
            Line sideBotRightArrow = new Line
            {
                X1 = 75,
                Y1 = 70,
                X2 = 75,
                Y2 = 50,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideBotRightArrow);
        }
        private void DrawOldPos(Canvas canvas,int row,int col)
        {
            SolidColorBrush solidColorBrush = (gameState.Board[row, col].Color == Player.Black) ? Brushes.DarkBlue : Brushes.Red;
            Line topLeftArrow = new Line
            {
                X1 = 20,
                Y1 = 15,
                X2 = 30,
                Y2 = 15,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(topLeftArrow);
            Line sideTopLeftArrow = new Line
            {
                X1 = 20,
                Y1 = 15,
                X2 = 20,
                Y2 = 25,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideTopLeftArrow);
            Line topRightArrow = new Line
            {
                X1 = 48,
                Y1 = 15,
                X2 = 58,
                Y2 = 15,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(topRightArrow);
            Line sideTopRightArrow = new Line
            {
                X1 = 58,
                Y1 = 15,
                X2 = 58,
                Y2 = 25,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideTopRightArrow);
            Line bottomLeftArrow = new Line
            {
                X1 = 20,
                Y1 = 53,
                X2 = 30,
                Y2 = 53,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(bottomLeftArrow);
            Line sideBotLeftArrow = new Line
            {
                X1 = 20,
                Y1 = 53,
                X2 = 20,
                Y2 = 43,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideBotLeftArrow);
            Line bottomRightArrow = new Line
            {
                X1 = 58,
                Y1 = 53,
                X2 = 48,
                Y2 = 53,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(bottomRightArrow);
            Line sideBotRightArrow = new Line
            {
                X1 = 58,
                Y1 = 53,
                X2 = 58,
                Y2 = 43,
                Stroke = solidColorBrush,
                StrokeThickness = 2
            };
            canvas.Children.Add(sideBotRightArrow);
        }
        private void ShowPrevMove(Move move)
        {
            DrawOldPos(posMoved[move.FromPos.Row, move.FromPos.Column],move.ToPos.Row,move.ToPos.Column);
            DrawNewPos(posMoved[move.ToPos.Row, move.ToPos.Column],move.ToPos.Row,move.ToPos.Column);
        }
        private void HidePrevMove(Move move)
        {
            posMoved[move.FromPos.Row, move.FromPos.Column].Children.Clear();
            posMoved[move.ToPos.Row, move.ToPos.Column].Children.Clear();
        }
        private async void HandleMove(Move move)
        {
            _mainWindow.PlayMoveSound();
            MainGame.IsHitTestVisible = false;
            if (gameState.Moved.Any()) HidePrevMove(gameState.Moved.First().Item1);
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            ShowPrevMove(move);
            await Dispatcher.InvokeAsync(() => { }, System.Windows.Threading.DispatcherPriority.Render);
            if (gameState is GameStateAI AI)
            {
                Move prevMove = gameState.Moved.First().Item1;
                await Task.Run(() => AI.AiMove());
                DrawBoard(gameState.Board);
                HidePrevMove(prevMove);
                ShowPrevMove(gameState.Moved.First().Item1);
                _mainWindow.PlayMoveSound();
            }

            MainGame.IsHitTestVisible = true;

            if (gameState.IsGameOver())
            {
                _mainWindow.CreateGameOverMenu(gameState);
            }
        }

        public event RoutedEventHandler PauseButtonClicked
        {
            add { AddHandler(PauseButtonClickedEvent, value); }
            remove { RemoveHandler(PauseButtonClickedEvent, value); }
        }
        public static readonly RoutedEvent PauseButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "PauseButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(GameUserControl)
        );
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PauseButtonClickedEvent));
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.PlayButtonClickSound();
            if(gameState.Moved.Any()) HidePrevMove(gameState.Moved.First().Item1);
            OnToPositionSelected(selectedPos);
            gameState.UndoMove();
            DrawBoard(gameState.Board);
            if (gameState.Moved.Any())
            {
                ShowPrevMove(gameState.Moved.First().Item1);
            }
        }
    }
}
