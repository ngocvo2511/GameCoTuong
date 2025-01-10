namespace ChessLogic
{
    public class Horse : Piece
    {
        public override PieceType Type => PieceType.Horse;
        public override Player Color { get; }
        public Horse(Player color, Player BottomPlayer = Player.Red)
        {
            Color = color;
            bottomPlayer = BottomPlayer;
        }
        public override Piece Copy()
        {
            Horse copy = new Horse(Color, bottomPlayer);
            copy.HasMoved = false;
            return copy;
        }

        private static IEnumerable<Position> PotentialToPositions(Position from, Board board)
        {
            foreach (Direction dir in new Direction[] { Direction.North, Direction.South })
            {

                Position toPos = from + dir;
                if (Board.IsInside(toPos) && board.IsEmpty(toPos)) //khong co quan chan
                {
                    yield return from + 2 * dir + Direction.West;
                    yield return from + 2 * dir + Direction.East;
                }
            }

            foreach (Direction dir in new Direction[] { Direction.East, Direction.West })
            {

                Position toPos = from + dir;
                if (Board.IsInside(toPos) && board.IsEmpty(toPos)) //khong co quan chan
                {
                    yield return from + 2 * dir + Direction.North;
                    yield return from + 2 * dir + Direction.South;
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
