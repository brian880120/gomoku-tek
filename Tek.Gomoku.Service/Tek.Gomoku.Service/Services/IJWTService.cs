using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Services
{
    public interface IJWTService
    {
        Task<JwtSecurityToken> CreateToken(string userName);
    }
}
