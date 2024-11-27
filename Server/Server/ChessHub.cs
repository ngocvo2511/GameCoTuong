using Microsoft.AspNetCore.SignalR;

namespace Server
{
    public class ChessHub : Hub
    {
       
        public async Task Click(int a, int b, int c, int d)
        {
            await Clients.All.SendAsync("ClickAtPoint", a, b, c, d);
        }
    }

 
}
