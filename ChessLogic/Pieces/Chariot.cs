namespace ChessLogic
{
    public class Chariot : Piece
    {
        public override PieceType Type => PieceType.Chariot;
        public override Player Color { get; }
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North, Direction.South, Direction.East, Direction.West
        };
        public Chariot(Player color, Player BottomPlayer = Player.Red)
        {
            Color = color;
            bottomPlayer = BottomPlayer;
        }
        public override Piece Copy()
        {
            Chariot copy = new Chariot(Color, bottomPlayer);
            copy.HasMoved = false;
            return copy;
        }

        private IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }

                Piece piece = board[pos];
                if (piece.Color != Color)
                {
                    yield return pos;
                }
                yield break;
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
