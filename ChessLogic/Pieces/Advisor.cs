using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Advisor : Piece
    {
        public override PieceType Type => PieceType.Advisor;
        public override Player Color { get; }
        public Advisor(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Advisor copy = new Advisor(Color);
            copy.HasMoved = false;
            return copy;
        }
    }
}
