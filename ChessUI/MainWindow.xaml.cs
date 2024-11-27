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
using ChessLogic.GameStates.GameState;
using ChessUI.Menus;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameUserControl gameUserControl;
        MainMenu mainMenu = new MainMenu();
        SelectGameModeMenu selectGameModeMenu = new SelectGameModeMenu();
        InstructionsMenu instructionsMenu = new InstructionsMenu();
        SettingsMenu settingsMenu = new SettingsMenu();
        PauseMenu pauseMenu = new PauseMenu();
        ConfirmMenu confirmMenu = new ConfirmMenu();
        GameOverMenu gameOverMenu;
        bool onGame = false;

        public MainWindow()
        {
            InitializeComponent();
            
            CreateMainMenu();
        }

        private void CreateMainMenu()
        {
            onGame = false;

            mainMenu.PlayButtonClicked += MainMenu_PlayButtonClicked;
            mainMenu.InstructionsButtonClicked += MainMenu_InstructionsButtonClicked;
            mainMenu.SettingsButtonClicked += SettingsButtonClicked;

            view.Content = mainMenu;
        }

        private void CreateSelectGameModeMenu()
        {
            selectGameModeMenu.BackButtonClicked += BackButtonClicked;
            selectGameModeMenu.PlayWithBotButtonClicked += SelectGameMode_PlayWithBotButtonClicked;
            selectGameModeMenu.TwoPlayerButtonClicked += SelectGameMode_TwoPlayerButtonClicked;

            view.Content = selectGameModeMenu;
        }

        private void CreateInstructionMenu()
        {
            instructionsMenu.BackButtonClicked += BackButtonClicked;

            view.Content = instructionsMenu;
        }

        private void CreateSettingsMenu()
        {
            settingsMenu.BackButtonClicked += BackButtonClicked;

            view.Content = settingsMenu;
        }

        private void CreatePauseMenu()
        {
            pauseMenu.ContinueButtonClicked += PauseMenu_ContinueButtonClicked;
            pauseMenu.NewButtonClicked += PauseMenu_NewButtonClicked;
            pauseMenu.HomeButtonClicked += PauseMenu_HomeButtonClicked;
            pauseMenu.SettingsButtonClicked += SettingsButtonClicked;

            view.Content = pauseMenu;
        }

        private void CreateConfirmMenu()
        {
            confirmMenu.YesButtonClicked += ConfirmMenu_YesButtonClicked;
            confirmMenu.NoButtonClicked += BackButtonClicked;

            view.Content = confirmMenu;
        }

        private void CreateGameOverMenu()
        {
            gameOverMenu = new GameOverMenu(gameUserControl.gameState);
            view.Content = gameOverMenu;
        }

        private void CreateViewGameAI()
        {
            onGame = true;

            gameUserControl = new GameUserControl(true, 4);
            gameUserControl.PauseButtonClicked += PauseButtonClicked;

            view.Content = gameUserControl;
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!onGame)
                CreateMainMenu();
            else CreatePauseMenu();
        }

        private void MainMenu_PlayButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateSelectGameModeMenu();
        }

        private void MainMenu_InstructionsButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateInstructionMenu();
        }

        private void SettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateSettingsMenu();
        }

        private void SelectGameMode_PlayWithBotButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateViewGameAI();
        }

        private void SelectGameMode_TwoPlayerButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void PauseButtonClicked(object sender, RoutedEventArgs e)
        {
            CreatePauseMenu();

        }

        private void PauseMenu_ContinueButtonClicked(object sender, RoutedEventArgs e)
        {
            view.Content = gameUserControl;
        }

        private void PauseMenu_NewButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateSelectGameModeMenu();
        }

        private void PauseMenu_HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateConfirmMenu();
        }

        private void ConfirmMenu_YesButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateMainMenu();
        }

    }
}