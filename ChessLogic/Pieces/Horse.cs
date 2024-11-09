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

        private static IEnumerable<Position> PotentialToPositions(Position from, Board board)
        {
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hDir in new Direction[] { Direction.West, Direction.East })
                {
                    Position toPos1 = from + vDir;
                    Position toPos2 = from + hDir;
                    if (board.IsEmpty(toPos1)) //co the di theo huong nay
                    {
                        yield return from + 2 * vDir + hDir;
                    }

                    if (board.IsEmpty(toPos2)) //co the di theo huong nay
                    {
                        yield return from + 2 * hDir + vDir;
                    }

                }
            }
        }
        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            return PotentialToPositions(from, board).Where(pos => Board.IsInside(pos) && (board.IsEmpty(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositions(from, board).Select(to => new NormalMove(from, to));
        }
    }
}
