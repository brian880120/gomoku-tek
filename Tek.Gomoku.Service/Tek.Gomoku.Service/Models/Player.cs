using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Tek.Gomoku.Service.Models
{
    public class Player : IdentityUser
    {
        public string Side { get; set; }
    }
}
