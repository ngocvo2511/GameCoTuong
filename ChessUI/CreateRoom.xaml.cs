using ChessLogic;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for CreateRoom.xaml
    /// </summary>
    public partial class CreateRoom : UserControl
    {
        private HubConnection _connection;
        private DispatcherTimer _notificationTimer;

        public CreateRoom()
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

            _connection.On<string, string, int>("RoomCreated", (roomName, username, time) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Room {roomName} created successfully.");
                    NavigateToGameOnlineE(roomName, username, Player.Red, time);
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
            string username;
            int time;
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                username = "Người chơi 1";
            }
            else
            {
                username = UsernameTextBox.Text;
            }
            time = int.Parse(TimeLimitTextBox.Text) * 60;
            string roomName = RoomNameTextBox.Text;
            await _connection.InvokeAsync("CreateRoom", roomName, username, time);
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
            typeof(CreateRoom)
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

        public static readonly RoutedEvent BackButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "BackButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(CreateRoom)
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

        public static readonly RoutedEvent TimeLimitTextBoxChangedEvent = EventManager.RegisterRoutedEvent(
            "TimeLimitTextBoxChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(CreateRoom)
            );

        public event RoutedEventHandler TimeLimitTextBoxChanged
        {
            add { AddHandler(TimeLimitTextBoxChangedEvent, value); }
            remove { RemoveHandler(TimeLimitTextBoxChangedEvent, value); }
        }

        private void TimeLimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(TimeLimitTextBoxChangedEvent));
        }

        private void TimeLimitTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TimeLimitTextBox.Text)) TimeLimitTextBox.Text = "10";
        }
    }
}
