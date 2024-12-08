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
        public Player Color { get; }

        public NavigateToGameOnlineEventArgs(RoutedEvent routedEvent, string roomName, Player color) : base(routedEvent)
        {
            RoomName = roomName;
            Color = color;
        }
    }
}
