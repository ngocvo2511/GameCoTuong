using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates.GameState
{
    public class GameStateAI : GameState
    {
        public int depth { get; set; }
        private ValuePiece value;
        public GameStateAI(Player player, Board board, int depth) : base(player, board)
        {
            this.depth = depth;
            value = new ValuePiece();
        }
        public override void UndoMove()
        {
            if (Moved.Count <= 1 || CurrentPlayer == Player.Black) return;
            for (int i = 0; i < 2; i++)
            {
                var undo = Moved.Pop();
                Move undoMove = new NormalMove(undo.Item1.ToPos, undo.Item1.FromPos);
                undoMove.Execute(Board);
                Board[undo.Item1.ToPos] = undo.Item2;
            }
            CurrentPlayer = Player.Red;
        }
        private void MakeTestMove(Move move)
        {
            Moved.Push(Tuple.Create(move, Board[move.ToPos]));
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
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
            IEnumerable<Move> moves = AllLegalMovesFor(CurrentPlayer);
            if (!moves.Any()) return;
            Move bestMove = moves.ElementAt(0);
            int value;
            int bestValue = -9999;
            foreach (var move in moves)
            {
                MakeTestMove(move);
                value = AlphaBeta(depth - 1);
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
            IEnumerable<Move> moves = AllLegalMovesFor(CurrentPlayer);
            if (CurrentPlayer == Player.Black)
            {
                int bestValue = -9999;                
                foreach (var move in moves)
                {
                    MakeTestMove(move);
                    bestValue = Math.Max(bestValue, AlphaBeta(depth - 1, alpha, beta));
                    UndoTestMove();
                    beta = Math.Min(beta, bestValue);
                    if (alpha >= beta) return bestValue;
                }
                return bestValue;
            }
            else if (CurrentPlayer == Player.Red)
            {
                int bestValue = 9999;
                foreach (var move in moves)
                {
                    MakeTestMove(move);
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
