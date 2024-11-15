using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates.GameState
{
    public class GameState2P:GameState
    {
        public GameState2P(Player player, Board board) : base(player, board) { }
        public override void UndoMove()
        {
            if (!Moved.Any()) return;
            Move move = Moved.Pop();
            Move undoMove = new NormalMove(move.ToPos,move.FromPos);
            undoMove.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
        }
    }
}
