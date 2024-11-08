using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Soldier : Piece
    {
        public override PieceType Type => PieceType.Soldier;
        public override Player Color { get; }
        public Soldier(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Soldier copy = new Soldier(Color);
            copy.HasMoved = false;
            return copy;
        }
    }
}
