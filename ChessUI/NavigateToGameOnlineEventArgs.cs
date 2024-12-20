using ChessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessUI
{
    public class NavigateToGameOnlineEventArgs : RoutedEventArgs
    {
        public string RoomName { get; }
        public string Username { get; }
        public Player Color { get; }
        public int Time { get; }

        public NavigateToGameOnlineEventArgs(RoutedEvent routedEvent, string roomName, string username, Player color, int time) : base(routedEvent)
        {
            RoomName = roomName;
            Username = username;
            Color = color;
            Time = time;
        }
    }
}
