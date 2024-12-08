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
        public static Player currentPlayer = Player.Red;
        public async Task CreateRoom(string roomName)
        {
            if (!Rooms.ContainsKey(roomName))
            {
                var checkAny = participants.Count(p => p.Id == Context.ConnectionId);
                if(checkAny > 0)
                {
                    return;
                }
                Rooms[roomName] = new List<string>();

                Rooms[roomName].Add(Context.ConnectionId);
                participants.Add(new ClientDetail
                {
                    Id = Context.ConnectionId,
                    RoomName = roomName,
                    Color = Player.Red
                });
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

                await Clients.Caller.SendAsync("RoomCreated", roomName);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Room already exists.");
            }
        }

        public async Task JoinRoom(string roomName)
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
                    await Clients.Caller.SendAsync("Error", "Room is full.");
                    return;
                }
                

                Rooms[roomName].Add(Context.ConnectionId);
                participants.Add(new ClientDetail
                {
                    Id = Context.ConnectionId,
                    RoomName = roomName,
                    Color = Player.Black
                });

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.Caller.SendAsync("RoomJoined", roomName);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Room does not exist.");
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
            else
            {
                await Clients.Caller.SendAsync("Error", "Room does not exist.");
            }
        }


        public async Task MakeMove(int a, int b, int c, int d)
        {
            // cần đủ 2 người chơi
            var item = participants.Where(p => p.Id == Context.ConnectionId).FirstOrDefault();
            var roomCount = participants.Count(p => p.RoomName == item.RoomName);
            if(roomCount < 2)
            {
                return;
            }

            if(currentPlayer == item.Color)
            {
                await Clients.Group(item.RoomName).SendAsync("MoveTo", a, b, c, d);
                currentPlayer = item.Color == Player.Red ? Player.Black : Player.Red;

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
        public Player Color { get; set; }
    }
}
