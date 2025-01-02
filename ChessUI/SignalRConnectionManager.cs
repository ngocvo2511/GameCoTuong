using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;

namespace ChessUI
{
    public class SignalRConnectionManager
    {
        private static SignalRConnectionManager _instance;
        private static readonly object _lock = new object();
        public HubConnection Connection { get; private set; }

        private SignalRConnectionManager()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7013/chessHub")
                .Build();
        }

        public static SignalRConnectionManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SignalRConnectionManager();
                    }
                    return _instance;
                }
            }
        }

        public async Task StartConnectionAsync()
        {
            if(Connection.State == HubConnectionState.Connected)
            {
                return;
            }
            try
            {
                await Connection.StartAsync();
            }
            catch (HttpRequestException ex)
            {
            }
        }
    }
}
