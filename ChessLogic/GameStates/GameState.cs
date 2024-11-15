using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates.GameState
{
    public abstract class GameState
    {
        public Board Board { get; }
        public Stack<Move> Moved { get; set; }
        public Player CurrentPlayer { get; protected set; }
        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
            Moved = new Stack<Move>();
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

        public virtual void MakeMove(Move move)
        {
            move.Execute(Board);
            Moved.Push(move);
            CurrentPlayer = CurrentPlayer.Opponent();
        }
        public abstract void UndoMove();
    }
}
