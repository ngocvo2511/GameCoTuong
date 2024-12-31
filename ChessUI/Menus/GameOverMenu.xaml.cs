using ChessLogic;
using ChessLogic.GameStates.GameState;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI.Menus
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;
        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);
        }

        public GameOverMenu(Result result, Player current)
        {
            InitializeComponent();
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, current);
        }

        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.Red => "ĐỎ THẮNG!",
                Player.Black => "ĐEN THẮNG!",
                _ => "HÒA!"
            };
        }

        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.Red => "Đỏ",
                Player.Black => "Đen",
                _ => ""
            };
        }

        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            return reason switch
            {
                EndReason.Stalemate => $"{PlayerString(currentPlayer)} hết nước đi",
                EndReason.Checkmate => $"{PlayerString(currentPlayer)} bị chiếu bí",
                EndReason.InsufficientMaterial => "Hòa vì thiếu quân",
                EndReason.FiftyMoveRule => "Hòa vì 50 nước không ăn quân",
                EndReason.ThreefoldRepetition => "Hòa vì lặp lại nước đi 3 lần",
                EndReason.TimeForfeit => $"{PlayerString(currentPlayer)} hết thời gian",
                EndReason.PlayerDisconnected => $"{PlayerString(currentPlayer)} đã thoát",
                _ => ""
            };
        }

        public static readonly RoutedEvent NewButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "NewButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(GameOverMenu)
        );
        public event RoutedEventHandler NewButtonClicked
        {
            add { AddHandler(NewButtonClickedEvent, value); }
            remove { RemoveHandler(NewButtonClickedEvent, value); }
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NewButtonClickedEvent));
        }
        public static readonly RoutedEvent HomeButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "HomeButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(GameOverMenu)
        );

        public event RoutedEventHandler HomeButtonClicked
        {
            add { AddHandler(HomeButtonClickedEvent, value); }
            remove { RemoveHandler(HomeButtonClickedEvent, value); }
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(HomeButtonClickedEvent));
        }

        public static readonly RoutedEvent ReviewButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "ReviewButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(GameOverMenu)
        );
        public event RoutedEventHandler ReviewButtonClicked
        {
            add { AddHandler(ReviewButtonClickedEvent, value); }
            remove { RemoveHandler(ReviewButtonClickedEvent, value); }
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ReviewButtonClickedEvent));
        }

    }
}
