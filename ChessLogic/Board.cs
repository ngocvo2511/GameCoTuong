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
    }
}
