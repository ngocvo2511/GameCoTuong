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
                .WithUrl("https://server20250109130908.azurewebsites.net/ChessHub")
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
            if(Connection.State == HubConnectionState.Connected || Connection.State == HubConnectionState.Connecting)
            {
                return;
            }

            int retryCount = 0;
            const int maxRetryAttempts = 3;
            const int delayBetweenRetries = 2;

            while (retryCount < maxRetryAttempts)
            {
                try
                {
                    await Connection.StartAsync();
                    return; 
                }
                catch (HttpRequestException ex)
                {
                    retryCount++;
                    await Task.Delay(delayBetweenRetries);
                }
            }
        }
    }
}
