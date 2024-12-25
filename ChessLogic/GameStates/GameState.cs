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
        public Player CurrentPlayer { get; set; }

        public Result Result { get; protected set; } = null;

        public Piece CapturedPiece { get; set; }
        public int timeRemainingRed { get; set; }
        public int timeRemainingBlack {  get; set; }
        public List<Piece> CapturedRedPiece { get; set; }
        public List<Piece> CapturedBlackPiece { get; set; }
        public Stack<int> noCapture { get; set; }

        private string stateString;
        private readonly Dictionary<string, int> stateHistory;
        public GameState(Player player, Board board, int timeLimit)
        {
            CurrentPlayer = player;
            Board = board;
            this.Moved = new Stack<Tuple<Move, Piece>>();
            this.CapturedBlackPiece = new List<Piece>();
            this.CapturedRedPiece = new List<Piece>();
            this.noCapture = new Stack<int>();
            this.stateHistory = new Dictionary<string, int>();
            stateString = new StateString(player, board).ToString();
            this.stateHistory[stateString] = 1;
            timeRemainingBlack = timeLimit;
            timeRemainingRed = timeLimit;
        }
        public GameState(Player player,Board board,int redTime,int blackTime,Stack<Tuple<Move, Piece>> Moved,Dictionary<string,int> stateHistory
            ,List<Piece> CapturedRedPiece,List<Piece> CapturedBlackPiece,Stack<int> noCapture)
        {
            CurrentPlayer = player;
            Board = board;
            this.Moved = Moved;
            this.stateHistory = stateHistory;
            this.CapturedBlackPiece = CapturedBlackPiece;
            this.CapturedRedPiece = CapturedRedPiece;
            timeRemainingBlack = blackTime;
            timeRemainingRed = redTime;
            this.noCapture = noCapture;
        }
        public List<string> getStateHistory()
        {
            List<string> history = new List<string>();
            foreach (var state in stateHistory)
            {
                history.Add(state.Key);
                history.Add($"{state.Value}");
            }
            return history;
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
            CapturedPiece = Board[move.ToPos];
            if(CapturedPiece != null)
            {
                if(CapturedPiece.Color==Player.Black) CapturedBlackPiece.Add(CapturedPiece);
                else CapturedRedPiece.Add(CapturedPiece);
            }
            bool capture = move.Execute(Board);

            if (capture)
            {
                noCapture.Push(0);
                stateHistory.Clear();
            }
            else
            {
                if (noCapture.Count == 0) noCapture.Push(1);
                else noCapture.Push(noCapture.Peek()+1);
            }
            CurrentPlayer = CurrentPlayer.Opponent();
            UpdateStateString();
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
            else if (ThreefoldRepetition())
            {
                Result = Result.Draw(EndReason.ThreefoldRepetition);
            }
        }

        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool FiftyMoveRule()
        {
            return noCapture.Peek() >= 100;
        }

        private void UpdateStateString()
        {
            stateString = new StateString(CurrentPlayer, Board).ToString();

            if (!stateHistory.ContainsKey(stateString))
            {
                stateHistory[stateString] = 1;
            }
            else
            {
                stateHistory[stateString]++;
            }
        }
       
        private bool ThreefoldRepetition()
        {
            return stateHistory[stateString] == 3;
        }
        public void TimeForfeit()
        {
            Result = Result.Win(CurrentPlayer.Opponent(), EndReason.TimeForfeit);
        }
    }
}
