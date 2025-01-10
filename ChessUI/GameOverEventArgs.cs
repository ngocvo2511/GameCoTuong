using ChessLogic;
using System.Windows;

namespace ChessUI
{
    public class GameOverEventArgs : RoutedEventArgs
    {
        public Result result;
        public Player currentPlayer;
        public GameOverEventArgs(RoutedEvent routedEvent, Result result, Player currentPlayer) : base(routedEvent)
        {
            this.result = result;
            this.currentPlayer = currentPlayer;
        }
    }
}
