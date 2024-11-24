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

namespace ChessUI.Menus
{
    /// <summary>
    /// Interaction logic for SelectGameModeMenu.xaml
    /// </summary>
    public partial class SelectGameModeMenu : UserControl
    {
        public SelectGameModeMenu()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent BackButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "BackButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SelectGameModeMenu)
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

        public static readonly RoutedEvent PlayWithBotButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "PlayWithBotButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SelectGameModeMenu)
        );

        public event RoutedEventHandler PlayWithBotButtonClicked
        {
            add { AddHandler(PlayWithBotButtonClickedEvent, value); }
            remove { RemoveHandler(PlayWithBotButtonClickedEvent, value); }
        }

        private void PlayWithBotButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PlayWithBotButtonClickedEvent));
        }

        public static readonly RoutedEvent TwoPlayerButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "TwoPlayerButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SelectGameModeMenu)
        );

        public event RoutedEventHandler TwoPlayerButtonClicked
        {
            add { AddHandler(TwoPlayerButtonClickedEvent, value); }
            remove { RemoveHandler(TwoPlayerButtonClickedEvent, value); }
        }

        private void TwoPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(TwoPlayerButtonClickedEvent));
        }

    }
}
