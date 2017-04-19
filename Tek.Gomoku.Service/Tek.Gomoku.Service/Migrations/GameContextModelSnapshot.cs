using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Migrations
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tek.Gomoku.Service.Models.GameMove", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ColorInString");

                    b.Property<int>("ColumnIndex");

                    b.Property<string>("PlayerName");

                    b.Property<int>("RowIndex");

                    b.HasKey("ID");

                    b.ToTable("GameMove");
                });
        }
    }
}
