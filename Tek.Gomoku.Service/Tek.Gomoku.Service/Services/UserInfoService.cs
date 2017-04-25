using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Services
{
    public class UserInfoService : IUserInfoService
    {
        public string GetUserName(ClaimsPrincipal principal)
        {
            var claim = principal.Claims.Where(p => p.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            return claim.Value;
        }
    }
}
