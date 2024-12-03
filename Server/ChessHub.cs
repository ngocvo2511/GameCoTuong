using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    public class ChessHub : Hub
    {
        private static readonly Dictionary<string, List<string>> Rooms = new Dictionary<string, List<string>>();
        public async Task CreateRoom(string roomName)
        {
            if (!Rooms.ContainsKey(roomName))
            {
                Rooms[roomName] = new List<string>();

                Rooms[roomName].Add(Context.ConnectionId);
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
                if(Rooms[roomName].Count >= 2)
                {
                    await Clients.Caller.SendAsync("Error", "Room is full.");
                    return;
                }
                Rooms[roomName].Add(Context.ConnectionId);
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
                Rooms[roomName].Remove(Context.ConnectionId);
                await Clients.Group(roomName).SendAsync("PlayerLeft", Context.ConnectionId);
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


        public async Task Click(int a, int b, int c, int d)
        {
            await Clients.All.SendAsync("ClickAtPoint", a, b, c, d);
        }
    }


}
