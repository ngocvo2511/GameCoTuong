namespace ChessLogic.GameStates
{
    public class GameStateForLoad
    {
        public string GameType { get; set; }
        public Board Board { get; set; }
        public Stack<Tuple<Move, Piece>> Moved { get; set; }
        public Player CurrentPlayer { get; set; }
        public Stack<int> noCapture { get; set; }
        public int depth { get; set; }
        public int timeRemainingRed { get; set; }
        public int timeRemainingBlack { get; set; }
        public Dictionary<string, int> stateHistory = new Dictionary<string, int>();
        public Stack<string> stateString = new Stack<string>();
        public List<Piece> CapturedRedPiece { get; set; }
        public List<Piece> CapturedBlackPiece { get; set; }
    }
}
