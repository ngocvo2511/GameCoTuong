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
    /// Interaction logic for GameDifficultyMenu.xaml
    /// </summary>
    public partial class GameDifficultyMenu : UserControl
    {
        public event EventHandler BackButtonClicked;
        public GameDifficultyMenu()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            BackButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            //var window = Application.Current.MainWindow;
            //GameWindow gameWindow = new GameWindow
            //{
            //    Left = window.Left,
            //    Top = window.Top,
            //};
            //Application.Current.MainWindow = gameWindow;
            //gameWindow.Show();
            //window.Close();
        }

        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            //var window = Application.Current.MainWindow;
            //GameWindow gameWindow = new GameWindow
            //{
            //    Left = window.Left,
            //    Top = window.Top,
            //};
            //Application.Current.MainWindow = gameWindow;
            //gameWindow.Show();
            //window.Close();
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            //var window = Application.Current.MainWindow;
            //GameWindow gameWindow = new GameWindow
            //{
            //    Left = window.Left,
            //    Top = window.Top,
            //};
            //Application.Current.MainWindow = gameWindow;
            //gameWindow.Show();
            //window.Close();
        }
    }
}
