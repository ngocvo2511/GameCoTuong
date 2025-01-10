namespace ChessLogic
{
    public class General : Piece
    {
        public override PieceType Type => PieceType.General;
        public override Player Color { get; }
        public General(Player color, Player BottomPlayer = Player.Red)
        {
            Color = color;
            bottomPlayer = BottomPlayer;
        }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North, Direction.South, Direction.East, Direction.West
        };
        public override Piece Copy()
        {
            General copy = new General(Color, bottomPlayer);
            copy.HasMoved = false;
            return copy;
        }

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
                if (IsExposedToOpponentKing(to, board))
                {
                    continue;
                }
                yield return new NormalMove(from, to);
            }
        }

        private bool IsExposedToOpponentKing(Position to, Board board)
        {
            Position opponentKingPos = FindOpponentKing(board);
            if (opponentKingPos == null || opponentKingPos.Column != to.Column)
            {
                return false;
            }
            int rowStep = opponentKingPos.Row > to.Row ? 1 : -1;
            for (int row = to.Row + rowStep; row != opponentKingPos.Row; row += rowStep)
            {
                if (!board.IsEmpty(new Position(row, to.Column)))
                {
                    return false;
                }
            }
            return true;
        }


        private Position FindOpponentKing(Board board)
        {
            if (bottomPlayer == Player.Red)
            {
                if (Color == Player.Red)
                {
                    for (int r = 0; r <= 2; r++)
                    {
                        for (int c = 3; c <= 5; c++)
                        {
                            Position pos = new Position(r, c);
                            Piece piece = board[pos];
                            if (piece != null && piece.Type == PieceType.General)
                            {
                                return pos;
                            }
                        }
                    }
                    return null;
                }
                else if (Color == Player.Black)
                {
                    for (int r = 7; r <= 9; r++)
                    {
                        for (int c = 3; c <= 5; c++)
                        {
                            Position pos = new Position(r, c);
                            Piece piece = board[pos];
                            if (piece != null && piece.Type == PieceType.General)
                            {
                                return pos;
                            }
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (Color == Player.Black)
                {
                    for (int r = 0; r <= 2; r++)
                    {
                        for (int c = 3; c <= 5; c++)
                        {
                            Position pos = new Position(r, c);
                            Piece piece = board[pos];
                            if (piece != null && piece.Type == PieceType.General)
                            {
                                return pos;
                            }
                        }
                    }
                    return null;
                }
                else if (Color == Player.Red)
                {
                    for (int r = 7; r <= 9; r++)
                    {
                        for (int c = 3; c <= 5; c++)
                        {
                            Position pos = new Position(r, c);
                            Piece piece = board[pos];
                            if (piece != null && piece.Type == PieceType.General)
                            {
                                return pos;
                            }
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public override bool CanCaptureOpponentGeneral(Position from, Board board)
        {
            return IsExposedToOpponentKing(from, board);
        }
    }
}
