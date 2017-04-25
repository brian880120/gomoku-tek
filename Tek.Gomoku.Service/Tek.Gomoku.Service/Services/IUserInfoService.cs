using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Services
{
    public interface IUserInfoService
    {
        string GetUserName(ClaimsPrincipal principle);
    }
}
