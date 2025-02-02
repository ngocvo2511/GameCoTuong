﻿using ChessLogic;
using System.Windows;

namespace ChessUI
{
    public class NavigateToGameOnlineEventArgs : RoutedEventArgs
    {
        public string RoomName { get; }
        public string Username { get; }
        public string OpponentUsername { get; }
        public Player Color { get; }
        public int Time { get; }

        public NavigateToGameOnlineEventArgs(RoutedEvent routedEvent, string roomName, string username, Player color, int time, string opponentUsername = "") : base(routedEvent)
        {
            RoomName = roomName;
            Username = username;
            Color = color;
            Time = time;
            OpponentUsername = opponentUsername;
        }
    }
}
