using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public interface ISocketService
    {
        Task AddSocket(WebSocketManager socketManager);

        Task BroadcastMessage(WebSocketMessage message);
    }
}
