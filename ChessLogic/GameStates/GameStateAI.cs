using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates.GameState
{
    public class GameStateAI:GameState
    {
        private int depth;
        public GameStateAI(Player player, Board board,int depth) : base(player, board)
        {
            this.depth = depth;
        }
        public override void MakeMove(Move move)
        {
            base.MakeMove(move);
            AiMove();
        }
        public override void UndoMove()
        {
            if (!Moved.Any() || CurrentPlayer == Player.Black) return;
            for (int i = 0; i < 2; i++)
            {
                Move move = Moved.Pop();
                Move undoMove = new NormalMove(move.ToPos, move.FromPos);
                undoMove.Execute(Board);
            }
            CurrentPlayer = Player.Red;
        }
        void AiMove()
        {

        }
    }
}
