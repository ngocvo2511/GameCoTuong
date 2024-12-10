using ChessLogic;
using ChessLogic.GameStates.GameState;
using ChessUI.Menus;
using Microsoft.AspNetCore.SignalR.Client;
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
        SaveSlotControl saveloadSlotControl;
        GameOverMenu gameOverMenu;

       

        bool onGame = false;
        Player color = Player.Red;
        int time = 600;

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
            mainMenu.LoadButtonClicked += LoadButton_Clicked;
            view.Content = mainMenu;
        }

        private void CreateSelectGameModeMenu()
        {
            selectGameModeMenu.BackButtonClicked += BackButtonClicked;
            selectGameModeMenu.PlayWithBotButtonClicked += SelectGameMode_PlayWithBotButtonClicked;
            selectGameModeMenu.TwoPlayerButtonClicked += SelectGameMode_TwoPlayerButtonClicked;
            selectGameModeMenu.PlayOnlineButtonClicked += SelectGameMode_PlayOnlineButtonClicked;

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
            settingsMenu.humanFirst.IsEnabled = !onGame;
            settingsMenu.botFirst.IsEnabled = !onGame;
            settingsMenu.isTimeLimit.IsEnabled = !onGame;
            settingsMenu.TimeLimitTextBox.IsEnabled = !onGame;

            settingsMenu.BackButtonClicked += BackButtonClicked;
            settingsMenu.humanFirstChecked += SettingsMenu_humanFirstChecked;
            settingsMenu.botFirstChecked += SettingsMenu_botFirstChecked;
            settingsMenu.VolumeSliderValueChanged += SettingsMenu_VolumeSliderValueChanged;
            settingsMenu.isTimeLimitChecked += SettingsMenu_isTimeLimitChecked;
            settingsMenu.isTimeLimitUnchecked += SettingsMenu_isTimeLimitUnchecked;
            settingsMenu.TimeLimitTextBoxChanged += SettingsMenu_TimeLimitTextBoxChanged;

            view.Content = settingsMenu;
        }

        private void CreatePauseMenu()
        {
            if (time != 0) gameUserControl.StopTimer();

            pauseMenu.ContinueButtonClicked += ContinueButtonClicked;
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

        

        private void CreateViewGameAI(int difficulty)
        {
            onGame = true;
            if (gameUserControl != null) gameUserControl.ResetTimer();
            gameUserControl = new GameUserControl(color, time, true, difficulty);
            gameUserControl.PauseButtonClicked += PauseButtonClicked;
            gameUserControl.SaveButtonClicked += SaveButtonClicked;
            gameUserControl.GameOver += OnGameOver;
            view.Content = gameUserControl;
        }

        private void CreateViewGame2P()
        {
            onGame = true;
            if(gameUserControl!=null) gameUserControl.ResetTimer();
            gameUserControl = new GameUserControl(color, time, false);
            gameUserControl.PauseButtonClicked += PauseButtonClicked;
            gameUserControl.SaveButtonClicked += SaveButtonClicked;
            gameUserControl.GameOver += OnGameOver;
            view.Content = gameUserControl;
        }
        private void CreateSaveMenu()
        {
            saveloadSlotControl = new SaveSlotControl(gameUserControl.gameState);
            saveloadSlotControl.BackButtonClicked += ContinueButtonClicked;
            view.Content = saveloadSlotControl;
        }
        private void CreateLoadMenu()
        {
            saveloadSlotControl=new SaveSlotControl();
            saveloadSlotControl.BackButtonClicked += BackButtonClicked;
            saveloadSlotControl.SelectedLoadSlot += SelectedLoadSlot_Clicked;
            view.Content = saveloadSlotControl;
        }
        private void CreateRoomControl()
        {
            RoomControl roomControl = new RoomControl();
            roomControl.NavigateToGameOnline += RoomControl_NavigateToGameOnline;
            roomControl.BackButtonClicked += RoomControl_BackButtonClicked;
            view.Content = roomControl;
        }

        private void RoomControl_NavigateToGameOnline(object sender, RoutedEventArgs e)
        {
            if (e is NavigateToGameOnlineEventArgs args)
            {

                string roomName = args.RoomName;
                Player color = args.Color;
                if (roomName == null)
                {
                    return;
                }
                onGame = true;
                GameOnline gameOnline = new GameOnline(roomName, this, color);
                view.Content = gameOnline;
            }
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            if (!onGame)
                view.Content = mainMenu;
            else view.Content = pauseMenu;
        }

        private void MainMenu_PlayButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateSelectGameModeMenu();
        }

        private void MainMenu_InstructionsButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateInstructionMenu();
        }
        private void LoadButton_Clicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateLoadMenu();
        }

        private void SettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateSettingsMenu();
        }
        private void SelectedLoadSlot_Clicked(object sender, SaveSlotEventArgs e)
        {

        }
        private void SelectGameMode_PlayWithBotButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
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
            Sound.PlayButtonClickSound();
            CreateViewGame2P();
        }

        private void SelectGameMode_PlayOnlineButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateRoomControl();
        }

        private void RoomControl_BackButtonClicked(object sender, RoutedEventArgs e)
        {
            view.Content = selectGameModeMenu;
        }

        private void SettingsMenu_humanFirstChecked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            if (color == Player.Black)
            {
                color = Player.Red;
            }
        }

        private void SettingsMenu_botFirstChecked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            if (color == Player.Red)
            {
                color = Player.Black;
            }
        }

        private void SettingsMenu_VolumeSliderValueChanged(object sender, RoutedEventArgs e)
        {
            Sound.SetVolume((int)settingsMenu.VolumeSlider.Value);
        }

        private void SettingsMenu_isTimeLimitChecked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            settingsMenu.TimeLimitTextBox.IsEnabled = true;
            this.time = Convert.ToInt16(settingsMenu.TimeLimitTextBox.Text);
        }

        private void SettingsMenu_isTimeLimitUnchecked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            settingsMenu.TimeLimitTextBox.IsEnabled = false;
            this.time = 0;
        }
        private void SettingsMenu_TimeLimitTextBoxChanged(object sender, RoutedEventArgs e)
        {
            this.time = Convert.ToInt16(settingsMenu.TimeLimitTextBox.Text) * 60;
        }
        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            if(time!=0) gameUserControl.StopTimer();
            Sound.PlayButtonClickSound();
            CreateSaveMenu();
        }
        private void PauseButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreatePauseMenu();
        }

        private void ContinueButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            if (time != 0) gameUserControl.ContinueTimer();
            view.Content = gameUserControl;
        }

        private void NewButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateSelectGameModeMenu();
        }

        private void PauseMenu_HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateConfirmMenu();
        }

        private void ConfirmMenu_YesButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateMainMenu();
        }

        private void GameOverMenu_HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateMainMenu();
        }
        private void GameOverMenu_ReviewButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
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

        private void OnGameOver(object sender, RoutedEventArgs e)
        {
            if (e is RoutedPropertyChangedEventArgs<GameState> gameOverEventArgs)
            {
                GameState gameState = gameOverEventArgs.NewValue;
                
                    Sound.PlayGameOverSound();
                    gameOverMenu = new GameOverMenu(gameState);

                    gameOverMenu.NewButtonClicked += NewButtonClicked;
                    gameOverMenu.HomeButtonClicked += GameOverMenu_HomeButtonClicked;
                    gameOverMenu.ReviewButtonClicked += GameOverMenu_ReviewButtonClicked;

                    view.Content = gameOverMenu;
               
            }
        }

    }
}