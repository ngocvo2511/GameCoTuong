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
        public Elephant(Player color, Player BottomPlayer = Player.Red)
        {
            Color = color;
            bottomPlayer = BottomPlayer;
        }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.SouthEast, Direction.SouthWest, Direction.NorthEast, Direction.NorthWest
        };
        public override Piece Copy()
        {
            Elephant copy = new Elephant(Color, bottomPlayer);
            copy.HasMoved = false;
            return copy;
        }

        private bool IsCrossedRiver(Position pos)
        {
            if (bottomPlayer == Player.Red)
            {
                if (Color == Player.Red)
                {
                    return pos.Row < 5;
                }
                else if (Color == Player.Black)
                {
                    return pos.Row > 4;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Color == Player.Red)
                {
                    return pos.Row > 4;
                }
                else if (Color == Player.Black)
                {
                    return pos.Row < 5;
                }
                else
                {
                    return false;
                }
            }
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (Direction dir in dirs)
            {
                Position middlePos = from + dir;
                if (!Board.IsInside(middlePos) || !board.IsEmpty(middlePos) || IsCrossedRiver(middlePos))
                {
                    continue;
                }

                Position to = middlePos + dir;
                if (Board.IsInside(to) && (board.IsEmpty(to) || board[to].Color != Color))
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
