using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates.GameState
{
    public abstract class GameState
    {
        public Board Board { get; }
        public Stack<Tuple<Move, Piece>> Moved { get; set; }
        public Player CurrentPlayer { get; protected set; }

        public Result Result { get; protected set; } = null;

        private int noCapture = 0;
        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
            Moved = new Stack<Tuple<Move, Piece>>();
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos];
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void MakeMove(Move move)
        {
            Moved.Push(Tuple.Create(move, Board[move.ToPos]));
            bool capture = move.Execute(Board);

            if (capture)
            {
                noCapture = 0;
            }
            else
            {
                noCapture++;
            }
            CurrentPlayer = CurrentPlayer.Opponent();
            CheckForGameOver();
        }
        public abstract void UndoMove();

        public IEnumerable<Move> AllLegalMovesFor(Player player)  // nước đi khả thi của người chơi
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionFor(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                if (Board.IsInCheck(CurrentPlayer))
                {
                    Result = Result.Win(CurrentPlayer.Opponent(), EndReason.Checkmate);
                }
                else
                {
                    Result = Result.Win(CurrentPlayer.Opponent(), EndReason.Stalemate);
                }
            }
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
            else if (FiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
        }

        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool FiftyMoveRule()
        {
            return noCapture >= 100;
        }
    }
}
