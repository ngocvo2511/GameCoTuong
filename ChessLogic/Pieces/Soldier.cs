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

        private readonly Direction forward;
        public Soldier(Player color)
        {
            Color = color;
            if (color == Player.Red)
            {
                forward = Direction.North;
            }
            else if (color == Player.Black)
            {
                forward = Direction.South;
            }
        }
        public override Piece Copy()
        {
            Soldier copy = new Soldier(Color);
            copy.HasMoved = false;
            return copy;
        }

        private bool IsCrossedRiver(Position pos)
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

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            Position to = from + forward;
            if(Board.IsInside(to) && (board.IsEmpty(to) || board[to].Color != Color))
            {
                yield return to;
            }

            if (IsCrossedRiver(from))
            {
                foreach(Direction dir in new Direction[] { Direction.East, Direction.West })
                {
                    Position sideTo = from + dir;
                    if(Board.IsInside(sideTo) && (board.IsEmpty(sideTo) || board[sideTo].Color != Color))
                    {
                        yield return sideTo;
                    }
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
