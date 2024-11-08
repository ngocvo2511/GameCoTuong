using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Chariot : Piece
    {
        public override PieceType Type => PieceType.Chariot;
        public override Player Color { get; }
        public Chariot(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Chariot copy = new Chariot(Color);
            copy.HasMoved = false;
            return copy;
        }
    }
}
