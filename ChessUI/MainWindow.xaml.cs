using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessLogic;
using ChessUI.Menus;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            selectGameModeMenu.BackButtonClicked += BackButtonClicked;
            instructionMenu.BackButtonClicked += BackButtonClicked;
            settingsMenu.BackButtonClicked += BackButtonClicked;
            gameDifficultyMenu.BackButtonClicked += BackButtonClicked;
            selectGameModeMenu.PlayWithBotButtonClicked += PlayWithBotButtonClicked;
        }
        private void BackButtonClicked(object sender, EventArgs e)
        {
            mainMenu.Visibility = Visibility.Visible;
        }
        private void PlayWithBotButtonClicked(object sender, EventArgs e)
        {
            gameDifficultyMenu.Visibility = Visibility.Visible;
        }
    }
}