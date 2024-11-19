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
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public GameOverMenu()
        {
            InitializeComponent();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = (GameWindow)Application.Current.MainWindow;
            gameWindow.selectGameModeMenu.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = (GameWindow)Application.Current.MainWindow;
            MainWindow mainWindow = new MainWindow
            {
                Left = gameWindow.Left,
                Top = gameWindow.Top,
            };
            Application.Current.MainWindow = mainWindow;
            mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            mainWindow.Show();
            gameWindow.Close();
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
