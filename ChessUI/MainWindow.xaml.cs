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
        GameUserControl gameUserControl;
        public MainWindow()
        {
            InitializeComponent();

            CreateMainMenu();
        }

        private void CreateMainMenu()
        {
            MainMenu mainMenu = new MainMenu();

            mainMenu.PlayButtonClicked += MainMenu_PlayButtonClicked;
            mainMenu.InstructionsButtonClicked += MainMenu_InstructionsButtonClicked;
            mainMenu.SettingsButtonClicked += MainMenu_SettingsButtonClicked;

            view.Content = mainMenu;
        }

        private void CreateSelectGameModeMenu()
        {
            SelectGameModeMenu selectGameModeMenu = new SelectGameModeMenu();

            selectGameModeMenu.BackButtonClicked += SelectGameMode_BackButtonClicked;
            selectGameModeMenu.PlayWithBotButtonClicked += SelectGameMode_PlayWithBotButtonClicked;
            selectGameModeMenu.TwoPlayerButtonClicked += SelectGameMode_TwoPlayerButtonClicked;

            view.Content = selectGameModeMenu;
        }

        private void SelectGameMode_PlayWithBotButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateViewGameAI();
        }

        private void CreateViewGameAI()
        {
            gameUserControl = new GameUserControl();
            gameUserControl.PauseButtonClicked += PauseButtonClicked;
            view.Content = gameUserControl;
        }

        private void PauseButtonClicked(object sender, RoutedEventArgs e)
        {
            CreatePauseMenu();

        }

        private void CreatePauseMenu()
        {
            PauseMenu pauseMenu = new PauseMenu();
            pauseMenu.ContinueButtonClicked += PauseMenu_ContinueButtonClicked;
            pauseMenu.NewButtonClicked += PauseMenu_NewButtonClicked;
            pauseMenu.HomeButtonClicked += PauseMenu_HomeButtonClicked;
            view.Content = pauseMenu;
        }

        private void PauseMenu_HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateMainMenu();
        }

        private void PauseMenu_NewButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateSelectGameModeMenu();
        }


        private void PauseMenu_ContinueButtonClicked(object sender, RoutedEventArgs e)
        {
            view.Content = gameUserControl;
        }

        private void SelectGameMode_TwoPlayerButtonClicked(object sender, RoutedEventArgs e)
        {

        }


        private void MainMenu_PlayButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateSelectGameModeMenu();
        }

        private void SelectGameMode_BackButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateMainMenu();
        }



        private void MainMenu_InstructionsButtonClicked(object sender, RoutedEventArgs e)
        {
            InstructionsMenu instructionsMenu = new InstructionsMenu();
            instructionsMenu.Visibility = Visibility.Visible;
            view.Content = instructionsMenu;
        }

        private void MainMenu_SettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            SettingsMenu settingsMenu = new SettingsMenu();
            settingsMenu.Visibility = Visibility.Visible;
            view.Content = settingsMenu;
        }

    }
}