using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPos { get; }
        public abstract Position ToPos { get; }
        public abstract void Execute(Board board);

        public virtual bool IsLegal(Board board) // tra ve true neu viec thuc hien nuoc di nay khong lam cho vua bi chieu
        {
            
        }
    }
}
