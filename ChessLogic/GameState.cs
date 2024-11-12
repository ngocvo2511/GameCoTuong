using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; }
        public bool isAI {  get;}
        public Stack<Move> Moved { get; set; }
        public Player CurrentPlayer { get; private set; }
        public GameState(Player player, Board board, bool isAI=false)
        {
            CurrentPlayer = player;
            Board = board;
            this.isAI = isAI;
            Moved=new Stack<Move>();
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
            move.Execute(Board);
            Moved.Push(move);
            CurrentPlayer = CurrentPlayer.Opponent();
            if(isAI == true && CurrentPlayer == Player.Black)
            {
                //AI move
            }
        }
        public void UndoMove(bool testMove=false)
        {
            if (isAI == true && CurrentPlayer == Player.Black) return;
            if (!Moved.Any()) return;
            int i;
            if (testMove == false && isAI == true && CurrentPlayer == Player.Red) i = 2;
            else i = 1;
            for(int j = 0; j < i; j++)
            {
                Move move = Moved.Pop();
                Move undoMove = new NormalMove(move.ToPos, move.FromPos);
                undoMove.Execute(Board);
                CurrentPlayer = CurrentPlayer.Opponent();
            }
        }
    }
}
