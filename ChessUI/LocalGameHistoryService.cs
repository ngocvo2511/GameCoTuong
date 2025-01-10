using ChessLogic.GameStates.GameState;
using ChessUI.Menus;
using Newtonsoft.Json;
using System.IO;

namespace ChessUI
{
    public class GameHistory
    {
        public string WinnerImagePath { get; set; }
        public DateTime PlayTime { get; set; }
        public string GameMode { get; set; }
        public string Winner { get; set; }
    }
    public class LocalGameHistoryService
    {
        public static string filePath = "GameHistory.json";

        public static void SaveGameHistory(GameOverMenu gameOverMenu, GameState gameState)
        {
            Stack<GameHistory> history = new Stack<GameHistory>();
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                history = JsonConvert.DeserializeObject<Stack<GameHistory>>(jsonData) ?? new Stack<GameHistory>();
            }
            string gameMode = "Chế độ chơi: ";
            if (gameState is GameStateAI AI)
            {
                switch (AI.depth)
                {
                    case 2:
                        gameMode += "Chơi với máy (Dễ)";
                        break;
                    case 3:
                        gameMode += "Chơi với máy (Thường)";
                        break;
                    case 4:
                        gameMode += "Chơi với máy (Khó)";
                        break;
                }
            }
            else gameMode += "2 người chơi";

            history.Push(new GameHistory
            {
                WinnerImagePath = gameState.CurrentPlayer == ChessLogic.Player.Red ? "/Assets/Images/TuongXanh.png" : "/Assets/Images/TuongDo.png",
                PlayTime = DateTime.Now,
                GameMode = gameMode,
                Winner = gameOverMenu.WinnerText.Text + " " + gameOverMenu.ReasonText.Text
            });

            string updatedData = JsonConvert.SerializeObject(history, Formatting.Indented);
            File.WriteAllText(filePath, updatedData);
        }
        public static List<GameHistory> LoadGameHistory()
        {
            if (!File.Exists(filePath))
            {
                return new List<GameHistory>();
            }
            string jsonData = File.ReadAllText(filePath);
            List<GameHistory> history = JsonConvert.DeserializeObject<List<GameHistory>>(jsonData);

            return history;
        }
    }
}
