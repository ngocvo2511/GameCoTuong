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
        public Stack<Tuple<Move,Piece>> Moved { get; set; }
        public Player CurrentPlayer { get; protected set; }
        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
            Moved = new Stack<Tuple<Move, Piece>>();
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
    }
}
