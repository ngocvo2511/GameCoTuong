using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates.GameState
{
    public class GameState2P:GameState
    {
        public GameState2P(Player player, Board board,int timeLimit=0) : base(player, board,timeLimit) { }
        public override void UndoMove()
        {
            if (!Moved.Any()) return;
            var undo = Moved.Pop();
            Move undoMove = new NormalMove(undo.Item1.ToPos, undo.Item1.FromPos);
            undoMove.Execute(Board);
            Board[undo.Item1.ToPos] = undo.Item2;
            CurrentPlayer = CurrentPlayer.Opponent();
            CapturedPiece = undo.Item2;
        }
    }
}
