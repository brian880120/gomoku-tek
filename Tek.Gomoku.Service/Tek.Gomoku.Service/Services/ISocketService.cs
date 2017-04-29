using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Services
{
    public interface ISocketService
    {
        Task AddSocket(WebSocketManager socketManager);

        Task BroadcastMessage(WebSocketMessage message);
    }
}
