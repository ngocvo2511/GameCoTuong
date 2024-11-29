using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[10, 9];
        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }
        public Piece this[Position pos]
        {
            get { return pieces[pos.Row, pos.Column]; }
            set { pieces[pos.Row, pos.Column] = value; }
        }
        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }
        private void AddStartPieces()
        {
            this[0, 0] = new Chariot(Player.Black);
            this[0, 1] = new Horse(Player.Black);
            this[0, 2] = new Elephant(Player.Black);
            this[0, 3] = new Advisor(Player.Black);
            this[0, 4] = new General(Player.Black);
            this[0, 5] = new Advisor(Player.Black);
            this[0, 6] = new Elephant(Player.Black);
            this[0, 7] = new Horse(Player.Black);
            this[0, 8] = new Chariot(Player.Black);
            this[2, 1] = new Cannon(Player.Black);
            this[2, 7] = new Cannon(Player.Black);
            this[3, 0] = new Soldier(Player.Black);
            this[3, 2] = new Soldier(Player.Black);
            this[3, 4] = new Soldier(Player.Black);
            this[3, 6] = new Soldier(Player.Black);
            this[3, 8] = new Soldier(Player.Black);

            this[9, 0] = new Chariot(Player.Red);
            this[9, 1] = new Horse(Player.Red);
            this[9, 2] = new Elephant(Player.Red);
            this[9, 3] = new Advisor(Player.Red);
            this[9, 4] = new General(Player.Red);
            this[9, 5] = new Advisor(Player.Red);
            this[9, 6] = new Elephant(Player.Red);
            this[9, 7] = new Horse(Player.Red);
            this[9, 8] = new Chariot(Player.Red);
            this[7, 1] = new Cannon(Player.Red);
            this[7, 7] = new Cannon(Player.Red);
            this[6, 0] = new Soldier(Player.Red);
            this[6, 2] = new Soldier(Player.Red);
            this[6, 4] = new Soldier(Player.Red);
            this[6, 6] = new Soldier(Player.Red);
            this[6, 8] = new Soldier(Player.Red);
        }
        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Column >= 0 && pos.Row < 10 && pos.Column < 9;
        }
        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }
        public static bool IsInPalace(Position pos, Player color)
        {
            if (color == Player.Red)
            {
                return pos.Row >= 7 && pos.Row <= 9 && pos.Column >= 3 && pos.Column <= 5;
            }
            else if (color == Player.Black)
            {
                return pos.Row >= 0 && pos.Row <= 2 && pos.Column >= 3 && pos.Column <= 5;
            }
            else return false;
        }

        public IEnumerable<Position> PiecePositions() //lay vi tri tat ca quan co
        {
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Position pos = new Position(row, col);

                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionFor(Player player) // lay vi tri tat ca quan co cua 1 nguoi choi
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInCheck(Player player) // kiem tra xem co bi chieu hay khong
        {
            return PiecePositionFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentGeneral(pos, this);
            });
        }

        public Board Copy()
        {
            Board copy = new Board();

            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }
            return copy;
        }

        public Counting CountPieces()
        {
            Counting counting = new Counting();
            foreach (Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }
            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();
            return IsNoAttackPiece(counting) || IsKingHorseVKing(counting) || IsKingCannonVKing(counting);
        }

        private static bool IsNoAttackPiece(Counting counting)
        {
            return counting.CountingAttackPieces(Player.Red) == 0 || counting.CountingAttackPieces(Player.Black) == 0;
        }

        private static bool IsKingHorseVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.Red(PieceType.Horse) == 1 || counting.Black(PieceType.Horse) == 1);
        }

        private static bool IsKingCannonVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.Red(PieceType.Cannon) == 1 || counting.Black(PieceType.Cannon) == 1);
        }

        
    }
}
