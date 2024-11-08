﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Cannon : Piece
    {
        public override PieceType Type => PieceType.Cannon;
        public override Player Color { get; }
        public Cannon(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Cannon copy = new Cannon(Color);
            copy.HasMoved = false;
            return copy;
        }
    }
}
