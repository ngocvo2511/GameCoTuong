﻿using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    public partial class RoomControl : UserControl
    {
        private HubConnection _connection;

        public RoomControl()
        {
            InitializeComponent();
            InitializeSignalR();
            ShowPlaceholderText(RoomNameTextBox, null);
        }

        private async void InitializeSignalR()
        {
            var connectionManager = SignalRConnectionManager.Instance;
            _connection = connectionManager.Connection;

            _connection.On<string>("RoomCreated", (roomName) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Room {roomName} created successfully.");
                    NavigateToGameOnlineE(roomName);
                });
            });

            _connection.On<string>("RoomJoined", (roomName) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Joined room {roomName} successfully.");
                    NavigateToGameOnlineE(roomName);
                });
            });

            _connection.On<string>("Error", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message);
                });
            });

            await connectionManager.StartConnectionAsync();
        }

        private async void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            string roomName = RoomNameTextBox.Text;
            await _connection.InvokeAsync("CreateRoom", roomName);
        }

        private async void JoinRoomButton_Click(object sender, RoutedEventArgs e)
        {
            string roomName = RoomNameTextBox.Text;
            await _connection.InvokeAsync("JoinRoom", roomName);
        }


        public event RoutedEventHandler NavigateToGameOnline
        {
            add { AddHandler(NavigateToGameOnlineEvent, value); }
            remove { RemoveHandler(NavigateToGameOnlineEvent, value); }
        }

        public static readonly RoutedEvent NavigateToGameOnlineEvent = EventManager.RegisterRoutedEvent(
            "NavigateToGameOnline",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(RoomControl)
        );

        private void NavigateToGameOnlineE(string RoomName)
        {
            RaiseEvent(new NavigateToGameOnlineEventArgs(NavigateToGameOnlineEvent, RoomName));
        }
        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == string.Empty)
            {
                PlaceholderTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == string.Empty)
            {
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}