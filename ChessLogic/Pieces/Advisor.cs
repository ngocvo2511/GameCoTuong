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
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.SouthEast, Direction.SouthWest, Direction.NorthEast, Direction.NorthWest
        };

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (Direction dir in dirs)
            {
                Position to = from + dir;
                if (!Board.IsInside(to))
                {
                    continue;
                }

                if (Board.IsInPalace(to, Color) && (board.IsEmpty(to) || board[to].Color != Color))
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }
        }
    }
}
