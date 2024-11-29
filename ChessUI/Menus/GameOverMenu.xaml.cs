using ChessLogic;
using ChessLogic.GameStates.GameState;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Schema;

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

        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.Red => "ĐỎ THẮNG!",
                Player.Black => "ĐEN THẮNG!",
                _ => "HOÀ"
            };
        }

        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.Red => "ĐỎ!",
                Player.Black => "ĐEN",
                _ => ""
            };
        }

        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            return reason switch
            {
                EndReason.Stalemate => $"{PlayerString(currentPlayer)} không thể di chuyển",
                EndReason.Checkmate => $"{PlayerString(currentPlayer)} bị chiếu bí",
                EndReason.InsufficientMaterial => "Hòa vì thiếu quân",
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
