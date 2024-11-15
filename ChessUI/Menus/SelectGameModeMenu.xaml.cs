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
        public event EventHandler BackButtonClicked;
        public SelectGameModeMenu()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            BackButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void PlayWithBotButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.MainWindow;
            GameWindow gameWindow = new GameWindow
            {
                Left = window.Left,
                Top = window.Top,
            };
            Application.Current.MainWindow = gameWindow;
            gameWindow.Show();
            window.Close();
        }

        private void TwoPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.MainWindow;
            GameWindow gameWindow = new GameWindow
            {
                Left = window.Left,
                Top = window.Top,
            };
            Application.Current.MainWindow = gameWindow;
            gameWindow.Show();
            window.Close();
        }

    }
}
