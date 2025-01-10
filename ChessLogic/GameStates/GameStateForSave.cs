namespace ChessLogic.GameStates
{
    public class GameStateForSave
    {
        public string GameType { get; set; }
        public List<string> Board { get; set; }
        public string CurrentPlayer { get; set; }
        public List<int> noCapture { get; set; }
        public int? depth { get; set; }
        public List<string> Moved { get; set; }
        public List<string> stateHistory { get; set; }
        public List<string> stateString { get; set; }
        public int timeRemainingRed { get; set; }
        public int timeRemainingBlack { get; set; }
        public List<string> CapturedRedPiece { get; set; }
        public List<string> CapturedBlackPiece { get; set; }
    }
}
