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

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentArea.Content = new MainMenu();
        }

        private void PlayWithBotButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            Game1pWindow game1pWindow = new Game1pWindow
            {
                Left = mainWindow.Left,
                Top = mainWindow.Top,
            };
            game1pWindow.Show();
            mainWindow.Close();
        }

        private void TwoPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            Game2pWindow game2pWindow = new Game2pWindow
            {
                Left = mainWindow.Left,
                Top = mainWindow.Top,
            };
            game2pWindow.Show();
            mainWindow.Close();
        }

    }
}
