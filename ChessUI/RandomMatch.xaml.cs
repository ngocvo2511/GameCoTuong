using ChessLogic;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for RandomMatch.xaml
    /// </summary>
    public partial class RandomMatch : UserControl
    {
        private HubConnection _connection;
        private DispatcherTimer _notificationTimer;
        public RandomMatch()
        {
            InitializeComponent();
            InitializeNotificationTimer();
            InitializeSignalR();
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

            _connection.Remove("RandomMatchFound");
            _connection.Remove("Error");

            _connection.On<string, string, string, Player, int>("RandomMatchFound", (roomName, username, opponentUsername, color, gameTime) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.NavigateToGameOnline(roomName, color, gameTime, username, opponentUsername);
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
            typeof(RandomMatch)
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

        private async void FindMatchButton_Click(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            var username = UsernameTextBox.Text;
            int time = (TimeComboBox.SelectedIndex + 1) * 300;



            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.SendAsync("JoinRandomMatch", username, time);
                searchingMatchMenu.Visibility = Visibility.Visible;
                searchingMatchMenu.BackButtonClicked += async (s, e) =>
                {
                    searchingMatchMenu.Visibility = Visibility.Collapsed;
                    await _connection.SendAsync("CancelFindMatch");
                };
            }
            else
            {
                ShowNotification("Không thể kết nối đến server, vui lòng kiểm tra kết nối mạng.");
            }
        }
    }
}
