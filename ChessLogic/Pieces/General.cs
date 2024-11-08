using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class General : Piece
    {
        public override PieceType Type => PieceType.General;
        public override Player Color { get; }
        public General(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            General copy = new General(Color);
            copy.HasMoved = false;
            return copy;
        }
    }
}
