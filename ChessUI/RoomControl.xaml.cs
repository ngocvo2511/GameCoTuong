using ChessLogic;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ChessUI
{
    public partial class RoomControl : UserControl
    {
        private HubConnection _connection;
        private DispatcherTimer _notificationTimer;

        public RoomControl()
        {
            InitializeComponent();
            InitializeNotificationTimer();
            InitializeSignalR();
            ShowPlaceholderText(RoomNameTextBox, null);
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

            _connection.On<string>("RoomCreated", (roomName) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Room {roomName} created successfully.");
                    NavigateToGameOnlineE(roomName, Player.Red);
                });
            });

            _connection.On<string>("RoomJoined", (roomName) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Joined room {roomName} successfully.");
                    NavigateToGameOnlineE(roomName, Player.Black);
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
            if(string.IsNullOrEmpty(RoomNameTextBox.Text))
            {
                ShowNotification("Vui lòng nhập tên phòng");
                return;
            }
            else if(_connection.State != HubConnectionState.Connected)
            {
                ShowNotification("Không thể kết nối đến server, vui lòng kiểm tra kết nối mạng.");
                return;
            }
            string roomName = RoomNameTextBox.Text;
            await _connection.InvokeAsync("CreateRoom", roomName);
        }

        private async void JoinRoomButton_Click(object sender, RoutedEventArgs e)
        {
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

        private void NavigateToGameOnlineE(string RoomName, Player Color)
        {
            RaiseEvent(new NavigateToGameOnlineEventArgs(NavigateToGameOnlineEvent, RoomName, Color));
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
            typeof(RoomControl)
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
        
    }
}
