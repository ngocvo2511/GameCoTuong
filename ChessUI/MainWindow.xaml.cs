using ChessLogic;
using ChessLogic.GameStates;
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
        private SettingsModel settingsModel = new SettingsModel();

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

        private void CleanupCurrentView()
        {
            if (mainWindowGrid.Children.Count > 0)
            {
                // Giả định rằng phần tử cuối cùng là view hiện tại
                var currentView = mainWindowGrid.Children[mainWindowGrid.Children.Count - 1];

                if (currentView is GameUserControl gameControl)
                {
                    gameControl.PauseButtonClicked -= PauseButtonClicked;
                    gameControl.SaveButtonClicked -= SaveButtonClicked;
                    gameControl.GameOver -= OnGameOver;
                    gameControl.StopTimer();
                }
                else if (currentView is PauseMenu pauseMenu)
                {
                    pauseMenu.ContinueButtonClicked -= ContinueButtonClicked;
                    pauseMenu.NewButtonClicked -= NewButtonClicked;
                    pauseMenu.HomeButtonClicked -= PauseMenu_HomeButtonClicked;
                    pauseMenu.SettingsButtonClicked -= SettingsButtonClicked;
                }
                else if (currentView is GameOverMenu gameOverMenu)
                {
                    gameOverMenu.NewButtonClicked -= NewButtonClicked;
                    gameOverMenu.HomeButtonClicked -= GameOverMenu_HomeButtonClicked;
                    gameOverMenu.ReviewButtonClicked -= GameOverMenu_ReviewButtonClicked;
                }
                else if (currentView is SettingsMenu settingsMenu)
                {
                    settingsMenu.CloseButtonClicked -= CloseButtonClicked;
                    settingsMenu.humanFirstChecked -= SettingsMenu_humanFirstChecked;
                    settingsMenu.botFirstChecked -= SettingsMenu_botFirstChecked;
                    settingsMenu.isTimeLimitChecked -= SettingsMenu_isTimeLimitChecked;
                    settingsMenu.isTimeLimitUnchecked -= SettingsMenu_isTimeLimitUnchecked;
                    settingsMenu.TimeLimitTextBoxChanged -= SettingsMenu_TimeLimitTextBoxChanged;
                    settingsMenu.SettingsChanged -= SettingsMenu_SettingsChanged;
                }
                else if (currentView is SelectGameModeMenu selectGameModeMenu)
                {
                    selectGameModeMenu.CloseButtonClicked -= CloseButtonClicked;
                    selectGameModeMenu.PlayWithBotButtonClicked -= SelectGameMode_PlayWithBotButtonClicked;
                    selectGameModeMenu.TwoPlayerButtonClicked -= SelectGameMode_TwoPlayerButtonClicked;
                    selectGameModeMenu.PlayOnlineButtonClicked -= SelectGameMode_PlayOnlineButtonClicked;
                }
                else if (currentView is RoomControl roomControl)
                {
                    roomControl.CreateRoomButtonClicked -= RoomControl_CreateRoomButtonClicked;
                    roomControl.JoinRoomButtonClicked -= RoomControl_JoinRoomButtonClicked;
                    roomControl.RandomMatchButtonClicked -= RoomControl_RandomMatchButtonClicked;
                    roomControl.BackButtonClicked -= CloseButtonClicked;
                }
                else if (currentView is JoinRoom joinRoom)
                {
                    joinRoom.BackButtonClicked -= CloseButtonClicked;
                    joinRoom.NavigateToGameOnline -= RoomControl_NavigateToGameOnline;
                }
                else if (currentView is CreateRoom createRoom)
                {
                    createRoom.BackButtonClicked -= CloseButtonClicked;
                    createRoom.NavigateToGameOnline -= RoomControl_NavigateToGameOnline;
                }
                else if (currentView is SaveSlotControl saveSlotControl)
                {
                    saveSlotControl.CloseButtonClicked -= ContinueButtonClicked;
                    saveSlotControl.SelectedLoadSlot -= SelectedLoadSlot_Clicked;
                }
                else if (currentView is HistoryMenu historyMenu)
                {
                    historyMenu.CloseButtonClicked -= CloseButtonClicked;
                }
                else if(currentView is GameOnline gameOnline)
                {
                    gameOnline.SettingButtonClicked -= SettingsButtonClicked;
                    gameOnline.LeaveRoomButtonClicked -= GameOnline_LeaveRoomButtonClicked;
                }

            }
        }



        private void CreateMainMenu()
        {
            CleanupCurrentView();
            onGame = false;


            MainMenu mainMenu = new MainMenu();

            mainMenu.PlayButtonClicked += MainMenu_PlayButtonClicked;
            mainMenu.InstructionsButtonClicked += MainMenu_InstructionsButtonClicked;
            mainMenu.SettingsButtonClicked += SettingsButtonClicked;
            mainMenu.HistoryButtonClicked += MainMenu_HistoryButtonClicked;
            mainMenu.LoadButtonClicked += LoadButton_Clicked;


            mainWindowGrid.Children.Clear();
            mainWindowGrid.Children.Add(mainMenu);
        }

        private void CreateSelectGameModeMenu()
        {
            CleanupCurrentView();
            SelectGameModeMenu selectGameModeMenu = new SelectGameModeMenu();

            selectGameModeMenu.CloseButtonClicked += CloseButtonClicked;
            selectGameModeMenu.PlayWithBotButtonClicked += SelectGameMode_PlayWithBotButtonClicked;
            selectGameModeMenu.TwoPlayerButtonClicked += SelectGameMode_TwoPlayerButtonClicked;
            selectGameModeMenu.PlayOnlineButtonClicked += SelectGameMode_PlayOnlineButtonClicked;


            mainWindowGrid.Children.Add(selectGameModeMenu);
        }
        private void CreateSelectDifficultMenu()
        {
            GameDifficultyMenu gameDifficultyMenu = new GameDifficultyMenu();

            gameDifficultyMenu.BackButtonClicked += GameDifficultyMenu_BackButtonClicked;
            gameDifficultyMenu.PlayEasyBotButtonClicked += GameDifficultyMenu_PlayEasyBotButtonClicked;
            gameDifficultyMenu.PlayNormalBotButtonClicked += GameDifficultyMenu_PlayNormalBotButtonClicked;
            gameDifficultyMenu.PlayHardBotButtonClicked += GameDifficultyMenu_PlayHardBotButtonClicked;


            mainWindowGrid.Children.Add(gameDifficultyMenu);
        }
        private void CreateInstructionMenu()
        {
            CleanupCurrentView();
            InstructionsMenu instructionsMenu = new InstructionsMenu();

            instructionsMenu.CloseButtonClicked += CloseButtonClicked;


            mainWindowGrid.Children.Add(instructionsMenu);
        }

        private void CreateSettingsMenu()
        {
            CleanupCurrentView();

            SettingsMenu settingsMenu = new SettingsMenu(settingsModel);
            mainWindowGrid.Children.Add(settingsMenu);

            settingsMenu.humanFirst.IsEnabled = !onGame;
            settingsMenu.botFirst.IsEnabled = !onGame;
            settingsMenu.isTimeLimit.IsEnabled = !onGame;
            settingsMenu.TimeLimitTextBox.IsEnabled = !onGame;

            settingsMenu.CloseButtonClicked += CloseButtonClicked;
            settingsMenu.humanFirstChecked += SettingsMenu_humanFirstChecked;
            settingsMenu.botFirstChecked += SettingsMenu_botFirstChecked;
            settingsMenu.VolumeSliderValueChanged += SettingsMenu_VolumeSliderValueChanged;
            settingsMenu.isTimeLimitChecked += SettingsMenu_isTimeLimitChecked;
            settingsMenu.isTimeLimitUnchecked += SettingsMenu_isTimeLimitUnchecked;
            settingsMenu.TimeLimitTextBoxChanged += SettingsMenu_TimeLimitTextBoxChanged;
            settingsMenu.SettingsChanged += SettingsMenu_SettingsChanged;




        }

        private void CreateHistoryMenu()
        {
            CleanupCurrentView();
            HistoryMenu historyMenu = new HistoryMenu();

            historyMenu.CloseButtonClicked += CloseButtonClicked;


            historyMenu.LoadHistory();
            mainWindowGrid.Children.Add(historyMenu);
        }

        private void CreatePauseMenu()
        {

            if (time != 0) gameUserControl.StopTimer();

            PauseMenu pauseMenu = new PauseMenu();

            pauseMenu.ContinueButtonClicked += ContinueButtonClicked;
            pauseMenu.NewButtonClicked += NewButtonClicked;
            pauseMenu.HomeButtonClicked += PauseMenu_HomeButtonClicked;
            pauseMenu.SettingsButtonClicked += SettingsButtonClicked;


            mainWindowGrid.Children.Add(pauseMenu);
        }

        private void CreateConfirmMenu()
        {

            ConfirmMenu confirmMenu = new ConfirmMenu();

            confirmMenu.YesButtonClicked += ConfirmMenu_YesButtonClicked;
            confirmMenu.NoButtonClicked += CloseButtonClicked;


            mainWindowGrid.Children.Add(confirmMenu);
        }



        private void CreateViewGameAI(int difficulty)
        {

            onGame = true;
            if (gameUserControl != null) gameUserControl.ResetTimer();
            color = settingsModel.HumanFirst ? Player.Red : Player.Black;
            gameUserControl = new GameUserControl(color, time, true, difficulty);
            gameUserControl.PauseButtonClicked += PauseButtonClicked;
            gameUserControl.SaveButtonClicked += SaveButtonClicked;
            gameUserControl.GameOver += OnGameOver;

            mainWindowGrid.Children.Clear();
            mainWindowGrid.Children.Add(gameUserControl);
        }

        private void CreateViewGame2P()
        {
            onGame = true;
            if (gameUserControl != null) gameUserControl.ResetTimer();
            color = settingsModel.HumanFirst ? Player.Red : Player.Black;
            gameUserControl = new GameUserControl(color, settingsModel.TimeLimit * 60, false);
            gameUserControl.PauseButtonClicked += PauseButtonClicked;
            gameUserControl.SaveButtonClicked += SaveButtonClicked;
            gameUserControl.GameOver += OnGameOver;

            mainWindowGrid.Children.Clear();
            mainWindowGrid.Children.Add(gameUserControl);
        }
        private void CreateViewGameLoad(GameStateForLoad gameStateForLoad)
        {
            onGame = true;
            if (gameUserControl != null) gameUserControl.ResetTimer();
            gameUserControl = new GameUserControl(gameStateForLoad);
            gameUserControl.PauseButtonClicked += PauseButtonClicked;
            gameUserControl.SaveButtonClicked += SaveButtonClicked;
            gameUserControl.GameOver += OnGameOver;

            mainWindowGrid.Children.Clear();
            mainWindowGrid.Children.Add(gameUserControl);
        }
        private void CreateSaveMenu()
        {
            saveloadSlotControl = new SaveSlotControl(gameUserControl.gameState);
            saveloadSlotControl.CloseButtonClicked += ContinueButtonClicked;

            mainWindowGrid.Children.Add(saveloadSlotControl);
        }
        private void CreateLoadMenu()
        {
            saveloadSlotControl = new SaveSlotControl();
            saveloadSlotControl.CloseButtonClicked += CloseButtonClicked;
            saveloadSlotControl.SelectedLoadSlot += SelectedLoadSlot_Clicked;

            mainWindowGrid.Children.Add(saveloadSlotControl);
        }
        private void CreateRoomControl()
        {
            RoomControl roomControl = new RoomControl();
            roomControl.CreateRoomButtonClicked += RoomControl_CreateRoomButtonClicked;
            roomControl.JoinRoomButtonClicked += RoomControl_JoinRoomButtonClicked;
            roomControl.RandomMatchButtonClicked += RoomControl_RandomMatchButtonClicked;
            roomControl.BackButtonClicked += CloseButtonClicked;



            mainWindowGrid.Children.Add(roomControl);
        }

        private void RoomControl_RandomMatchButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        private void RoomControl_JoinRoomButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();

            JoinRoom joinRoom = new JoinRoom();
            joinRoom.BackButtonClicked += CloseButtonClicked;
            joinRoom.NavigateToGameOnline += RoomControl_NavigateToGameOnline;

            mainWindowGrid.Children.Add(joinRoom);
        }

        private void RoomControl_CreateRoomButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateRoom createRoom = new CreateRoom();
            createRoom.BackButtonClicked += CloseButtonClicked;
            createRoom.NavigateToGameOnline += RoomControl_NavigateToGameOnline;

            mainWindowGrid.Children.Add(createRoom);

        }

        private void RoomControl_NavigateToGameOnline(object sender, RoutedEventArgs e)
        {
            if (e is NavigateToGameOnlineEventArgs args)
            {
                CleanupCurrentView();
                string roomName = args.RoomName;
                string username = args.Username;
                string opponentUsername = args.OpponentUsername;
                Player color = args.Color;
                int time = args.Time;
                if (roomName == null)
                {
                    return;
                }
                onGame = true;
                GameOnline gameOnline = new GameOnline(roomName, color, time, username, opponentUsername);
                gameOnline.SettingButtonClicked += SettingsButtonClicked;
                gameOnline.LeaveRoomButtonClicked += GameOnline_LeaveRoomButtonClicked;

                mainWindowGrid.Children.Clear();
                mainWindowGrid.Children.Add(gameOnline);
            }
        }

        private void GameOnline_LeaveRoomButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateMainMenu();
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CloseAMenu();
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
        private void MainMenu_HistoryButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateHistoryMenu();
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
            GameStateForLoad gameStateForLoad = SaveService.Load(e.FilePath);
            CreateViewGameLoad(gameStateForLoad);
        }
        private void SelectGameMode_PlayWithBotButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            CreateSelectDifficultMenu();
        }
        private void GameDifficultyMenu_BackButtonClicked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            mainWindowGrid.Children.RemoveAt(mainWindowGrid.Children.Count - 1);
        }
        private void GameDifficultyMenu_PlayEasyBotButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateViewGameAI(3);
        }
        private void GameDifficultyMenu_PlayNormalBotButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateViewGameAI(4);
        }
        private void GameDifficultyMenu_PlayHardBotButtonClicked(object sender, RoutedEventArgs e)
        {
            CreateViewGameAI(5);
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

        private void SettingsMenu_humanFirstChecked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            settingsModel.HumanFirst = true;
            if (color == Player.Black)
            {
                color = Player.Red;
            }
        }

        private void SettingsMenu_botFirstChecked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            settingsModel.HumanFirst = false;
            if (color == Player.Red)
            {
                color = Player.Black;
            }
        }

        private void SettingsMenu_SettingsChanged(SettingsModel updatedSettings)
        {
            Sound.SetVolume((int)updatedSettings.Volume);
            settingsModel = updatedSettings;

            // Nếu cần, bạn cũng có thể lưu trạng thái hoặc cập nhật UI khác
        }
        private void SettingsMenu_VolumeSliderValueChanged(object sender, RoutedEventArgs e)
        {
            //settingsModel.Volume =
            //Sound.SetVolume((int)settingsMenu.VolumeSlider.Value);

        }

        private void SettingsMenu_isTimeLimitChecked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            //this.time = Convert.ToInt16(settingsMenu.TimeLimitTextBox.Text);
        }

        private void SettingsMenu_isTimeLimitUnchecked(object sender, RoutedEventArgs e)
        {
            Sound.PlayButtonClickSound();
            this.time = 0;
        }
        private void SettingsMenu_TimeLimitTextBoxChanged(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(settingsMenu.TimeLimitTextBox.Text)) this.time = 0;
            //else this.time = Convert.ToInt16(settingsMenu.TimeLimitTextBox.Text) * 60;
        }
        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            if (time != 0) gameUserControl.StopTimer();
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
            CloseAMenu();
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
            CloseAMenu();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (onGame && mainWindowGrid.Children.Count != 1)
                {
                    CloseAMenu();
                }
                else if (onGame)
                {
                    CreatePauseMenu();
                }

            }
        }

        private void CloseAMenu()
        {
            mainWindowGrid.Children.RemoveAt(mainWindowGrid.Children.Count - 1);
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

                mainWindowGrid.Children.Add(gameOverMenu);

                LocalGameHistoryService.SaveGameHistory(gameOverMenu, gameState);
            }
        }

    }
}