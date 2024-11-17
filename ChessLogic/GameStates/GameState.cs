using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates.GameState
{
    public abstract class GameState
    {
        protected int depth;
        protected ValuePiece value;
        public Board Board { get; }
        public Stack<Tuple<Move,Piece>> Moved { get; set; }
        public Player CurrentPlayer { get; protected set; }
        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
            Moved = new Stack<Tuple<Move, Piece>>();
            value = new ValuePiece();
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos];
            return piece.GetMoves(pos, Board);
        }

        public void MakeMove(Move move)
        {
            Moved.Push(Tuple.Create(move, Board[move.ToPos]));
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
        }
        public abstract void UndoMove();

        public IEnumerable<Move> GetAllMove(Player player) // nước đi khả thi của người chơi
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (Board[i, j] != null && Board[i, j].Color == player)
                    {
                        foreach (var move in LegalMovesForPiece(new Position(i, j)))
                        {
                            yield return move;
                        }
                    }
                }
            }
        }
        private void UndoTestMove()
        {
            if (!Moved.Any()) return;
            var undo = Moved.Pop();
            Move undoMove = new NormalMove(undo.Item1.ToPos, undo.Item1.FromPos);
            undoMove.Execute(Board);
            Board[undo.Item1.ToPos] = undo.Item2;
            CurrentPlayer = CurrentPlayer.Opponent();
        }
        public void AiMove()
        {
            IEnumerable<Move> moves = GetAllMove(CurrentPlayer);
            if (!moves.Any()) return;
            Move bestMove = moves.ElementAt(0);
            int bestValue = -9999;
            foreach (var move in moves)
            {
                MakeMove(move);
                int value = AlphaBeta(depth);
                UndoTestMove();
                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }
            MakeMove(bestMove);
        }
        private int AlphaBeta(int depth, int alpha = -9999, int beta = 9999) // giá trị nước đi
        {
            if (depth == 0) return value.GetValueBoard(Board);
            if (CurrentPlayer == Player.Black)
            {
                int bestValue = 9999;
                IEnumerable<Move> moves = GetAllMove(CurrentPlayer);
                foreach (var move in moves)
                {
                    MakeMove(move);
                    bestValue = Math.Max(bestValue, AlphaBeta(depth - 1, alpha, beta));
                    UndoTestMove();
                    beta = Math.Min(beta, bestValue);
                    if (alpha >= beta) return bestValue;
                }
                return bestValue;
            }
            else if (CurrentPlayer == Player.Red)
            {
                int bestValue = -9999;
                IEnumerable<Move> moves = GetAllMove(CurrentPlayer);
                foreach (var move in moves)
                {
                    MakeMove(move);
                    bestValue = Math.Min(bestValue, AlphaBeta(depth - 1, alpha, beta));
                    UndoTestMove();
                    alpha = Math.Max(alpha, bestValue);
                    if (alpha >= beta) return bestValue;
                }
                return bestValue;
            }
            else return value.GetValueBoard(Board);
        }
    }
}
