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
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        public PauseMenu()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent ContinueButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "ContinueButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PauseMenu)
        );

        public event RoutedEventHandler ContinueButtonClicked
        {
            add { AddHandler(ContinueButtonClickedEvent, value); }
            remove { RemoveHandler(ContinueButtonClickedEvent, value); }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ContinueButtonClickedEvent));
        }

        public static readonly RoutedEvent NewButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "NewButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PauseMenu)
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
            typeof(PauseMenu)
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

        public static readonly RoutedEvent SettingsButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "SettingsButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PauseMenu)
        );

        public event RoutedEventHandler SettingsButtonClicked
        {
            add { AddHandler(SettingsButtonClickedEvent, value); }
            remove { RemoveHandler(SettingsButtonClickedEvent, value); }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SettingsButtonClickedEvent));
        }
    }
}
