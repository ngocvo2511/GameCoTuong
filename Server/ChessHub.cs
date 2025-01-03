using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    public class ChessHub : Hub
    {
        private static readonly Dictionary<string, List<string>> Rooms = new Dictionary<string, List<string>>();
        public static List<ClientDetail> participants = new List<ClientDetail>();
        public static Dictionary<string, Player> currentPlayer = new Dictionary<string, Player>();
        private static readonly Dictionary<int, List<string>> waitingList = new Dictionary<int, List<string>>();

        private void ResetGameState(string roomName)
        {
            if (currentPlayer.ContainsKey(roomName))
            {
                currentPlayer[roomName] = Player.Red;
            }
            else
            {
                currentPlayer.Add(roomName, Player.Red);
            }
        }

        public async Task JoinRandomMatch(string username, int time)
        {
            if (waitingList.ContainsKey(time) && waitingList[time].Count > 0)
            {
                var opponentId = waitingList[time][0];
                waitingList[time].RemoveAt(0);



                var opponent = participants.First(p => p.Id == opponentId);
                string roomName = opponent.RoomName;
                if (string.IsNullOrEmpty(username))
                    username = "Người chơi 2";
                Rooms[roomName].Add(Context.ConnectionId);
                participants.Add(new ClientDetail
                {
                    Id = Context.ConnectionId,
                    RoomName = roomName,
                    Username = username,
                    Color = Player.Black,
                    Time = time
                });

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Groups.AddToGroupAsync(opponentId, roomName);

                await Clients.Client(opponentId).SendAsync("RandomMatchFound", roomName, opponent.Username, username, Player.Red, time);
                await Clients.Caller.SendAsync("RandomMatchFound", roomName, username, opponent.Username, Player.Black, time);
                await StartGame(roomName);
            }
            else
            {
                if (!waitingList.ContainsKey(time))
                {
                    waitingList[time] = new List<string>();
                }
                waitingList[time].Add(Context.ConnectionId);
                var roomName = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(username))
                    username = "Người chơi 1";
                Rooms[roomName] = new List<string> { Context.ConnectionId };
                participants.Add(new ClientDetail
                {
                    Id = Context.ConnectionId,
                    RoomName = roomName,
                    Username = username,
                    Color = Player.Red,
                    Time = time
                });
            }
        }

        public void CancelFindMatch()
        {
            var item = participants.Where(p => p.Id == Context.ConnectionId).FirstOrDefault();
            if (item != null)
            {
                var time = item.Time;
                if (waitingList.ContainsKey(time))
                {
                    waitingList[time].Remove(Context.ConnectionId);
                }
                participants.Remove(item);
                Rooms.Remove(item.RoomName);
            }
        }

        public async Task CreateRoom(string roomName, string username, int time)
        {
            if (!Rooms.ContainsKey(roomName))
            {
                var checkAny = participants.Count(p => p.Id == Context.ConnectionId);
                if (checkAny > 0)
                {
                    return;
                }
                Rooms[roomName] = new List<string>();

                Rooms[roomName].Add(Context.ConnectionId);
                participants.Add(new ClientDetail
                {
                    Id = Context.ConnectionId,
                    RoomName = roomName,
                    Username = username,
                    Color = Player.Red,
                    Time = time
                });
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                await Clients.Caller.SendAsync("RoomCreated", roomName, username, time);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Tên phòng đã tồn tại.");
            }
        }

        public async Task JoinRoom(string roomName, string username)
        {
            if (Rooms.ContainsKey(roomName))
            {
                var checkAny = participants.Count(p => p.Id == Context.ConnectionId);
                if (checkAny > 0)
                {
                    return;
                }
                if (Rooms[roomName].Count >= 2)
                {
                    await Clients.Caller.SendAsync("Error", "Phòng đã đầy.");
                    return;
                }


                Rooms[roomName].Add(Context.ConnectionId);
                var roomCreator = participants.Where(p => p.RoomName == roomName).FirstOrDefault();
                participants.Add(new ClientDetail
                {
                    Id = Context.ConnectionId,
                    RoomName = roomName,
                    Username = username,
                    Color = Player.Black,
                    Time = roomCreator.Time
                });

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.Caller.SendAsync("RoomJoined", roomName, username, roomCreator.Time, roomCreator.Username);
                await Clients.OthersInGroup(roomName).SendAsync("PlayerJoined", roomCreator.Username, username);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Phòng không tồn tại.");
            }
        }

        public async Task StartGame(string roomName)
        {
            if (Rooms.ContainsKey(roomName))
            {
                var roomParticipants = participants.Where(p => p.RoomName == roomName).ToList();
                if (roomParticipants.Count == 2)
                {
                    ResetGameState(roomName);
                    await Task.Delay(1000);
                    for (int i = 5; i > 0; i--)
                    {

                        await Clients.Group(roomName).SendAsync("Countdown", i);
                        await Task.Delay(1000);
                    }
                    await Clients.Group(roomName).SendAsync("GameStarted", roomParticipants[0].Time);
                }

            }

        }

        public async Task LeaveRoom(string roomName)
        {
            if (Rooms.ContainsKey(roomName))
            {
                var item = participants.Where(p => p.Id == Context.ConnectionId).FirstOrDefault();
                participants.Remove(item);

                Rooms[roomName].Remove(Context.ConnectionId);
                await Clients.OthersInGroup(roomName).SendAsync("PlayerLeft", Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

                if (Rooms[roomName].Count == 0)
                {
                    Rooms.Remove(roomName);
                }
            }

        }

        public async Task GameOver(string roomName, Result result, Player current)
        {
            await Clients.Caller.SendAsync("CreateGameOver", result, current);
            Rooms.Remove(roomName);
            participants.RemoveAll(p => p.RoomName == roomName);
        }


        public async Task MakeMove(int a, int b, int c, int d)
        {
            // cần đủ 2 người chơi
            var item = participants.Where(p => p.Id == Context.ConnectionId).FirstOrDefault();
            var roomCount = participants.Count(p => p.RoomName == item.RoomName);
            if (roomCount < 2)
            {
                return;
            }

            if (currentPlayer[item.RoomName] == item.Color)
            {
                await Clients.Group(item.RoomName).SendAsync("MoveTo", a, b, c, d);
                currentPlayer[item.RoomName] = item.Color == Player.Red ? Player.Black : Player.Red;
            }


        }
    }

    public enum Player
    {
        None,
        Red,
        Black
    }

    public class ClientDetail
    {
        public string Id { get; set; }
        public string RoomName { get; set; }
        public string Username { get; set; }
        public int Time { get; set; }
        public Player Color { get; set; }
    }

    public class Result
    {
        public EndReason Reason { get; }
        public Player Winner { get; }

        public Result(Player winner, EndReason reason)
        {
            Reason = reason;
            Winner = winner;
        }

        public static Result Win(Player winner, EndReason reason)
        {
            return new Result(winner, reason);
        }

        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason);
        }
    }

    public enum EndReason
    {
        Checkmate,
        Stalemate, // van la thua chu khong hoa giong nhu co vua
        InsufficientMaterial,
        ThreefoldRepetition,
        FiftyMoveRule,
        DrawAgreed,
        Resignation,
        TimeForfeit,
        Abandoned,
        PlayerDisconnected,
        IllegalMove,
        Unknown
    }
}
