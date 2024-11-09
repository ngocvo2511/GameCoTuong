using System;
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

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North, Direction.South, Direction.East, Direction.West
        };
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

        private IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            bool foundObstacle = false;

            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                if (board.IsEmpty(pos))
                {
                    if (!foundObstacle) // chua gap quan can
                    {
                        yield return pos; continue;
                    }   
                }
                else
                {
                    if (!foundObstacle) // gap quan can dau tien
                    {
                        foundObstacle = true;
                    }
                    else // gap quan can thu hai
                    {
                        Piece piece = board[pos];
                        if (piece.Color != Color)
                        {
                            yield return pos;
                        }
                        yield break;
                    }
                }                
            }
        }

        private IEnumerable<Position> MovePositionsInDirs(Position from, Board board, Direction[] dirs)
        {
            return dirs.SelectMany(dirs => MovePositionsInDir(from, board, dirs));
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
