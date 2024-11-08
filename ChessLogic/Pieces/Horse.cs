using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Horse : Piece
    {
        public override PieceType Type => PieceType.Horse;
        public override Player Color { get; }
        public Horse(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Horse copy = new Horse(Color);
            copy.HasMoved = false;
            return copy;
        }
    }
}
