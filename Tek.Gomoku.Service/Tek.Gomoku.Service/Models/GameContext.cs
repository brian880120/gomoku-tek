using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tek.Gomoku.Service.Models
{
    public class GameContext : IdentityDbContext<Player>
    {
        private IConfigurationRoot _config;

        public GameContext (DbContextOptions<GameContext> options, IConfigurationRoot config)
            : base(options)
        {
            _config = config;
        }

        public DbSet<Tek.Gomoku.Service.Models.GameMove> GameMove { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:GameContext"]);
        }
    }
}
