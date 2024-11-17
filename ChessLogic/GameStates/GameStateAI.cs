using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates.GameState
{
    public class GameStateAI:GameState
    {
       
        public GameStateAI(Player player, Board board,int depth) : base(player, board)
        {
            this.depth = depth;
        }
        public override void MakeMove(Move move)
        {
            base.MakeMove(move);
            // Nếu chiếu bí??
           
        }
        public override void UndoMove()
        {
            if (Moved.Count<=1 || CurrentPlayer == Player.Black) return;
            for (int i = 0; i < 2; i++)
            {
                var undo = Moved.Pop();
                Move undoMove = new NormalMove(undo.Item1.ToPos, undo.Item1.FromPos);
                undoMove.Execute(Board);
                Board[undo.Item1.ToPos] = undo.Item2;
            }
            CurrentPlayer = Player.Red;
        }
        
    }
}
