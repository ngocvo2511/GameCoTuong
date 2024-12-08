using ChessLogic.GameStates;
using ChessLogic.GameStates.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChessLogic
{
    public static class SaveService
    {
        public static void Save(GameState gameState,string fileName)
        {
            GameStateForSave gameStateForSave=ToSave(gameState);
            File.WriteAllText(fileName, JsonSerializer.Serialize(gameStateForSave, new JsonSerializerOptions { WriteIndented = true }));
        }
        public static GameState Load(string fileName)
        {
            string json = File.ReadAllText(fileName);
            GameStateForSave gameStateForSave=JsonSerializer.Deserialize<GameStateForSave>(json);
            GameState gameState = fromSave(gameStateForSave);
            return gameState;
        }
        public static GameStateForSave ToSave(GameState gameState)
        {
            GameStateForSave gameStateForSave=new GameStateForSave();
            gameStateForSave.GameType = gameState is GameState2P ? "GameState2P" : "GameStateAI";
            gameStateForSave.depth = (gameState is GameStateAI AI) ? AI.depth : null;
            gameStateForSave.CurrentPlayer = (gameState.CurrentPlayer == Player.Black) ? "Black" : "Red";
            gameStateForSave.Board = new List<string>();
            for(int i=0;i< 10; i++)
            {
                for(int j=0;j< 9; j++)
                {
                    if (gameState.Board[i, j] == null) gameStateForSave.Board.Add("n");
                    else
                    {
                        switch (gameState.Board[i, j].Type)
                        {
                            case PieceType.Advisor:
                                gameStateForSave.Board.Add((gameState.Board[i, j].Color == Player.Black) ? "bA" : "rA");
                                break;
                            case PieceType.Horse:
                                gameStateForSave.Board.Add((gameState.Board[i, j].Color == Player.Black) ? "bH" : "rH");
                                break;
                            case PieceType.Chariot:
                                gameStateForSave.Board.Add((gameState.Board[i, j].Color == Player.Black) ? "bCh" : "rCh");
                                break;
                            case PieceType.Cannon:
                                gameStateForSave.Board.Add((gameState.Board[i, j].Color == Player.Black) ? "bC" : "rC");
                                break;
                            case PieceType.Elephant:
                                gameStateForSave.Board.Add((gameState.Board[i, j].Color == Player.Black) ? "bE" : "rE");
                                break;
                            case PieceType.Soldier:
                                gameStateForSave.Board.Add((gameState.Board[i, j].Color == Player.Black) ? "bS" : "rS");
                                break;
                            case PieceType.General:
                                gameStateForSave.Board.Add((gameState.Board[i, j].Color == Player.Black) ? "bG" : "rG");
                                break;
                        }
                    }                    
                }
            }
            return gameStateForSave;
        }
        public static GameState fromSave(GameStateForSave gameStateForSave)
        {
            var mapping = new Dictionary<string, Func<Player, Piece>>
            {
                {"bA",color=>new Advisor(color)},
                {"rA",color=>new Advisor(color)},
                {"bH",color=>new Horse(color)},
                {"rH",color=>new Horse(color)},
                {"bCh",color=>new Chariot(color)},
                {"rCh",color=>new Chariot(color)},
                {"bC",color=>new Cannon(color)},
                {"rC",color=>new Cannon(color)},
                {"bE",color=>new Elephant(color)},
                {"rE",color=>new Elephant(color)},
                {"bS",color=>new Soldier(color)},
                {"rS",color=>new Soldier(color)},
                {"bG",color=>new General(color)},
                {"rG",color=>new General(color)}
            };
            Board board = new Board();
            int k = 0;
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (gameStateForSave.Board[k] == "n") board[i, j] = null;
                    else if (mapping.TryGetValue(gameStateForSave.Board[k],out var CreatePiece))
                    {
                        Player color = (gameStateForSave.Board[k].StartsWith('b')) ? Player.Black : Player.Red;
                        board[i, j] = CreatePiece(color);
                    }
                    ++k;
                }
            }
            Player currentPlayer = (gameStateForSave.CurrentPlayer == "Black") ? Player.Black : Player.Red;
            int depth = gameStateForSave.depth ?? 0;
            if (gameStateForSave.GameType == "GameStateAI")
            {
                return new GameStateAI(currentPlayer,board,depth);
            }
            return new GameState2P(currentPlayer, board);
        }
    }
}
