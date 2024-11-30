using ChessLogic.GameStates.GameState;
using ChessUI.Menus;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
        GameDifficultyMenu gameDifficultyMenu = new GameDifficultyMenu();
        InstructionsMenu instructionsMenu = new InstructionsMenu();
        SettingsMenu settingsMenu = new SettingsMenu();
        PauseMenu pauseMenu = new PauseMenu();
        ConfirmMenu confirmMenu = new ConfirmMenu();
        GameOverMenu gameOverMenu;

        MediaPlayer buttonClickSound = new MediaPlayer();
        MediaPlayer gameOverSound = new MediaPlayer();
        MediaPlayer moveSound = new MediaPlayer();

        bool onGame = false;

        public MainWindow()
        {
            InitializeComponent();

            CreateMainMenu();

            buttonClickSound.Open(new Uri("Assets/Sounds/buttonClickSound.mp3", UriKind.Relative));
            gameOverSound.Open(new Uri("Assets/Sounds/gameOverSound.mp3", UriKind.Relative));
            moveSound.Open(new Uri("Assets/Sounds/moveSound.mp3", UriKind.Relative));
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
        private void CreateSelectDifficultMenu()
        {
            gameDifficultyMenu.BackButtonClicked += GameDifficultyMenu_BackButtonClicked;
            gameDifficultyMenu.PlayEasyBotButtonClicked += GameDifficultyMenu_PlayEasyBotButtonClicked;
            gameDifficultyMenu.PlayNormalBotButtonClicked += GameDifficultyMenu_PlayNormalBotButtonClicked;
            gameDifficultyMenu.PlayHardBotButtonClicked += GameDifficultyMenu_PlayHardBotButtonClicked;
            view.Content = gameDifficultyMenu;
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
            pauseMenu.NewButtonClicked += NewButtonClicked;
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

        internal void CreateGameOverMenu(GameState gameState)
        {
            PlayGameOverSound();
            gameOverMenu = new GameOverMenu(gameState);

            gameOverMenu.NewButtonClicked += NewButtonClicked;
            gameOverMenu.HomeButtonClicked += GameOverMenu_HomeButtonClicked;
            gameOverMenu.ReviewButtonClicked += GameOverMenu_ReviewButtonClicked;

            view.Content = gameOverMenu;
        }

        private void CreateViewGameAI(int difficulty)
        {
            onGame = true;

            gameUserControl = new GameUserControl(this, true, difficulty);
            gameUserControl.PauseButtonClicked += PauseButtonClicked;

            view.Content = gameUserControl;
        }

        private void CreateViewGame2P()
        {
            onGame = true;

            gameUserControl = new GameUserControl(this, false);
            gameUserControl.PauseButtonClicked += PauseButtonClicked;

            view.Content = gameUserControl;
        }

        private void CreateViewGameOnline()
        {
            onGame = true;
            GameOnline gameOnline = new GameOnline();
            view.Content = gameOnline;
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            if (!onGame)
                view.Content = mainMenu;
            else view.Content = pauseMenu;
        }

        private void MainMenu_PlayButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateSelectGameModeMenu();
        }

        private void MainMenu_InstructionsButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateInstructionMenu();
        }

        private void SettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateSettingsMenu();
        }

        private void SelectGameMode_PlayWithBotButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateSelectDifficultMenu();
        }
        private void GameDifficultyMenu_BackButtonClicked(object sender, RoutedEventArgs e)
        {
            view.Content = selectGameModeMenu;
        }
        private void GameDifficultyMenu_PlayEasyBotButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateViewGameAI(2);
        }
        private void GameDifficultyMenu_PlayNormalBotButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateViewGameAI(3);
        }
        private void GameDifficultyMenu_PlayHardBotButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateViewGameAI(4);
        }

        private void SelectGameMode_TwoPlayerButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateViewGame2P();
        }

        private void PauseButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreatePauseMenu();

        }

        private void PauseMenu_ContinueButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            view.Content = gameUserControl;
        }

        private void NewButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateSelectGameModeMenu();
        }

        private void PauseMenu_HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateConfirmMenu();
        }

        private void ConfirmMenu_YesButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateMainMenu();
        }

        private void GameOverMenu_HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            CreateMainMenu();
        }
        private void GameOverMenu_ReviewButtonClicked(object sender, RoutedEventArgs e)
        {
            PlayButtonClickSound();
            view.Content = gameUserControl;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {

                if (view.Content == pauseMenu)
                {
                    view.Content = gameUserControl;
                }
                else if (onGame)
                {
                    CreatePauseMenu();
                }

            }
        }
        internal void PlayButtonClickSound()
        {
            buttonClickSound.Position = TimeSpan.Zero;
            buttonClickSound.Play();
        }
        internal void PlayGameOverSound()
        {
            gameOverSound.Position = TimeSpan.Zero;
            gameOverSound.Play();
        }
        internal void PlayMoveSound()
        {
            moveSound.Position = TimeSpan.Zero;
            moveSound.Play();
        }
    }
}