using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic.GameStates
{
    public class GameStateForLoad
    {
        public string GameType { get; set; }
        public Board Board { get; set; }
        public Stack<Tuple<Move, Piece>> Moved { get; set; }
        public Player CurrentPlayer { get; set; }
        public int depth {  get; set; }
        public int timeRemainingRed {  get; set; }
        public int timeRemainingBlack { get; set; }
        public Dictionary<string, int> stateHistory = new Dictionary<string, int>();
    }
}
