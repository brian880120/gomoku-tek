using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Models
{
    public class GameInitializer
    {
        private readonly GameContext _context;

        public GameInitializer(GameContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            await _context.Game.ForEachAsync(p => _context.Game.Remove(p));
            await _context.GameMove.ForEachAsync(p => _context.GameMove.Remove(p));
            await _context.SaveChangesAsync();
        }
    }
}
