using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Elephant : Piece
    {
        public override PieceType Type => PieceType.Elephant;
        public override Player Color { get; }
        public Elephant(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Elephant copy = new Elephant(Color);
            copy.HasMoved = false;
            return copy;
        }
    }
}
