﻿using ChessLogic.GameStates.GameState;
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
        private Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private GameState gameState;
        private Position selectedPos = null;
        public GameUserControl(bool isAI,int difficult=1)
        {
            InitializeComponent();
            InitializeBoard();
            if (isAI == true) gameState = new GameStateAI(Player.Red, Board.Initial(), difficult);
            else gameState = new GameState2P(Player.Red, Board.Initial());
            DrawBoard(gameState.Board);
            //settingsMenu.BackButtonClicked += BackButtonClicked;
            //selectGameModeMenu.BackButtonClicked += BackButtonClicked;
        }
        //private void BackButtonClicked(object sender, EventArgs e)
        //{
        //    pauseMenu.Visibility = Visibility.Visible;
        //}

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
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };

                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight);
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

        private async void HandleMove(Move move)
        {
            MainGame.IsHitTestVisible = false;

            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            await Dispatcher.InvokeAsync(() => { }, System.Windows.Threading.DispatcherPriority.Render);
            if (gameState is GameStateAI AI)
            {
                await Task.Run(() => AI.AiMove());
                DrawBoard(gameState.Board);
            }

            MainGame.IsHitTestVisible = true;
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
            OnToPositionSelected(selectedPos);
            gameState.UndoMove();
            DrawBoard(gameState.Board);
        }
    }
}