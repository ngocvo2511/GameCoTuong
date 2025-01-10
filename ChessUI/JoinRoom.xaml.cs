using ChessLogic;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for JoinRoom.xaml
    /// </summary>
    public partial class JoinRoom : UserControl
    {
        private HubConnection _connection;
        private DispatcherTimer _notificationTimer;

        public JoinRoom()
        {
            InitializeComponent();
            InitializeNotificationTimer();
            InitializeSignalR();
            ShowPlaceholderText(RoomNameTextBox, null);
            ShowPlaceholderText1(UsernameTextBox, null);
        }

        private void InitializeNotificationTimer()
        {
            _notificationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2.5) 
            };
            _notificationTimer.Tick += (s, e) =>
            {
                HideNotification();
                _notificationTimer.Stop();
            };
        }

        private void ShowNotification(string message)
        {
            if (NotificationPanel.Child is TextBlock textBlock)
            {
                textBlock.Text = message;
            }

            NotificationPanel.Visibility = Visibility.Visible;

            var fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            NotificationPanel.BeginAnimation(OpacityProperty, fadeInAnimation);

            _notificationTimer.Start();
        }

        private void HideNotification()
        {
            var fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += (s, e) => NotificationPanel.Visibility = Visibility.Collapsed;
            NotificationPanel.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }



        private async void InitializeSignalR()
        {
            var connectionManager = SignalRConnectionManager.Instance;
            _connection = connectionManager.Connection;

            _connection.Remove("RoomJoined");
            _connection.Remove("Error");

            _connection.On<string, string, int, string>("RoomJoined", (roomName, username, time, opponentUsername) =>
            {
                Dispatcher.Invoke(() =>
                {
                    NavigateToGameOnlineE(roomName, username, Player.Black, time, opponentUsername);
                });
            });

            _connection.On<string>("Error", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    ShowNotification(message);
                });
            });

            await connectionManager.StartConnectionAsync();
        }

        private async void JoinRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            if (string.IsNullOrEmpty(RoomNameTextBox.Text))
            {
                ShowNotification("Vui lòng nhập tên phòng");
                return;
            }
            else if (_connection.State != HubConnectionState.Connected)
            {
                ShowNotification("Không thể kết nối đến server, vui lòng kiểm tra kết nối mạng.");
                return;
            }
            string roomName = RoomNameTextBox.Text;
            string username;
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                username = "Người chơi 2";
            }
            else
            {
                username = UsernameTextBox.Text;
            }
            await _connection.InvokeAsync("JoinRoom", roomName, username);
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
            typeof(JoinRoom)
        );

        private void NavigateToGameOnlineE(string RoomName, string Username, Player Color, int Time, string OpponentUsername)
        {
            RaiseEvent(new NavigateToGameOnlineEventArgs(NavigateToGameOnlineEvent, RoomName, Username, Color, Time, OpponentUsername));
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

        public static readonly RoutedEvent BackButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "BackButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(JoinRoom)
            );
        public event RoutedEventHandler BackButtonClicked
        {
            add { AddHandler(BackButtonClickedEvent, value); }
            remove { RemoveHandler(BackButtonClickedEvent, value); }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BackButtonClickedEvent));
        }

        private void RemovePlaceholderText1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == string.Empty)
            {
                PlaceholderTextBlock1.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowPlaceholderText1(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == string.Empty)
            {
                PlaceholderTextBlock1.Visibility = Visibility.Visible;
            }
        }
    }
}
