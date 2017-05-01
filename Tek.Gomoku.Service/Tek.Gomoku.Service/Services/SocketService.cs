using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Tek.Gomoku.Service.Services
{
    public class SocketService : ISocketService
    {
        private HashSet<WebSocket> _sockets = new HashSet<WebSocket>();

        public async Task AddSocket(WebSocketManager socketManager)
        {
            WebSocket socket = await socketManager.AcceptWebSocketAsync();

            if (!_sockets.Contains(socket))
            {
                _sockets.Add(socket);
            }

            await CloseSocketWhenNeed(socket);
        }

        public async Task BroadcastMessage(WebSocketMessage message)
        {
            var jsonString = JsonConvert.SerializeObject(
                message,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            foreach (var socket in _sockets)
            {
                var messageCharArray = Encoding.ASCII.GetBytes(jsonString);

                var buffer = new ArraySegment<byte>(messageCharArray, 0, messageCharArray.Length);

                try
                {
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch
                {
                }
            }
        }

        private async Task CloseSocketWhenNeed(WebSocket socket)
        {
            var buffer = new byte[1024 * 4];

            WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

            if (_sockets.Contains(socket))
            {
                _sockets.Remove(socket);
            }
        }
    }
}
