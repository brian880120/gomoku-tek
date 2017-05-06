using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public interface IGameService
    {
        Task SignIn(string userName, bool manToMachine);

        Task SignOut(string userName);

        Task Move(string userName, GameMove move);
    }
}
