using System.Text;

namespace ChessLogic
{
    public class StateString
    {
        private readonly StringBuilder sb = new StringBuilder();

        public StateString(Player currentPlayer, Board board)
        {
            AddPiecePlacement(board);
            sb.Append(' ');
            AddCurrentPlayer(currentPlayer);
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        private static char PieceChar(Piece piece)
        {
            char c = piece.Type switch
            {
                PieceType.General => 'g',
                PieceType.Advisor => 'a',
                PieceType.Elephant => 'e',
                PieceType.Horse => 'h',
                PieceType.Chariot => 'c',
                PieceType.Cannon => 'n',
                PieceType.Soldier => 's',
                _ => ' '
            };

            if (piece.Color == Player.Red)
            {
                return char.ToUpper(c);
            }
            return c;
        }

        private void AddRowData(Board board, int row)
        {
            int empty = 0;

            for (int c = 0; c < 9; c++)
            {
                if (board[row, c] == null)
                {
                    empty++;
                    continue;
                }
                
                if(empty > 0)
                {
                    sb.Append(empty);
                    empty = 0;
                }

                sb.Append(PieceChar(board[row, c]));
            }

            if(empty > 0)
            {
                sb.Append(empty);
            }
        }

        private void AddPiecePlacement(Board board)
        {
            for (int r = 0; r < 10; r++)
            {
                if(r != 0)
                {
                    sb.Append('/');
                }
                AddRowData(board, r);
            }
        }

        private void AddCurrentPlayer(Player currentPlayer)
        {
            if(currentPlayer == Player.Red)
            {
                sb.Append("r");
            }
            else
            {
                sb.Append("b");
            }
        }
    }
}

