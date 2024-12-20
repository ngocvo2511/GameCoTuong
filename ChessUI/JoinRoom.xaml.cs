using ChessLogic;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                Interval = TimeSpan.FromSeconds(2.5) // Thời gian thông báo hiển thị
            };
            _notificationTimer.Tick += (s, e) =>
            {
                HideNotification();
                _notificationTimer.Stop();
            };
        }

        private void ShowNotification(string message)
        {
            // Cập nhật nội dung thông báo (nếu cần)
            if (NotificationPanel.Child is TextBlock textBlock)
            {
                textBlock.Text = message;
            }

            // Hiển thị thông báo
            NotificationPanel.Visibility = Visibility.Visible;

            // Hiệu ứng mờ dần khi xuất hiện
            var fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            NotificationPanel.BeginAnimation(OpacityProperty, fadeInAnimation);

            // Bắt đầu đếm thời gian tự động tắt
            _notificationTimer.Start();
        }

        private void HideNotification()
        {
            // Hiệu ứng mờ dần khi ẩn
            var fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += (s, e) => NotificationPanel.Visibility = Visibility.Collapsed;
            NotificationPanel.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }



        private async void InitializeSignalR()
        {
            var connectionManager = SignalRConnectionManager.Instance;
            _connection = connectionManager.Connection;
          
            _connection.On<string, string, int>("RoomJoined", (roomName, username, time) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Joined room {roomName} successfully.");
                    NavigateToGameOnlineE(roomName, username, Player.Black, time);
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

        private void NavigateToGameOnlineE(string RoomName, string Username, Player Color, int Time)
        {
            RaiseEvent(new NavigateToGameOnlineEventArgs(NavigateToGameOnlineEvent, RoomName, Username, Color, Time));
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
